namespace MassTransitPoc.Contracts.Entities;
public class Product
{
    public Guid Guid { get; set; }
    public string Name { get; set; } = string.Empty;

    public Product(string name) => (Guid, Name) = (Guid.NewGuid(), name);
}
