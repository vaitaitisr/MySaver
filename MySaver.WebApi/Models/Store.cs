using System.ComponentModel.DataAnnotations;

namespace WebApiTest.Models;

public class Store
{
    public string Name { get; set; }

    [Key]
    public string Address { get; set; }

    public string DefaultSchedule { get; set; }
    public string? SaturdaySchedule { get; set; }
    public string? SundaySchedule { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
