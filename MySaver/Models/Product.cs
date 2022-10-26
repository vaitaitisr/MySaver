namespace MySaver.Models;

public class Product //Should probably implement IEquatable for better comparison functionality
{
    public string StoreName { get; set; }
    public string Name { get; set; }
    public float Amount { get; set; } = 1;
    public float Price { get; set; }
}
