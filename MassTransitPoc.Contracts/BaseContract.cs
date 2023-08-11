namespace MassTransitPoc.Contracts
{
    public record BaseContract
    {
        public bool ShouldThrowException { get; init; } = false;
        public int DelayInMilliseconds { get; init; } = 1000;
    }
}