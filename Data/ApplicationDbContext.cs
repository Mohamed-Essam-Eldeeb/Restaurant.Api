using Microsoft.EntityFrameworkCore;
using Restaurant.Api.API.Models;
using Restaurant.Api.Models;

namespace Restaurant.Api.Data
{
    public class RestaurantContext : DbContext
    {
        public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options) { }

        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure decimal precision
            modelBuilder.Entity<MenuItem>()
                .Property(m => m.Price)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(10,2)");

            // Configure relationships
            modelBuilder.Entity<Category>()
                .HasMany(c => c.MenuItems)
                .WithOne(m => m.Category)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed some default categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Pizza" },
                new Category { Id = 2, Name = "Sides" },
                new Category { Id = 3, Name = "Drinks" },
                new Category { Id = 4, Name = "Desserts" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
