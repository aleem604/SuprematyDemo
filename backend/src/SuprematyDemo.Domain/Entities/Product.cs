namespace SuprematyDemo.Domain.Entities;
public sealed class Product {
 private Product(){} public Product(string name,string colour,decimal price){Id=Guid.NewGuid();Name=name.Trim();Colour=colour.Trim();Price=price;Sku=$"SKU-{Id.ToString()[..8].ToUpper()}";CreatedUtc=DateTimeOffset.UtcNow;UpdatedUtc=CreatedUtc;}
 public Guid Id{get;set;} public Guid? OwnerId{get;set;} public Guid? CategoryId{get;set;} public string Name{get;set;}=""; public string Sku{get;set;}=""; public string Description{get;set;}=""; public string Colour{get;set;}=""; public decimal Price{get;set;} public decimal? DiscountedPrice{get;set;} public string? ImageUrl{get;set;} public bool IsActive{get;set;}=true; public bool IsFeatured{get;set;} public int UnitsSold{get;set;} public DateTimeOffset CreatedUtc{get;set;} public DateTimeOffset UpdatedUtc{get;set;}
 public decimal EffectivePrice=>DiscountedPrice is >0 && DiscountedPrice<Price?DiscountedPrice.Value:Price;
}
