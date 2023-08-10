namespace MassTransitPoc.Contracts;
public record ProductDeleted
{
    public Guid Guid { get; init; } = Guid.Empty;
}