using Microsoft.EntityFrameworkCore;
using Restaurant.Api.Data;
using Restaurant.Api.DTOs;
using Restaurant.Api.Models;

namespace Restaurant.Api.Services
{
    public class CategoryService
    {
        private readonly RestaurantContext _context;

        public CategoryService(RestaurantContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories
                                 .Include(c => c.MenuItems)
                                 .OrderBy(c => c.Name)
                                 .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
                                 .Include(c => c.MenuItems)
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> CreateAsync(CategoryDTO dto)
        {
            var category = new Category
            {
                Name = dto.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> UpdateAsync(int id, CategoryDTO dto)
        {
            var existing = await _context.Categories.FindAsync(id);
            if (existing == null) return false;

            existing.Name = dto.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
