using WpfApplication.Helpers;
using WpfApplication.Models.DataModels;
using WpfApplication.Models.Interfaces;

namespace WpfApplication.Models.Services;

public class DataService : IDataService
{
    private readonly List<CityMeasurementsCapacity> _cityCapacities;
    private readonly List<Measurement> _measurements;

    public DataService(IDataGeneratorService dataGeneratorService)
    {
        _cityCapacities = dataGeneratorService.GenerateCityMeasurementsCapacity(30);
        _measurements = dataGeneratorService.GenerateMeasurement(200);
    }

    public IEnumerable<Measurement> GetAllMeasurementsByCity(string selectedCity)
    {
        return _measurements.Where(x => x.City.Contains(selectedCity));
    }

    public int GetAllAvailableCapacity(string city, DateTime date)
    {
        var cityCapacity = _cityCapacities.FirstOrDefault(c => c.City == city && Utils.CheckTwoDatesOnEqualityByDays(c.MainDate, date));
        return cityCapacity == null
            ? 0
            : cityCapacity.MeasurementPeriods.Sum(x => x.Capacity);
    }

    public IEnumerable<Measurement> GetUnmanagedMeasurementsByCity(string selectedCity)
    {
        return _measurements.Where(x => x.MeasurementDate == null && x.City.Contains(selectedCity));
    }

    public IEnumerable<Measurement> GetAllMeasurements()
    {
        return _measurements;
    }

    public IEnumerable<CityMeasurementsCapacity> GetAllCityMeasurementsCapacity()
    {
        return _cityCapacities;
    }

    public void UpdateCapacity(string city, DateTime date, int newCapacity)
    {
        var cityCapacity = _cityCapacities.FirstOrDefault(c => c.City == city);
        var period = cityCapacity?.MeasurementPeriods.FirstOrDefault(x => x.StartTime.Day == date.Day);
        if (period != null) period.Capacity = newCapacity;
    }
}