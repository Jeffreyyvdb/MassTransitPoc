using MassTransit;
using MassTransitPoc.Contracts;
using MassTransitPoc.Contracts.Entities;
using Microsoft.Extensions.Logging;

namespace MassTransitPoc.Consumer.Consumers;

public class ProductCreatedConsumer : IConsumer<ProductCreated>
{
    private readonly ILogger<ProductCreatedConsumer> _logger;
    private readonly ProductService _productService;

    public ProductCreatedConsumer(ILogger<ProductCreatedConsumer> logger, ProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public async Task Consume(ConsumeContext<ProductCreated> context)
    {
        string name = context.Message.Name;
        Product product = new(name);

        var products = await _productService.LoadProductsAsync();
        products.Add(product);
        await _productService.SaveProductsAsync(products);

        _logger.LogInformation("Product: {Name} added with Guid: {Guid}.", product.Name, product.Guid);
        _logger.LogInformation("ProductCreated Consumed");
    }
}
