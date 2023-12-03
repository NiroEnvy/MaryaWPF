using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using WpfApplication.Helpers;
using WpfApplication.Models.DataModels;
using WpfApplication.Models.Interfaces;

namespace WpfApplication.ViewModels;

public class CustomCalendarViewModel : ReactiveObject
{
    private readonly IDataService _dataService;
    private readonly List<string> _dayNames;
    private string? _selectedCity;

    public CustomCalendarViewModel(IDataService dataService)
    {
        _dataService = dataService;
        _dayNames = Utils.GetDayNamesStartingMonday().ToList();
        DailyInfos = new ObservableCollection<DayInfoToShow>(Enumerable.Repeat(new DayInfoToShow(), 7));
    }

    [Reactive] public ObservableCollection<DayInfoToShow> DailyInfos { get; set; }
    [Reactive] public DateTime? SelectedDate { get; set; }
    [Reactive] public DateTime CurrentWeekStart { get; set; }

    public void SetSelectedCity(string city)
    {
        _selectedCity = city;
    }

    public void SetWeek(DateTime date)
    {
        if (_selectedCity == null) return;

        CurrentWeekStart = date.AddDays(-(int) date.DayOfWeek);
        for (var i = 0; i < _dayNames.Count; i++)
        {
            var nextDate = CurrentWeekStart.AddDays(i);
            var capacity = _dataService.GetAllAvailableCapacity(_selectedCity, nextDate);
            var people = _dataService
                .GetAllMeasurementsByCity(_selectedCity)
                .Where(x => x.MeasurementDate != null && Utils.CheckTwoDatesOnEqualityByDays(x.MeasurementDate.Value, nextDate));

            DailyInfos[i] = new DayInfoToShow {
                DayName = _dayNames[i],
                Date = nextDate,
                Capacity = capacity,
                People = new List<Measurement>(people)
            };
        }
        SelectedDate = null;
    }

    public CityMeasurementsCapacity? GetCityMeasurementsCapacityForDate(DateTime clickedDate)
    {
        return (_dataService.GetAllCityMeasurementsCapacity() ?? Array.Empty<CityMeasurementsCapacity>())
            .FirstOrDefault(c => c.City == _selectedCity && Utils.CheckTwoDatesOnEqualityByDays(c.MainDate, clickedDate));
    }
}