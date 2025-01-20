using Microsoft.EntityFrameworkCore;
using Orders.Controllers;
using Orders.Models;

var builder = WebApplication.CreateBuilder(args);

// Add SQLite database support, using a file as the data source
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=orders.db"));  // SQLite uses a local file for storage

// Register the OrdersController as a scoped service
builder.Services.AddScoped<OrdersController>();

// Enable API documentation using Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS to allow requests from any origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()  // Allows requests from any origin
              .AllowAnyHeader()  // Allows any HTTP headers
              .AllowAnyMethod(); // Allows any HTTP methods (GET, POST, etc.)
    });
});

var app = builder.Build();

// Use CORS policy in the middleware pipeline
app.UseCors("AllowAllOrigins");

// Enable Swagger UI and API documentation in development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Map a GET endpoint to retrieve orders by user ID
app.MapGet("/orders/{userId:guid}", async (Guid userId, OrdersController ordersController) =>
{
    var orders = ordersController.GetOrdersByUserId(userId);  // Fetch orders for the given user ID
    return orders.Any() ? Results.Ok(orders) : Results.NotFound("No orders found for the given user.");
});

// Map a POST endpoint to place new orders for a user
app.MapPost("/place_orders/{userId:guid}", async (Guid userId, List<OrderItem> items, OrdersController ordersController) =>
{
    if (items == null || !items.Any())  // Validate that order items are not null or empty
    {
        return Results.BadRequest("Order items cannot be null or empty.");
    }

    // Calculate the total cost of the order
    var total = items.Sum(item => item.Price * item.Quantity);

    // Generate a random order number
    var orderNumber = "ORD-" + new Random().Next(100000, 999999).ToString();

    // Create a new order object
    var order = new Order
    {
        UserId = userId,
        OrderNumber = orderNumber,
        OrderDate = DateTime.UtcNow,
        Status = "Completed",
        Total = total,
        Quantity = items.Sum(item => item.Quantity),  // Sum the total quantity of items
        Items = items
    };

    // Add the new order using the controller
    ordersController.AddOrder(order);

    // Return a Created result with the location of the new resource
    return Results.Created($"/orders/{userId}", order);
});

// Start the web application
app.Run();
