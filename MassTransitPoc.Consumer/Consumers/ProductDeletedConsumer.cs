using MassTransit;
using MassTransitPoc.Contracts;
using MassTransitPoc.Contracts.Entities;
using Microsoft.Extensions.Logging;

namespace MassTransitPoc.Consumer.Consumers;

public class ProductDeletedConsumer : IConsumer<ProductDeleted>
{
    private readonly ILogger<ProductDeletedConsumer> _logger;
    private readonly ProductService _productService;

    public ProductDeletedConsumer(ILogger<ProductDeletedConsumer> logger, ProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public async Task Consume(ConsumeContext<ProductDeleted> context)
    {
        Guid guid = context.Message.Guid;

        var products = await _productService.LoadProductsAsync();
        products.RemoveAll(p => p.Guid == guid);
        await _productService.SaveProductsAsync(products);

        _logger.LogInformation("Product with Guid: {Guid} deleted.", guid);
    }
}
