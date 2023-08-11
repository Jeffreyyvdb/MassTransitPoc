using MassTransit;
using MassTransitPoc.Consumer;
using MassTransitPoc.Contracts.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

public class Program
{

    public static async Task Main(string[] args)
    {
        Console.WriteLine("Consumer started.");
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddScoped(provider =>
                {
                    var filePath = Path.Combine("/app/shared-data", "products.json");
                    var logger = provider.GetRequiredService<ILogger<ProductService>>();
                    return new ProductService(filePath, logger);
                });

                services.AddMassTransit(x =>
                {
                    x.SetKebabCaseEndpointNameFormatter();

                    // By default, sagas are in-memory, but should be changed to a durable
                    // saga repository.
                    x.SetInMemorySagaRepositoryProvider();

                    var entryAssembly = Assembly.GetEntryAssembly();

                    x.AddConsumers(entryAssembly);

                    x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        cfg.Host("rabbitmq://local-rabbitmq", h =>
                        {
                            h.Username("product-consumer");
                            h.Password("product-consumer");
                        });

                        cfg.ConfigureEndpoints(context);
                    }));
                });

                services.AddHostedService<Worker>();
            });
}