using asp.net_web_api.Models;
using Microsoft.EntityFrameworkCore;

namespace asp.net_web_api.Data
{
    public class FoodShopDbContext(DbContextOptions<FoodShopDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products => Set<Product>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderLine> OrderLines => Set<OrderLine>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderLine>()
                .HasKey(ol => new { ol.Orderno, ol.Stockno });
        }
    }
}
