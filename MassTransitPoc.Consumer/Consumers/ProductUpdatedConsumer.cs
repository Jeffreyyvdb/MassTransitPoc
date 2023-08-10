using MassTransit;
using MassTransitPoc.Contracts;
using MassTransitPoc.Contracts.Entities;
using Microsoft.Extensions.Logging;

namespace MassTransitPoc.Consumer.Consumers;

public class ProductUpdatedConsumer : IConsumer<ProductUpdated>
{
    private readonly ILogger<ProductUpdatedConsumer> _logger;
    private readonly ProductService _productService;

    public ProductUpdatedConsumer(ILogger<ProductUpdatedConsumer> logger, ProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public async Task Consume(ConsumeContext<ProductUpdated> context)
    {
        Guid guid = context.Message.Guid;
        string newName = context.Message.NewName;

        var products = await _productService.LoadProductsAsync();
        Product? product = products.Find(p => p.Guid == guid);

        if(product is null)
        {
            _logger.LogWarning("Product with Guid: {Guid} not found.", guid);
            return;
        }

        product.Name = newName;
        products.Add(product);
        await _productService.SaveProductsAsync(products);

        _logger.LogInformation("Product with Guid: {Guid} updated to {NewName}.", guid, newName);
    }
}
