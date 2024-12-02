using System;
using System.ComponentModel.DataAnnotations;

namespace ProductsApi.Models
{
    public class Order
    {
        public int Id { get; set; } // Prima, je hebt hier geen required nodig

        [Required] // Hiermee geef je aan dat dit een verplicht veld is
        public string UserId { get; set; } = string.Empty; // Hiermee voorkom je de waarschuwing

        [Required] // Hiermee geef je aan dat dit een verplicht veld is
        public string OrderNumber { get; set; } = string.Empty; // Hiermee voorkom je de waarschuwing

        public DateTime OrderDate { get; set; } = DateTime.UtcNow; // Standaardwaarde instellen

        [Required] // Hiermee geef je aan dat dit een verplicht veld is
        public string Status { get; set; } = string.Empty; // Hiermee voorkom je de waarschuwing

        [Required] // Hiermee geef je aan dat dit een verplicht veld is
        public decimal Total { get; set; } // Geen standaardwaarde, maar vereist
    }
}
