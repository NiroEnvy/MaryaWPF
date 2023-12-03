using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfApplication.ViewModels;

namespace WpfApplication.Views;

public partial class CustomCalendar
{
    private Border? _lastSelected;

    public CustomCalendar()
    {
        InitializeComponent();
    }

    // private void OnDayColumnClicked(object sender, MouseButtonEventArgs e)
    // {
    //     if (sender is Border {Tag: DateTime clickedDate} border && DataContext is CustomCalendarViewModel viewModel)
    //     {
    //         // Show Time Selection Dialog
    //         var timeDialog = new TimeSelectionDialog();
    //         if (timeDialog.ShowDialog() == true)
    //         {
    //             var selectedDateTime = clickedDate.Add(timeDialog.SelectedTime);
    //             viewModel.SelectedDate = selectedDateTime;
    //             UpdateBorderColor(border);
    //         }
    //     }
    // }

    private void OnDayColumnClicked(object sender, MouseButtonEventArgs e)
    {
        if (sender is Border {Tag: DateTime clickedDate} border && DataContext is CustomCalendarViewModel viewModel)
        {
            var capacity = viewModel.GetCityMeasurementsCapacityForDate(clickedDate);

            if (capacity != null)
            {
                var dialog = new TimeSelectionDialog(capacity.MeasurementPeriods);
                if (dialog.ShowDialog() == true && dialog.SelectedPeriod!=null)
                {
                    viewModel.SelectedDate = new DateTime(
                        clickedDate.Year,
                        clickedDate.Month,
                        clickedDate.Day,
                        dialog.SelectedPeriod.StartTime.Hour,
                        dialog.SelectedPeriod.StartTime.Minute,
                        dialog.SelectedPeriod.StartTime.Second);
                    UpdateBorderColor(border);
                }
            }
        }
    }

    private void OnPreviousWeekClicked(object sender, RoutedEventArgs routedEventArgs)
    {
        UpdateWeek(-7);
    }

    private void OnNextWeekClicked(object sender, RoutedEventArgs routedEventArgs)
    {
        UpdateWeek(7);
    }

    private void UpdateBorderColor(Border border)
    {
        if (_lastSelected != null)
        {
            _lastSelected.BorderBrush = Brushes.Transparent;
        }
        _lastSelected = border;
        _lastSelected.BorderBrush = Brushes.Yellow;
    }

    private void UpdateWeek(int dayDelta)
    {
        if (DataContext is not CustomCalendarViewModel vm) return;

        vm.SetWeek(vm.CurrentWeekStart.AddDays(dayDelta));
        if (_lastSelected != null) _lastSelected.BorderBrush = Brushes.Transparent;
        _lastSelected = null;
    }
}