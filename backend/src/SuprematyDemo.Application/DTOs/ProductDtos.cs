namespace SuprematyDemo.Application.DTOs;
public sealed record CreateProductRequest(string Name, string Colour, decimal Price);
public sealed record ProductResponse(Guid Id, string Name, string Colour, decimal Price, DateTimeOffset CreatedUtc);
