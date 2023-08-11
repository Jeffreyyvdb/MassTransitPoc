namespace MassTransitPoc.Contracts;
public record ProductCreated : BaseContract
{
    public string Name { get;init;} = string.Empty;
}