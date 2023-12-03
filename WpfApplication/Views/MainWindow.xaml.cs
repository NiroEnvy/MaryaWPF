using WpfApplication.ViewModels;

namespace WpfApplication.Views;

public partial class MainWindow
{
    public MainWindow(MainWindowViewModel mainWindowViewModel)
    {
        DataContext = mainWindowViewModel;
        InitializeComponent();
    }
}