using Microsoft.EntityFrameworkCore;
using Restaurant.Api.Data;
using Restaurant.Api.Models;
using Restaurant.Api.DTOs;

namespace Restaurant.Api.Services
{
    public class MenuItemService
    {
        private readonly RestaurantContext _context;

        public MenuItemService(RestaurantContext context)
        {
            _context = context;
        }

        public async Task<List<MenuItem>> GetAllAsync()
        {
            return await _context.MenuItems
                                 .Include(m => m.Category)
                                 .OrderBy(m => m.CategoryId)
                                 .ToListAsync();
        }

        public async Task<MenuItem?> GetByIdAsync(int id)
        {
            return await _context.MenuItems
                                 .Include(m => m.Category)
                                 .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<MenuItem> CreateAsync(MenuItemDTO dto)
        {
            var menuItem = new MenuItem
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId,
                IsAvailable = dto.IsAvailable
            };

            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();
            return menuItem;
        }

        public async Task<bool> UpdateAsync(int id, MenuItemDTO dto)
        {
            var existing = await _context.MenuItems.FindAsync(id);
            if (existing == null) return false;

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.Price = dto.Price;
            existing.ImageUrl = dto.ImageUrl;
            existing.CategoryId = dto.CategoryId;
            existing.IsAvailable = dto.IsAvailable;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item == null) return false;

            _context.MenuItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
