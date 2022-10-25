namespace MySaver.Models;

public class Store
{
    public string Name { get; set; }
    public string Address { get; set; }
    public Dictionary<DayOfWeek, string> Schedule { get; set; }
    public string DefaultSchedule { get; set; }

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

    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
