using MassTransit;
using MassTransitPoc.Api.RequestDtos;
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
    private readonly IMessageScheduler _messageScheduler;
    private readonly ProductService _productService;

    public ProductController(ILogger<ProductController> logger, IBus bus, IMessageScheduler messageScheduler, ProductService productService)
    {
        _logger = logger;
        _bus = bus;
        _messageScheduler = messageScheduler;
        _productService = productService;
    }

    [HttpGet(Name = "GetProducts")]
    public async Task<IActionResult> Get()
    {
        var products = await _productService.LoadProductsAsync();

        _logger.LogInformation("Products requested, returned {Count} products.", products.Count);

        return Ok(products);
    }

    [HttpPost("add",Name = "AddProduct")]
    public IActionResult Add(CreateProductRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing ProductCreated event");
        
        var message = new ProductCreated
        {
            Name = request.Name,
            ShouldThrowException = request.ShouldThrowException,
            DelayInMilliseconds = request.DelayInMilliseconds
        };

        //_messageScheduler.SchedulePublish(DateTime.Now.AddMinutes(5), message, cancellationToken);
        _bus.Publish(message, cancellationToken);
        return Ok();
    }

    [HttpPatch("update/{guid}", Name = "UpdateProduct")]
    public async Task<IActionResult> Update(Guid guid, UpdateProductRequest request)
    {
        var products = await _productService.LoadProductsAsync();

        Product? product = products.Find(p => p.Guid == guid);

        if(product is null)
        {
            _logger.LogWarning("Product with Guid: {Guid} not found.", guid);
            return NotFound(guid);
        }

        _logger.LogInformation("Publishing ProductUpdated event");

        await _bus.Publish(new ProductUpdated
        {
            Guid = guid,
            NewName = request.NewName,
            ShouldThrowException = request.ShouldThrowException,
            DelayInMilliseconds = request.DelayInMilliseconds
        });
        return Ok();
    }


    [HttpDelete("Delete/{guid}", Name = "DeleteProduct")]
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

    [HttpDelete("DeleteAll", Name = "DeleteAllProducts")]
    public async Task<IActionResult> DeleteAll()
    {

        _logger.LogInformation("Publishing delete all.");
        await _bus.Publish(new ProductAllDeleted());

        return Ok();
    }
}
