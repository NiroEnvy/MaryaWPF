using System.Reactive;
using System.Windows;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using WpfApplication.Models.Interfaces;

namespace WpfApplication.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private readonly IDataService _dataService;

    public MainWindowViewModel(IDataService dataService,
        MeasurementSchedulerViewModel measurementSchedulerVm,
        CustomCalendarViewModel calendarViewModel)
    {
        _dataService = dataService;
        MeasurementSchedulerVm = measurementSchedulerVm;
        CalendarViewModel = calendarViewModel;
        var canSubmit = this.WhenAnyValue(
            x => x.MeasurementSchedulerVm.SelectedPerson,
            x => x.CalendarViewModel.SelectedDate,
            x => x.MeasurementSchedulerVm.SelectedCityCapacity!.City,
            (person, date, city) => person != null && date.HasValue && !string.IsNullOrWhiteSpace(city));

        SubmitCommand = ReactiveCommand.Create(SubmitMeasurement, canSubmit);
        MeasurementSchedulerVm
            .WhenAnyValue(x => x.SelectedCityCapacity)
            .Subscribe(m => OnCitySelected(m?.City));
    }

    //For designer
    public MainWindowViewModel() { }

    public ReactiveCommand<Unit, Unit> SubmitCommand { get; }
    [Reactive] public Visibility CustomCalendarVisibility { get; set; } = Visibility.Hidden;
    [Reactive] public MeasurementSchedulerViewModel MeasurementSchedulerVm { get; set; }
    [Reactive] public CustomCalendarViewModel CalendarViewModel { get; set; }

    private void OnCitySelected(string? city)
    {
        if (!string.IsNullOrEmpty(city))
        {
            CustomCalendarVisibility = Visibility.Visible;
            CalendarViewModel.SetSelectedCity(city);
            CalendarViewModel.SetWeek(DateTime.Today);
        }
        else
        {
            CustomCalendarVisibility = Visibility.Hidden;
        }
    }

    private void SubmitMeasurement()
    {
        if (MeasurementSchedulerVm.SelectedPerson != null
            && CalendarViewModel.SelectedDate.HasValue
            && MeasurementSchedulerVm.SelectedCityCapacity != null)
        {
            var selectedDate = CalendarViewModel.SelectedDate.Value;
            // Update the capacity
            var city = MeasurementSchedulerVm.SelectedCityCapacity.City;
            var capacity = CalendarViewModel.GetCityMeasurementsCapacityForDate(selectedDate);
            if (capacity != null)
            {
                _dataService.UpdateCapacity(city, selectedDate,
                    capacity.MeasurementPeriods
                        .FirstOrDefault(x => Convert.ToDateTime(x.StartTime).Day == CalendarViewModel.SelectedDate.Value.Day).Capacity
                    - 1);
            }
            //update person date for measure
            MeasurementSchedulerVm.SelectedPerson.MeasurementDate = selectedDate;
            // Refresh the calendar to reflect the new capacity
            CalendarViewModel.SetWeek(CalendarViewModel.CurrentWeekStart);
            //reset value
            MeasurementSchedulerVm.SelectedCityCapacity = null;
        }
    }
}