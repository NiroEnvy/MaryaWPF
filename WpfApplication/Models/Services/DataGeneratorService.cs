using Bogus;
using WpfApplication.Models.DataModels;
using WpfApplication.Models.Interfaces;

namespace WpfApplication.Models.Services;

public class DataGeneratorService : IDataGeneratorService
{
    private readonly List<string> _generatedCities;
    private Faker<CityMeasurementsCapacity> _cityCapacityFaker;
    private Faker<Measurement> _measurementFaker;

    public DataGeneratorService()
    {
        _generatedCities = new List<string>();
        SetupFakers();
    }

    public List<CityMeasurementsCapacity> GenerateCityMeasurementsCapacity(int cityCount)
    {
        var cities = _cityCapacityFaker.Generate(cityCount);
        _generatedCities.AddRange(cities.Select(c => c.City));
        return cities;
    }

    public List<Measurement> GenerateMeasurement(int measurementCount)
    {
        var faker = _measurementFaker.RuleFor(m => m.City, f => f.PickRandom(_generatedCities));
        return faker.Generate(measurementCount);
    }

    private void SetupFakers()
    {
        // Faker configuration for CityMeasurementsCapacity
        _cityCapacityFaker = new Faker<CityMeasurementsCapacity>()
            .StrictMode(true)
            .RuleFor(c => c.City, f => f.Address.City())
            .RuleFor(c => c.MainDate, f => f.Date.Future())
            .RuleFor(c => c.MeasurementPeriods, (f, c) => GenerateMeasurementPeriods(f, c.MainDate));


        // Faker configuration for Measurement
        var orderIds = 0;
        _measurementFaker = new Faker<Measurement>()
            .StrictMode(true)
            .RuleFor(m => m.Id, f => orderIds++)
            .RuleFor(m => m.ClientName, f => f.Name.FullName())
            .RuleFor(m => m.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(m => m.MeasurementDate, f => null)
            .RuleFor(m => m.Address, f => f.Address.FullAddress());
    }

    private static List<MeasurementPeriod> GenerateMeasurementPeriods(Faker faker, DateTime mainDate)
    {
        var periods = new List<MeasurementPeriod>();
        var periodCount = faker.Random.Number(1, 5); // Random number of periods
        var startTime = new DateTime(mainDate.Year, mainDate.Month, mainDate.Day, faker.Random.Number(8, 12), 0, 0);

        for (var i = 0; i < periodCount; i++)
        {
            var duration = TimeSpan.FromHours(faker.Random.Number(1, 3)); // Duration between 1 to 3 hours
            var endTime = startTime.Add(duration);

            periods.Add(new MeasurementPeriod {
                StartTime = startTime,
                EndTime = endTime,
                Capacity = faker.Random.Number(1, 10) // Random capacity
            });

            startTime = endTime;
        }

        return periods;
    }
}