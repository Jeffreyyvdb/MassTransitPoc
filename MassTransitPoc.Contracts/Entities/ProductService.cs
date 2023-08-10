using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MassTransitPoc.Contracts.Entities;

public class ProductService
{
    private readonly string _filePath;
    private readonly ILogger<ProductService> _logger;

    public ProductService(string filePath, ILogger<ProductService> logger)
    {
        _filePath = filePath;
        _logger = logger;

        if (!File.Exists(_filePath))
        {
            _logger.LogWarning("{FilePath} does not exist, creating file", _filePath);
            // Create the file if it doesn't exist
            File.WriteAllText(_filePath, "[]");
        }

    }

    public async Task<List<Product>> LoadProductsAsync()
    {
        using FileStream fs = File.OpenRead(_filePath);

        var products = await JsonSerializer.DeserializeAsync<List<Product>>(fs);

        if(products is null)
        {
            _logger.LogWarning("No valid products json found in {FilePath}", _filePath);
            return new List<Product>();
        }
        return products;
    }

    public async Task SaveProductsAsync(List<Product> products)
    {
        using FileStream fs = File.Create(_filePath);

        await JsonSerializer.SerializeAsync(fs, products);
    }
}
