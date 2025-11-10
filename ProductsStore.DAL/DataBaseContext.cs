using Microsoft.EntityFrameworkCore;
using ProductsStore.Models.Carts;
using ProductsStore.Models.Orders;
using ProductsStore.Models.Products;
using ProductsStore.Models.Users;

namespace ProductsStore.DAL
{
    public class DataBaseContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<User> UserRoles { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductAttributes> ProductAttributes { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasMany(c => c.ProductAttributes)
                .WithMany(p => p.Products)
                .UsingEntity(j => j.ToTable("AttributeProducts")); // Имя таблицы для промежуточной связи

            modelBuilder.Entity<Cart>()
                .HasMany(c => c.Products)
                .WithMany(p => p.Carts)
                .UsingEntity(j => j.ToTable("CartProducts"));

            modelBuilder.Entity<Order>()
                .HasMany(c => c.Products)
                .WithMany(p => p.Orders)
                .UsingEntity(j => j.ToTable("OrderProducts")); 
        }
    }
}
