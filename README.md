## RabbitMQ Authentication & Authorization
Production environments should not use the default user and create new user accounts with generated credentials instead.

It is highly recommended to pre-configure a new user with a generated username and password or delete the guest user or at least change its password to reasonably secure generated value that won't be known to the public.

Two primary ways of authenticating a client are [username/password pairs]("https://www.rabbitmq.com/passwords.html") and [X.509 certificates]("https://en.wikipedia.org/wiki/X.509"). Username/password pairs can be used with a variety of authentication backends that verify the credentials.

### Seeding (Pre-creating) Users and Permissions
[Production environments](https://www.rabbitmq.com/production-checklist) typically need to pre-configure (seed) a number of virtual hosts, users and user permissions.

This can be done in a few ways:

- Using CLI tools
- [Definition export and import on node boot]("https://www.rabbitmq.com/definitions.html") (recommended)
- Override default credentials in configuration file(s)

### IMPORTANT

- MassTransit automatically creates queue's, topics, exchanges, etc. to do this the user used must have to right permissions. a user with tag ```administrator``` has all permissions.
-  MassTransit configuration in .NET Core consumer application like below, automatically creates exchanges with type `fanout`
```C#
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
 ```