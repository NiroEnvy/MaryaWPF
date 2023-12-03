using WpfApplication.Helpers;
using WpfApplication.Models.DataModels;
using WpfApplication.Models.Interfaces;
using WpfApplication.ViewModels;

namespace TestProject1.ViewModels;

public class CustomCalendarViewModelTests
{
    private const string MockCity = "TestCity";
    private readonly Mock<IDataService> _mockDataService;

    public CustomCalendarViewModelTests()
    {
        _mockDataService = new Mock<IDataService>();
        List<Measurement> mockMeasurements = new() {
            new Measurement {
                Id = 1,
                ClientName = "John Doe",
                Address = "123 Main St, CityA",
                City = "CityA",
                Phone = "555-1234",
                MeasurementDate = new DateTime(2023, 1, 1)
            },
            new Measurement {
                Id = 2,
                ClientName = "Jane Smith",
                Address = "456 Elm St, CityB",
                City = "CityB",
                Phone = "555-5678",
                MeasurementDate = null
            }
        };

        _mockDataService.Setup(s => s.GetAllAvailableCapacity(It.IsAny<string>(), It.IsAny<DateTime>()))
            .Returns(10);

        _mockDataService.Setup(s => s.GetAllMeasurementsByCity(It.IsAny<string>()))
            .Returns(mockMeasurements);
    }

    [Fact]
    public void SetWeek_ShouldPopulateDailyInfos()
    {
        // Arrange
        var viewModel = new CustomCalendarViewModel(_mockDataService.Object);
        var testDate = new DateTime(2023, 1, 1);

        // Act
        viewModel.SetSelectedCity(MockCity);
        viewModel.SetWeek(testDate);

        // Assert
        viewModel.DailyInfos.Should().HaveCount(7);
        viewModel.DailyInfos.First().DayName.Should().Be(Utils.GetDayNamesStartingMonday().First());
        viewModel.DailyInfos.First().Date.Should().Be(testDate);
        viewModel.DailyInfos.First().Capacity.Should().Be(10);
        viewModel.SelectedDate.Should().BeNull();
    }

    [Fact]
    public void SetWeek_NotChangingIfCityNull()
    {
        // Arrange
        var viewModel = new CustomCalendarViewModel(_mockDataService.Object);
        var testDate = new DateTime(2023, 1, 1);

        // Act
        viewModel.SetWeek(testDate);

        // Assert
        viewModel.DailyInfos.Should().HaveCount(7);
        viewModel.DailyInfos.First().DayName.Should().NotBe(Utils.GetDayNamesStartingMonday().First());
        viewModel.DailyInfos.First().Date.Should().NotBe(testDate);
        viewModel.DailyInfos.First().Capacity.Should().NotBe(10);
        viewModel.SelectedDate.Should().BeNull();
    }
}