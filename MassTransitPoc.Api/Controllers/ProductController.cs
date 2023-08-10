using MassTransit;
using MassTransitPoc.Contracts;
using MassTransitPoc.Contracts.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MassTransitPoc.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{

    private readonly ILogger<ProductController> _logger;
    private readonly IBus _bus;
    private readonly ProductService _productService;

    public ProductController(ILogger<ProductController> logger, IBus bus, ProductService productService)
    {
        _logger = logger;
        _bus = bus;
        _productService = productService;
    }

    [HttpGet(Name = "GetProducts")]
    public async Task<IActionResult> Get()
    {
        var products = await _productService.LoadProductsAsync();

        _logger.LogInformation("Products requested, returned {Count} products.", products.Count);

        return Ok(products);
    }

    [HttpPost(Name = "AddProduct")]
    public IActionResult Add(string name)
    {
        _logger.LogInformation("Publishing ProductCreated event");
        _bus.Publish(new ProductCreated { Name = name });
        return Ok();
    }

    [HttpPatch(Name = "UpdateProduct")]
    public async Task<IActionResult> Update(Guid guid, string newName)
    {
        var products = await _productService.LoadProductsAsync();

        Product? product = products.Find(p => p.Guid == guid);

        if(product is null)
        {
            _logger.LogWarning("Product with Guid: {Guid} not found.", guid);
            return NotFound(guid);
        }

        _logger.LogInformation("Publishing ProductUpdated event");

        await _bus.Publish(new ProductUpdated { Guid = guid, NewName = newName });
        return Ok();
    }

    [HttpDelete(Name = "DeleteProduct")]
    public async Task<IActionResult> Delete(Guid guid)
    {
        var products = await _productService.LoadProductsAsync();

        Product? product = products.Find(p => p.Guid == guid);

        if (product is null)
        {
            _logger.LogWarning("Product with Guid: {Guid} not found.", guid);
            return NotFound(guid);
        }

        _logger.LogInformation("Publishing ProductDeleted event");
        await _bus.Publish(new ProductDeleted { Guid = guid });

        return Ok();
    }
}
