namespace MassTransitPoc.Api.RequestDtos
{
    public class UpdateProductRequest : BaseRequest
    {
        public string NewName { get; set; } = string.Empty;
    }
}
