namespace MassTransitPoc.Contracts;
public record ProductUpdated
{
    public Guid Guid { get; init; } = Guid.Empty;
    public string NewName { get; init; } = string.Empty;
}