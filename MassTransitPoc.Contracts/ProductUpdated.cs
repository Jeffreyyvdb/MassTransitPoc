namespace MassTransitPoc.Contracts;
public record ProductUpdated : BaseContract
{
    public Guid Guid { get; init; } = Guid.Empty;
    public string NewName { get; init; } = string.Empty;
}