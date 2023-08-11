using MassTransit;
using MassTransitPoc.Contracts;
using MassTransitPoc.Contracts.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace MassTransitPoc.Consumer.Consumers;

public class ProductAllDeletedConsumer : IConsumer<ProductAllDeleted>
{
    private readonly ILogger<ProductAllDeletedConsumer> _logger;
    private readonly ProductService _productService;

    public ProductAllDeletedConsumer(ILogger<ProductAllDeletedConsumer> logger, ProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public async Task Consume(ConsumeContext<ProductAllDeleted> context)
    {
        await _productService.SaveProductsAsync(new List<Product>());

        _logger.LogInformation("All products are deleted.");
    }
}
