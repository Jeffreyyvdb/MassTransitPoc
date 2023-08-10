namespace MassTransitPoc.Contracts.Entities;
public class Product
{
    public static readonly List<Product> Products = new(){
        new ("Bike"),
        new ("Computer"),
        new ("Coffee mug")
    };

    public Guid Guid { get; set; }
    public string Name { get; set; } = string.Empty;

    public Product(string name) => (Guid, Name) = (Guid.NewGuid(), name);
}
