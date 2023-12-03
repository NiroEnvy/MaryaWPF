using WpfApplication.Models.DataModels;
using WpfApplication.Models.Interfaces;
using WpfApplication.ViewModels;

namespace TestProject1.ViewModels;

public class MainWindowViewModelTests
{
    private readonly Mock<CustomCalendarViewModel> _mockCustomCalendarVm;
    private readonly Mock<IDataService> _mockDataService;
    private readonly MeasurementSchedulerViewModel _mockMeasurementSchedulerVm;

    public MainWindowViewModelTests()
    {
        _mockDataService = new Mock<IDataService>();
        _mockMeasurementSchedulerVm = new MeasurementSchedulerViewModel(_mockDataService.Object);
        _mockCustomCalendarVm = new Mock<CustomCalendarViewModel>(_mockDataService.Object);

        _mockDataService.Setup(s => s.GetAllAvailableCapacity(It.IsAny<string>(), It.IsAny<DateTime>()))
            .Returns((string arg1, DateTime _) =>
            {
                if (_mockMeasurementSchedulerVm.SelectedCityCapacity != null
                    && _mockMeasurementSchedulerVm.SelectedCityCapacity.City == arg1)
                {
                    return _mockMeasurementSchedulerVm.SelectedCityCapacity.MeasurementPeriods.Sum(x => x.Capacity);
                }
                return 0;
            });
        // Setup mock data service as needed
        _mockDataService.Setup(s => s.UpdateCapacity(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<int>()))
            .Verifiable();

    }


    [Fact]
    public void SubmitCommand_ShouldUpdateCapacityAndPersonNullAfterSubmit()
    {
        // Arrange
        var viewModel = new MainWindowViewModel(_mockDataService.Object, _mockMeasurementSchedulerVm, _mockCustomCalendarVm.Object);
        var now = DateTime.Today;
        var testCity = "TestCity";
        var testPeriods = new List<MeasurementPeriod> {
            new() {
                StartTime = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0),
                EndTime = new DateTime(now.Year, now.Month, now.Day, 10, 0, 0),
                Capacity = 1
            }
        };

        _mockMeasurementSchedulerVm.SelectedCityCapacity = new CityMeasurementsCapacity {
            City = testCity,
            MainDate = now,
            MeasurementPeriods = testPeriods
        };

        _mockMeasurementSchedulerVm.SelectedPerson = new Measurement {
            Id = 1,
            ClientName = "John Doe",
            Address = "123 Main St, CityA",
            City = testCity,
            Phone = "555-1234",
            MeasurementDate = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0)
        };
        _mockCustomCalendarVm.Object.SelectedDate = now;

        // // Mocking DataService responses
        _mockDataService.Setup(s => s.GetAllAvailableCapacity(It.IsAny<string>(), It.IsAny<DateTime>()))
            .Returns(1);

        // Act
        viewModel.SubmitCommand.Execute().Subscribe();

        // Assert
        viewModel.MeasurementSchedulerVm.SelectedPerson.Should().BeNull();
        viewModel.MeasurementSchedulerVm.SelectedCityCapacity.Should().BeNull();
    }
}