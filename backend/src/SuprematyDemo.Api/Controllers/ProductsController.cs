using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuprematyDemo.Application.DTOs;
using SuprematyDemo.Application.Products;
namespace SuprematyDemo.Api.Controllers;
[ApiController,Route("api/products"),Authorize]
public sealed class ProductsController(ProductService service):ControllerBase
{
    [HttpGet] public async Task<ActionResult<IReadOnlyList<ProductResponse>>> Get([FromQuery]string? colour,CancellationToken ct)=>Ok(await service.GetAsync(colour,ct));
    [HttpPost] public async Task<ActionResult<ProductResponse>> Create(CreateProductRequest request,CancellationToken ct)
    {
        if(string.IsNullOrWhiteSpace(request.Name)||string.IsNullOrWhiteSpace(request.Colour)||request.Price<0) return ValidationProblem();
        var result=await service.CreateAsync(request,ct); return Created($"/api/products/{result.Id}",result);
    }
}
