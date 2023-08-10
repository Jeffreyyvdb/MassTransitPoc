using MassTransit;
using MassTransitPoc.Contracts;
using MassTransitPoc.Contracts.Entities;
using Microsoft.Extensions.Logging;

namespace MassTransitPoc.Consumer.Consumers;

public class ProductUpdatedConsumer : IConsumer<ProductUpdated>
{
    private readonly ILogger<ProductUpdatedConsumer> _logger;

    public ProductUpdatedConsumer(ILogger<ProductUpdatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<ProductUpdated> context)
    {
        Guid guid = context.Message.Guid;
        string newName = context.Message.NewName;

        Product? product = Product.Products.Find(p => p.Guid == guid);

        if(product is null)
        {
            _logger.LogWarning("Product with Guid: {Guid} not found.", guid);
            return Task.CompletedTask;
        }

        product.Name = newName;
        _logger.LogInformation("Product with Guid: {Guid} updated to {NewName}.", guid, newName);
        return Task.CompletedTask;
    }
}
