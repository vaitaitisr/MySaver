namespace MySaver.Models;

public class Product : IComparable<Product>
{
    public string StoreName { get; set; }
    public string Name { get; set; }
    public float Amount { get; set; } = 1;
    public float Price { get; set; }

    public int CompareTo(Product other)
    {
        return Price.CompareTo(other.Price);
    }
}
