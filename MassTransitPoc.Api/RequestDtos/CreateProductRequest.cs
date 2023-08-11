namespace MassTransitPoc.Api.RequestDtos
{
    public class CreateProductRequest : BaseRequest
    {
        public string Name { get; set; } = string.Empty;
    }
}
