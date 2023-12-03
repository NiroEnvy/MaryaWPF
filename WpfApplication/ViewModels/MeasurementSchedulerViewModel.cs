using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using WpfApplication.Models.DataModels;
using WpfApplication.Models.Interfaces;

namespace WpfApplication.ViewModels;

public class MeasurementSchedulerViewModel : ReactiveObject
{
    public delegate void MyEventAction(CityMeasurementsCapacity? cityMeasurementsCapacity);

    private readonly IDataService _dataService;

    public MeasurementSchedulerViewModel(IDataService dataService)
    {
        _dataService = dataService;
        Measurements = new ObservableCollection<Measurement>(_dataService.GetAllMeasurements());
        CityMeasurementsCapacity = new ObservableCollection<CityMeasurementsCapacity>(_dataService.GetAllCityMeasurementsCapacity());

        MyEvent += SortMeasurements;
        this.WhenAnyValue(x => x.SelectedCityCapacity)
            .Subscribe(x => MyEvent?.Invoke(x));
    }

    [Reactive] public ObservableCollection<Measurement> Measurements { get; set; }

    [Reactive] public Measurement? SelectedPerson { get; set; }
    [Reactive] public CityMeasurementsCapacity? SelectedCityCapacity { get; set; }

    [Reactive] public ObservableCollection<CityMeasurementsCapacity> CityMeasurementsCapacity { get; set; }
    public event MyEventAction? MyEvent;

    private void SortMeasurements(CityMeasurementsCapacity? m)
    {
        SelectedPerson = null;
        if (m == null)
        {
            Measurements = new ObservableCollection<Measurement>(_dataService.GetAllMeasurements()
                .Where(x => x.MeasurementDate == null));
        }
        else
        {
            Measurements = new ObservableCollection<Measurement>(_dataService.GetAllMeasurements()
                .Where(x => x.City == m.City && x.MeasurementDate == null));
        }
    }
}