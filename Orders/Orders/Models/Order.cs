namespace Orders.Models
{
    public class Order
    {
        // Unique identifier for the order
        public int OrderId { get; set; }

        // Unique identifier for the user who placed the order
        public Guid UserId { get; set; }

        // Order number used for tracking (nullable)
        public string? OrderNumber { get; set; }

        // Current status of the order (e.g., "Completed", "Pending") (nullable)
        public string? Status { get; set; }

        // Total cost of all items in the order
        public decimal Total { get; set; }

        // Total quantity of items in the order
        public int Quantity { get; set; }

        // Date and time when the order was placed
        public DateTime OrderDate { get; set; }

        // List of items associated with the order (nullable)
        public List<OrderItem>? Items { get; set; }
    }
}
