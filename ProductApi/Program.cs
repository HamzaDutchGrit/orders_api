using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductsApi.Data; // Namespace voor ApplicationDbContext
using ProductsApi.Models; // Namespace voor ApplicationUser en Order
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// 1. Voeg CORS-services toe aan de container
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin() // Sta alle origins toe
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 2. Voeg andere services toe aan de container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configureer Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false; // Pas wachtwoordvereisten aan als nodig
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Voeg Newtonsoft.Json toe
builder.Services.AddControllers()
    .AddNewtonsoftJson(); // Zorg ervoor dat deze regel correct is

// JWT-configuratie
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is not configured."))),
    };
});

// Swagger-configuratie
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product API", Version = "v1" });
});

// Voeg DbSet voor Orders toe
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Zorg dat de database gemigreerd is bij het starten
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate(); // Voer migraties uit
}

// Configureer de HTTP-aanvraagpipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 3. Gebruik het CORS-beleid v��r authentication en authorization
app.UseCors("AllowAllOrigins"); // Gebruik de nieuwe policy

app.UseAuthentication();
app.UseAuthorization();

// POST endpoint voor het aanmaken van een order
app.MapPost("/orders", async (ApplicationDbContext dbContext, Order order) =>
{
    dbContext.Orders.Add(order);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/orders/{order.Id}", order); // Retourneer de aangemaakte order
});

// GET endpoint voor het ophalen van orders van een specifieke gebruiker
app.MapGet("/orders/{userId}", async (ApplicationDbContext dbContext, string userId) =>
{
    var orders = await dbContext.Orders.Where(o => o.UserId == userId).ToListAsync();
    return Results.Ok(orders);
});

// Laat de applicatie draaien
app.Run();
