using MassTransit;
using Microsoft.Extensions.Hosting;

namespace MassTransitPoc.Consumer;

public class Worker : BackgroundService
{
    private readonly IBus _bus;

    public Worker(IBus bus)
    {
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.CompletedTask;
       Console.WriteLine("Worker executed");
        while (true)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}
