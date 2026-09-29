using Microsoft.EntityFrameworkCore; using SuprematyDemo.Domain.Entities;
namespace SuprematyDemo.Infrastructure.Persistence;
public sealed class AppDbContext(DbContextOptions<AppDbContext> options):DbContext(options){
 public DbSet<Product> Products=>Set<Product>(); public DbSet<User> Users=>Set<User>(); public DbSet<Category> Categories=>Set<Category>(); public DbSet<InventoryItem> Inventory=>Set<InventoryItem>(); public DbSet<Coupon> Coupons=>Set<Coupon>(); public DbSet<Order> Orders=>Set<Order>(); public DbSet<OrderItem> OrderItems=>Set<OrderItem>();
 protected override void OnModelCreating(ModelBuilder b){
  var p=b.Entity<Product>();p.HasKey(x=>x.Id);p.Property(x=>x.Name).HasMaxLength(200).IsRequired();p.Property(x=>x.Sku).HasMaxLength(64);p.HasIndex(x=>x.Sku).IsUnique();p.Property(x=>x.Price).HasPrecision(18,2);p.Property(x=>x.DiscountedPrice).HasPrecision(18,2);
  var u=b.Entity<User>();u.HasKey(x=>x.Id);u.Property(x=>x.Email).HasMaxLength(320).IsRequired();u.HasIndex(x=>x.Email).IsUnique();u.Property(x=>x.PasswordHash).IsRequired();
  b.Entity<Category>().HasIndex(x=>x.Slug).IsUnique(); b.Entity<InventoryItem>().HasIndex(x=>x.ProductId).IsUnique(); b.Entity<Coupon>().HasIndex(x=>x.Code).IsUnique(); b.Entity<Order>().HasMany(x=>x.Items).WithOne().HasForeignKey(x=>x.OrderId).OnDelete(DeleteBehavior.Cascade);
 }
}
