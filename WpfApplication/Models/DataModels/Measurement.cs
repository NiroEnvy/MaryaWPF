namespace WpfApplication.Models.DataModels;

public class Measurement
{
    // Parameterless constructor for Bogus
    public int Id { get; set; }
    public string ClientName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Phone { get; set; }
    public DateTime? MeasurementDate { get; set; }
}