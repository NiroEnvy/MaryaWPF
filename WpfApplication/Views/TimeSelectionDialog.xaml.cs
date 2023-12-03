using System.Windows;
using WpfApplication.Models.DataModels;

namespace WpfApplication.Views;

public partial class TimeSelectionDialog : Window
{
    public TimeSelectionDialog(IEnumerable<MeasurementPeriod> periods)
    {
        InitializeComponent();
        PeriodsListView.ItemsSource = periods.Where(p => p.Capacity > 0);
    }

    public MeasurementPeriod? SelectedPeriod { get; private set; }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        SelectedPeriod = PeriodsListView.SelectedItem as MeasurementPeriod;
        DialogResult = true;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}