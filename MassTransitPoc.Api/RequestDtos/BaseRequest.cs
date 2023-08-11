namespace MassTransitPoc.Api.RequestDtos
{
    public class BaseRequest
    {
        public bool ShouldThrowException{ get; set; } = false;
        public int DelayInMilliseconds { get; set; } = 1000;
    }
}
