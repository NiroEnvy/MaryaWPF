using WpfApplication.Models.DataModels;
using WpfApplication.Models.Interfaces;
using WpfApplication.ViewModels;

namespace TestProject1.ViewModels;

public class MeasurementSchedulerViewModelTest
{
    private readonly List<CityMeasurementsCapacity> _mockCityCapacities;
    private readonly Mock<IDataService> _mockDataService;
    private readonly List<Measurement> _mockMeasurements;

    public MeasurementSchedulerViewModelTest()
    {
        _mockDataService = new Mock<IDataService>();

        var now = DateTime.Now;
        // Setup mock data
        _mockCityCapacities = new List<CityMeasurementsCapacity> {
            new() {
                City = "CityA",
                MainDate = new DateTime(2023, 1, 1),
                MeasurementPeriods = new List<MeasurementPeriod> {
                    new() {
                        StartTime = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0),
                        EndTime = new DateTime(now.Year, now.Month, now.Day, 10, 0, 0),
                        Capacity = 10
                    }
                }
            },
            new() {
                City = "CityB",
                MainDate = new DateTime(2023, 1, 2),
                MeasurementPeriods = new List<MeasurementPeriod> {
                    new() {
                        StartTime = new DateTime(now.Year, now.Month, now.Day, 11, 0, 0),
                        EndTime = new DateTime(now.Year, now.Month, now.Day, 12, 0, 0),
                        Capacity = 5
                    }
                }
            }
        };

        _mockMeasurements = new List<Measurement> {
            new() {
                Id = 1,
                ClientName = "John Doe",
                Address = "123 Main St, CityA",
                City = "CityA",
                Phone = "555-1234",
                MeasurementDate = new DateTime(2023, 1, 1)
            },
            new() {
                Id = 2,
                ClientName = "Jane Smith",
                Address = "456 Elm St, CityB",
                City = "CityB",
                Phone = "555-5678",
                MeasurementDate = null
            }
        };

        // Setup mock behavior
        _mockDataService.Setup(s => s.GetAllMeasurements()).Returns(_mockMeasurements);
        _mockDataService.Setup(s => s.GetAllCityMeasurementsCapacity()).Returns(_mockCityCapacities);
    }

    [Fact]
    public void Constructor_ShouldInitializeMeasurementsAndCapacities()
    {
        // Arrange & Act
        var viewModel = new MeasurementSchedulerViewModel(_mockDataService.Object);

        // Assert
        viewModel.Measurements.Should().Contain(m => m.MeasurementDate == null);
        viewModel.CityMeasurementsCapacity.Should().HaveCount(_mockCityCapacities.Count);
    }

    [Fact]
    public void SortMeasurements_ShouldFilterMeasurementsBySelectedCity()
    {
        // Arrange
        var selectedCityCapacity = _mockCityCapacities.Last();
        var viewModel = new MeasurementSchedulerViewModel(_mockDataService.Object) {
            // Act
            SelectedCityCapacity = selectedCityCapacity
        };

        // Assert
        // Assuming you have a way to identify the expected measurements
        var expectedMeasurements = _mockMeasurements.Where(m => m.City == selectedCityCapacity.City);
        viewModel.Measurements.Should().BeEquivalentTo(expectedMeasurements);
    }

    [Fact]
    public void SortMeasurements_NoResultsWhenCitySelected()
    {
        // Arrange
        var selectedCityCapacity = _mockCityCapacities.First();
        var viewModel = new MeasurementSchedulerViewModel(_mockDataService.Object) {
            // Act
            SelectedCityCapacity = selectedCityCapacity
        };
        // Assert
        viewModel.Measurements.Should().HaveCount(0);
    }
}