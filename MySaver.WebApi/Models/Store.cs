using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApiTest.Models;

public class Store
{
    public string Name { get; set; }

    
    [Key] 
    public string Address { get; set; }

    //[AllowNull]
    //public Dictionary<DayOfWeek, string> Schedule { get; set; }

    public string DefaultSchedule { get; set; }

    /*
    public string TodaysSchedule
    {
        get
        {
            var today = DateTime.Now.DayOfWeek;

            if (Schedule?.ContainsKey(today) ?? false)
                return Schedule[today];

            return DefaultSchedule;
        }
    }
    */
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
