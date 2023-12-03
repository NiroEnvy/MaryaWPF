using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using WpfApplication.Models.Interfaces;
using WpfApplication.Models.Services;
using WpfApplication.ViewModels;
using WpfApplication.Views;

namespace WpfApplication;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Register services

        services.AddSingleton<IDataService, DataService>();
        services.AddSingleton<IDataGeneratorService, DataGeneratorService>();

        // Register ViewModels
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MeasurementSchedulerViewModel>();
        services.AddSingleton<CustomCalendarViewModel>();

        // Register Windows
        services.AddSingleton(typeof(MainWindow));
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var mainWindow = _serviceProvider.GetService<MainWindow>();
        mainWindow.Show();
    }
}