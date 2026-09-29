using Moq; using SuprematyDemo.Application.Abstractions; using SuprematyDemo.Application.DTOs; using SuprematyDemo.Application.Products; using SuprematyDemo.Domain.Entities;
namespace SuprematyDemo.UnitTests;
public sealed class ProductServiceTests
{
 [Fact] public async Task Create_persists_and_maps_product(){var repo=new Mock<IProductRepository>();var sut=new ProductService(repo.Object);var result=await sut.CreateAsync(new("Chair","Red",99.5m),default);Assert.Equal("Chair",result.Name);repo.Verify(x=>x.AddAsync(It.IsAny<Product>(),default),Times.Once);repo.Verify(x=>x.SaveChangesAsync(default),Times.Once);}
 [Fact] public async Task Get_passes_colour_filter(){var repo=new Mock<IProductRepository>();repo.Setup(x=>x.GetAsync("Blue",default)).ReturnsAsync([new Product("Desk","Blue",10)]);var result=await new ProductService(repo.Object).GetAsync(" Blue ",default);Assert.Single(result);Assert.Equal("Blue",result[0].Colour);}
}
