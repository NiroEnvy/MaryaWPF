namespace WpfApplication.Models.DataModels;

public class CityMeasurementsCapacity
{
    public string City { get; set; }
    public DateTime MainDate { get; set; }
    public List<MeasurementPeriod> MeasurementPeriods { get; set; }
}

public class MeasurementPeriod
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Capacity { get; set; }
}