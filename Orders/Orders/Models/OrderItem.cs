﻿public class OrderItem
{
    public int OrderItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int OrderId { get; set; }
}
