using SuprematyDemo.Domain.Entities;
namespace SuprematyDemo.Application.Abstractions;
public interface IProductRepository
{
    Task AddAsync(Product product, CancellationToken ct);
    Task<IReadOnlyList<Product>> GetAsync(string? colour, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
