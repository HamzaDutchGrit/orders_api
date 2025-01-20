namespace Orders.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public Guid UserId { get; set; }

        public string? OrderNumber { get; set; }
        public string? Status { get; set; }
        public decimal Total { get; set; }
        public int Quantity { get; set; }

        // Voeg OrderDate toe als een DateTime eigenschap
        public DateTime OrderDate { get; set; } // <-- Hier is de ontbrekende eigenschap

        public List<OrderItem>? Items { get; set; }
    }
}
