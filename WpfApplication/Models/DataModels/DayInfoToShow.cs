namespace WpfApplication.Models.DataModels;

public class DayInfoToShow
{
    public string DayName { get; set; }
    public DateTime Date { get; set; }
    public int Capacity { get; set; }
    public List<Measurement> People { get; set; } = new();
}