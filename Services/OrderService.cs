using Microsoft.EntityFrameworkCore;
using Restaurant.Api.API.Models;
using Restaurant.Api.Data;
using Restaurant.Api.Models;

namespace Restaurant.Api.Services
{
    public class OrderService
    {
        private readonly RestaurantContext _context;

        public OrderService(RestaurantContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.MenuItem)
                .Include(o => o.User)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.MenuItem)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> CreateAsync(Order order)
        {
            if (order.Items == null || !order.Items.Any())
                throw new ArgumentException("Order must contain at least one item.");

            decimal totalPrice = 0;

            foreach (var item in order.Items)
            {
                var menuItem = await _context.MenuItems.FindAsync(item.MenuItemId);
                if (menuItem == null)
                    throw new ArgumentException($"MenuItem with ID {item.MenuItemId} does not exist.");

                if (!menuItem.IsAvailable)
                    throw new ArgumentException($"MenuItem '{menuItem.Name}' is not available.");

                if (item.Quantity <= 0)
                    throw new ArgumentException($"Quantity for '{menuItem.Name}' must be greater than 0.");

                totalPrice += menuItem.Price * item.Quantity;
            }

            order.TotalPrice = totalPrice;
            order.Status = OrderStatus.Pending;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }



        public async Task<bool> UpdateAsync(int id, Order updatedOrder)
        {
            var existingOrder = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (existingOrder == null)
                return false;

            existingOrder.CustomerName = updatedOrder.CustomerName;
            existingOrder.DeliveryAddress = updatedOrder.DeliveryAddress;
            existingOrder.PhoneNumber = updatedOrder.PhoneNumber;
            existingOrder.Status = updatedOrder.Status;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStatusAsync(int id, OrderStatus newStatus)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return false;

            order.Status = newStatus;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
