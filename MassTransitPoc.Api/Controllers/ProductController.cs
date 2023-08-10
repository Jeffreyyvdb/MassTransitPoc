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

    public ProductController(ILogger<ProductController> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    [HttpGet(Name = "GetProducts")]
    public IActionResult Get()
    {
        _logger.LogInformation("Products requested, returned {Count} products.", Product.Products.Count);

        return Ok(Product.Products);
    }

    [HttpPost(Name = "AddProduct")]
    public IActionResult Add(string name)
    {
        _logger.LogInformation("Publishing ProductCreated event");
        _bus.Publish(new ProductCreated { Name = name });
        return Ok();
    }

    [HttpPatch(Name = "UpdateProduct")]
    public IActionResult Update(Guid guid, string newName)
    {
        Product? product = Product.Products.Find(p => p.Guid == guid);

        if(product is null)
        {
            _logger.LogWarning("Product with Guid: {Guid} not found.", guid);
            return NotFound(guid);
        }

        _logger.LogInformation("Publishing ProductUpdated event");
        _bus.Publish(new ProductUpdated { Guid = guid, NewName = newName });
        return Ok();
    }

    [HttpDelete(Name = "DeleteProduct")]
    public IActionResult Delete(Guid guid)
    {
        Product? product = Product.Products.Find(p => p.Guid == guid);
        if(product is null)
        {
            _logger.LogWarning("Product with Guid: {Guid} not found.", guid);
            return NotFound(guid);
        }

        _logger.LogInformation("Publishing ProductDeleted event");
        _bus.Publish(new ProductDeleted { Guid = guid });

        return Ok(Product.Products);
    }
}
