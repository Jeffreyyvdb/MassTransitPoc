using MassTransit;
using MassTransitPoc.Contracts;
using MassTransitPoc.Contracts.Entities;
using Microsoft.Extensions.Logging;

namespace MassTransitPoc.Consumer.Consumers;

public class ProductCreatedConsumer : IConsumer<ProductCreated>
{
    private readonly ILogger<ProductCreatedConsumer> _logger;

    public ProductCreatedConsumer(ILogger<ProductCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<ProductCreated> context)
    {
        string name = context.Message.Name;
        Product product = new(name);
        Product.Products.Add(product);
        _logger.LogInformation("Product: {Name} added with Guid: {Guid}.", product.Name, product.Guid);
        _logger.LogInformation("ProductCreated Consumed");
        return Task.CompletedTask;
    }
}
