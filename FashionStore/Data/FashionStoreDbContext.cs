using FashionStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Data
{
    public class FashionStoreDbContext : IdentityDbContext<ApplicationUser>
    {
        public FashionStoreDbContext(DbContextOptions<FashionStoreDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers {  get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<Material> Materials { get; set; }

        
    }
}
