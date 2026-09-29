using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuprematyDemo.Domain.Entities;

namespace SuprematyDemo.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public const string DemoEmail = "demo@suprematy.local";
    public const string DemoPassword = "Demo123!";

    public static async Task SeedAsync(AppDbContext db, IPasswordHasher<User> hasher, CancellationToken ct = default)
    {
        await db.Database.EnsureCreatedAsync(ct);
        var user = await db.Users.FirstOrDefaultAsync(x => x.Email == DemoEmail, ct);
        if (user is null)
        {
            user = new User(DemoEmail, "Demo User") { IsAdmin = true };
            user.SetPasswordHash(hasher.HashPassword(user, DemoPassword));
            db.Users.Add(user);
            await db.SaveChangesAsync(ct);
        }
        if (!user.IsAdmin) { user.IsAdmin = true; await db.SaveChangesAsync(ct); }

        if (!await db.Categories.AnyAsync(ct))
        {
            db.Categories.AddRange(
                new Category { Name="Electronics", Slug="electronics" }, new Category { Name="Computers", Slug="computers" },
                new Category { Name="Mobile", Slug="mobile" }, new Category { Name="Audio", Slug="audio" },
                new Category { Name="Gaming", Slug="gaming" }, new Category { Name="Home", Slug="home" },
                new Category { Name="Kitchen", Slug="kitchen" }, new Category { Name="Fashion", Slug="fashion" },
                new Category { Name="Fitness", Slug="fitness" }, new Category { Name="Lifestyle", Slug="lifestyle" });
            await db.SaveChangesAsync(ct);
        }

        if (!await db.Products.AnyAsync(ct))
        {
            var categories = await db.Categories.OrderBy(x=>x.Name).ToListAsync(ct);
            var adjectives = new[] { "Apex", "Nova", "Prime", "Elite", "Urban", "Pro", "Air", "Studio", "Smart", "Ultra" };
            var nouns = new[] { "Headphones", "Backpack", "Keyboard", "Watch", "Speaker", "Camera", "Sneakers", "Blender", "Monitor", "Lamp" };
            var colours = new[] { "Black", "Blue", "Silver", "Red", "White", "Green", "Graphite", "Navy" };
            for (var i = 1; i <= 100; i++)
            {
                var regular = 24m + (i * 7.35m % 475m);
                var onSale = i % 3 == 0 || i % 7 == 0;
                var p = new Product($"{adjectives[(i-1)%adjectives.Length]} {nouns[(i-1)%nouns.Length]} {i:000}", colours[i%colours.Length], decimal.Round(regular,2))
                {
                    Id = Guid.Parse($"10000000-0000-0000-0000-{i:000000000000}"),
                    OwnerId = user.Id,
                    CategoryId = categories[(i-1)%categories.Count].Id,
                    Sku = $"SUP-{i:0000}",
                    Description = $"Premium {nouns[(i-1)%nouns.Length].ToLowerInvariant()} from the Suprematy demo catalog. Seeded with realistic pricing, inventory and sales data.",
                    DiscountedPrice = onSale ? decimal.Round(regular * (i%2==0 ? .80m : .90m),2) : null,
                    ImageUrl = $"https://picsum.photos/seed/suprematy-{i}/900/700",
                    IsFeatured = i <= 16,
                    IsActive = i % 23 != 0,
                    UnitsSold = Math.Max(2, 620 - i * 5)
                };
                db.Products.Add(p);
                db.Inventory.Add(new InventoryItem { ProductId=p.Id, QuantityOnHand=i%19==0?0:(i*13)%145, ReorderLevel=10+(i%8), Warehouse=i%4==0?"North":"Main" });
            }
            db.Coupons.AddRange(
                new Coupon { Code="WELCOME10", DiscountPercent=10, MinimumOrder=25, IsActive=true },
                new Coupon { Code="SALE20", DiscountPercent=20, MinimumOrder=150, IsActive=true },
                new Coupon { Code="VIP25", DiscountPercent=25, MinimumOrder=300, IsActive=true, ExpiresUtc=DateTimeOffset.UtcNow.AddMonths(6) });
            await db.SaveChangesAsync(ct);
        }

        if (!await db.Orders.AnyAsync(x=>x.UserId==user.Id,ct))
        {
            var products=await db.Products.OrderBy(x=>x.Sku).Take(24).ToListAsync(ct);
            var statuses=new[]{"Delivered","Delivered","Shipped","Processing","Confirmed","Pending","Cancelled"};
            for(var o=0;o<7;o++)
            {
                var selected=products.Skip(o*3).Take(3).ToList();
                var order=new Order { Number=$"SUP-DEMO-{1001+o}", UserId=user.Id, CustomerName=user.DisplayName, CustomerEmail=user.Email,
                    ShippingAddress="Demo Address, Lahore", Status=statuses[o], PaymentMethod=o%2==0?"Card":"COD",
                    PaymentStatus=statuses[o]=="Cancelled"?"Cancelled":(statuses[o]=="Delivered"||o%2==0?"Paid":"Pending"), CreatedUtc=DateTimeOffset.UtcNow.AddDays(-(o+1)*8), DeliveredUtc=statuses[o]=="Delivered"?DateTimeOffset.UtcNow.AddDays(-(o+1)*8+3):null };
                foreach(var p in selected) order.Items.Add(new OrderItem { ProductId=p.Id, ProductName=p.Name, UnitPrice=p.EffectivePrice, Quantity=1+(o%2) });
                order.Subtotal=order.Items.Sum(x=>x.UnitPrice*x.Quantity); order.Discount=o%3==0?decimal.Round(order.Subtotal*.10m,2):0; order.Shipping=order.Subtotal>=100?0:9.99m; order.Total=order.Subtotal-order.Discount+order.Shipping;
                db.Orders.Add(order);
            }
            await db.SaveChangesAsync(ct);
        }
    }
}
