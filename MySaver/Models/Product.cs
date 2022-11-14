namespace MySaver.Models;

public class Product : IEquatable<Product>
{
    public string StoreName { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public float Amount { get; set; } = 1;
    public float Price { get; set; }

    public bool Equals(Product other)
    {
        return (StoreName?.Equals(other.StoreName) ?? false) &&
               (Name?.Equals(other.Name) ?? false);
    }
}
