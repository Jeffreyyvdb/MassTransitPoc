using MassTransit;
using MassTransitPoc.Contracts;
using MassTransitPoc.Contracts.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ProductService>(provider =>
{
    var filePath = Path.Combine("/app/shared-data", "products.json");
    var logger = provider.GetRequiredService<ILogger<ProductService>>();
    return new ProductService(filePath, logger);
});


builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    // By default, sagas are in-memory, but should be changed to a durable
    // saga repository.
    x.SetInMemorySagaRepositoryProvider();


    Uri schedulerEndpoint = new Uri("queue:product-scheduled");

    x.AddMessageScheduler(schedulerEndpoint);

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://local-rabbitmq", h =>
        {
            h.Username("publisher");
            h.Password("publisher");
        });

        cfg.UseMessageScheduler(schedulerEndpoint);
        cfg.UseMessageRetry(r => r.Immediate(5));
        cfg.ConfigureEndpoints(context);
    });

});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
