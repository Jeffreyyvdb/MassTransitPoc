## Docker stuff

- It is possible to run multiple instances of a consumer image by adding it at a service 2 times in the compose.
- I used a json file to persist changes in this repo. To make sure the publisher (api) and the consumer have the same .json file i added a volume to the compose file and added it to the services.
- using the depends_on property i made sure the rabbitmq service was started before the other services.
- i also added a volume for rabbitmq-data see RabbitMQ configuration and definitions below. 

## RabbitMQ configuration and definitions

- It is possible to configure RabbitMQ and import defintions when the service starts.
- in the defintions.json you can create users for the different clients with their permissions, to make sure MassTransit can configure some exchanges/queues etc. you need to make sure the users have the right permissions

## Authentication 
Authentication to RabbitMQ with MassTransit allows username & password and X.509 Certificates.
By using plugins and some custom solutions it is possible to perform authentication other ways like OAuth2, but im not sure how hard that would be to integrate with MassTransit, seems like a bit overkill

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

### Exceptions : 

It is possible to automatically retry when exceptions happen : `cfg.UseMessageRetry(r => r.Immediate(5));`
it is possible to configure the retries see : [Retry Configuration](https://masstransit.io/documentation/concepts/exceptions#retry-configuration)

using Exception filters we may be able to make decisions based on the exceptions. If the exception has something to do with a database timeout, we might want to retry it in a few minutes. If it is an exception that was throwed because of bad user input, we might want to move it to an error queue, send an email to support/developers to check what is wrong or something like that.
https://masstransit.io/documentation/concepts/exceptions

### Scheduling

It is possible to schedule things.

MassTransit supports two different methods of message scheduling:
1. Scheduler-based, using either Quartz.NET or Hangfire, where the scheduler runs in a service and schedules messages using a queue.
2. Transport-based, using the transports built-in message scheduling/delay capabilities. In some cases, such as RabbitMQ, this requires an additional plug-in to be installed and configured.

_Recurring schedules are only supported by Quartz.NET or Hangfire._

I think we should use something like quartz or hangfire to use recurring schedules 

### Sagas

```
The ability to orchestrate a series of events is a powerful feature, and MassTransit makes this possible.
A saga is a long-lived transaction managed by a coordinator. Sagas are initiated by an event, sagas orchestrate events, and sagas maintain the state of the overall transaction. Sagas are designed to manage the complexity of a distributed transaction without locking and immediate consistency. They manage state and track any compensations required if a partial failure occurs.
```

I think we can use sagas to create execute long tasks with alot of dependencies, should look in to more.

It is possible to persist the SAGAs in EF CORE.

### IMPORTANT

- MassTransit automatically creates queue's, topics, exchanges, etc. to do this the user used must have to right permissions. a user with tag ```administrator``` has all permissions.
