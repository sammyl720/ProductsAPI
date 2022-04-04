global using ProductsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductsAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options) { 
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Inventory> Inventory { get; set; }
    }
}
