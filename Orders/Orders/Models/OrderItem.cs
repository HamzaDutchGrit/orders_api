public class OrderItem
{
    // Unique identifier for the order item
    public int OrderItemId { get; set; }

    // Name of the item being ordered
    public string Name { get; set; } = string.Empty;

    // Quantity of the item ordered
    public int Quantity { get; set; }

    // Price of a single unit of the item
    public decimal Price { get; set; }

    // Identifier for the associated order
    public int OrderId { get; set; }
}
