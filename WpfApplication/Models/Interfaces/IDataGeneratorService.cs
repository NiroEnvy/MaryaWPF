using WpfApplication.Models.DataModels;

namespace WpfApplication.Models.Interfaces;

public interface IDataGeneratorService
{
    List<CityMeasurementsCapacity> GenerateCityMeasurementsCapacity(int cityCount);

    List<Measurement> GenerateMeasurement(int measurementCount);
}