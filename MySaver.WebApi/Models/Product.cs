using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WebApiTest.Models;

public class Product
{
    public string StoreName { get; set; } = String.Empty;

    [Key]
    public string Name { get; set; } = String.Empty;

    public decimal UnitPrice { get; set; }
}
