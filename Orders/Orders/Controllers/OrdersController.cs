using Microsoft.EntityFrameworkCore;
using Orders.Models;

namespace Orders.Controllers
{
    public class OrdersController
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Haal orders op voor een gebruiker
        public List<Order> GetOrdersByUserId(Guid userId)
        {
            return _context.Orders.Include(o => o.Items).Where(o => o.UserId == userId).ToList();
        }

        // Voeg een nieuwe order toe
        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();  // Sla wijzigingen op in de database
        }
    }
}
