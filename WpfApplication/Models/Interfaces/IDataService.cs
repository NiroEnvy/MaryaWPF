using WpfApplication.Models.DataModels;

namespace WpfApplication.Models.Interfaces;

public interface IDataService
{
    IEnumerable<Measurement> GetAllMeasurements();
    IEnumerable<CityMeasurementsCapacity>? GetAllCityMeasurementsCapacity();
    IEnumerable<Measurement> GetUnmanagedMeasurementsByCity(string selectedCity);
    IEnumerable<Measurement> GetAllMeasurementsByCity(string selectedCity);
    int GetAllAvailableCapacity(string city, DateTime date);
    void UpdateCapacity(string city, DateTime date, int newCapacity);
}