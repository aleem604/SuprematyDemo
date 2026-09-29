using Microsoft.EntityFrameworkCore;
using SuprematyDemo.Application.Abstractions;
using SuprematyDemo.Domain.Entities;
using SuprematyDemo.Infrastructure.Persistence;
namespace SuprematyDemo.Infrastructure.Repositories;
public sealed class ProductRepository(AppDbContext db) : IProductRepository
{
    public Task AddAsync(Product p,CancellationToken ct)=>db.Products.AddAsync(p,ct).AsTask();
    public async Task<IReadOnlyList<Product>> GetAsync(string? colour,CancellationToken ct)
    {
        var q=db.Products.AsNoTracking();
        if(colour is not null) q=q.Where(x=>x.Colour.ToLower()==colour.ToLower());
        return await q.OrderBy(x=>x.Name).ToListAsync(ct);
    }
    public Task SaveChangesAsync(CancellationToken ct)=>db.SaveChangesAsync(ct);
}
