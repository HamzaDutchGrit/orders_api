using Microsoft.EntityFrameworkCore;
using Orders.Models;

public class ApplicationDbContext : DbContext
{
    // Represents the Orders table in the database
    public DbSet<Order> Orders { get; set; }

    // Represents the OrderItems table in the database
    public DbSet<OrderItem> OrderItems { get; set; }

    // Constructor that accepts DbContext options and passes them to the base DbContext
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Configures the model relationships and constraints
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configures a one-to-many relationship between Order and OrderItem
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Items)  // Specifies that an Order has many Items
            .WithOne()              // Specifies that each Item has one associated Order
            .HasForeignKey("OrderId");  // Sets the foreign key in OrderItem to reference OrderId
    }
}
