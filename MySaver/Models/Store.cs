namespace MySaver.Models;

public class Store
{
    public string Name { get; set; }
    public string Address { get; set; }

    public string DefaultSchedule { get; set; }
    public string SaturdaySchedule { get; set; }
    public string SundaySchedule { get; set; }

    public string TodaysSchedule
    {
        get
        {
            var today = DateTime.Now.DayOfWeek;

            if (today == DayOfWeek.Saturday)
                return SaturdaySchedule ?? DefaultSchedule;
            if (today == DayOfWeek.Sunday)
                return SundaySchedule ?? DefaultSchedule;

            return DefaultSchedule;
        }
    }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Distance { get; set; }
}
