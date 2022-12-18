using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace WebApiTest.Models;

[PrimaryKey("StoreName", "Name")]
public class Product
{
    
    public string StoreName { get; set; } = String.Empty;

    public string Name { get; set; } = String.Empty;

    public decimal UnitPrice { get; set; }
}
