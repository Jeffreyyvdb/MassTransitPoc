using MassTransit;
using MassTransitPoc.Contracts;
using MassTransitPoc.Contracts.Entities;
using Microsoft.Extensions.Logging;

namespace MassTransitPoc.Consumer.Consumers;

internal class ProductDeletedConsumer : IConsumer<ProductDeleted>
{
    private readonly ILogger<ProductDeletedConsumer> _logger;

    public ProductDeletedConsumer(ILogger<ProductDeletedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<ProductDeleted> context)
    {
        Guid guid = context.Message.Guid;

        Product.Products.RemoveAll(p => p.Guid == guid);
        _logger.LogInformation("Product with Guid: {Guid} deleted.", guid);
        return Task.CompletedTask;
    }
}
