using SuprematyDemo.Application.Abstractions;
using SuprematyDemo.Application.DTOs;
using SuprematyDemo.Domain.Entities;
namespace SuprematyDemo.Application.Products;
public sealed class ProductService(IProductRepository repository)
{
    public async Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken ct)
    {
        var product = new Product(request.Name, request.Colour, request.Price);
        await repository.AddAsync(product, ct);
        await repository.SaveChangesAsync(ct);
        return Map(product);
    }
    public async Task<IReadOnlyList<ProductResponse>> GetAsync(string? colour, CancellationToken ct) =>
        (await repository.GetAsync(string.IsNullOrWhiteSpace(colour) ? null : colour.Trim(), ct)).Select(Map).ToList();
    private static ProductResponse Map(Product p) => new(p.Id,p.Name,p.Colour,p.Price,p.CreatedUtc);
}
