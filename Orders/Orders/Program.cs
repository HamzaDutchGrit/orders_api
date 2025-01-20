using Microsoft.EntityFrameworkCore;
using Orders.Controllers;
using Orders.Models;

var builder = WebApplication.CreateBuilder(args);

// Voeg SQLite-databaseondersteuning toe
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=orders.db"));  // SQLite gebruikt een bestand

builder.Services.AddScoped<OrdersController>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()  // Staat alle origins toe
              .AllowAnyHeader()  // Staat elke header toe
              .AllowAnyMethod(); // Staat elke HTTP-methode toe
    });
});

var app = builder.Build();

// Voeg de CORS-middleware toe aan de pipeline
app.UseCors("AllowAllOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/orders/{userId:guid}", async (Guid userId, OrdersController ordersController) =>
{
    var orders = ordersController.GetOrdersByUserId(userId);
    return orders.Any() ? Results.Ok(orders) : Results.NotFound("No orders found for the given user.");
});

app.MapPost("/place_orders/{userId:guid}", async (Guid userId, List<OrderItem> items, OrdersController ordersController) =>
{
    if (items == null || !items.Any())
    {
        return Results.BadRequest("Order items cannot be null or empty.");
    }

    var total = items.Sum(item => item.Price * item.Quantity);
    var orderNumber = "ORD-" + new Random().Next(100000, 999999).ToString();

    var order = new Order
    {
        UserId = userId,
        OrderNumber = orderNumber,
        OrderDate = DateTime.UtcNow,
        Status = "Completed",
        Total = total,
        Quantity = items.Sum(item => item.Quantity),
        Items = items
    };

    ordersController.AddOrder(order);
    return Results.Created($"/orders/{userId}", order);
});

app.Run();
