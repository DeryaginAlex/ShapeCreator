using Microsoft.Extensions.DependencyInjection;
using ShapeCreator.Services;
using ShapeCreator.Services.Interface;
using ShapeCreator.ViewModel;
using System.Windows;

namespace ShapeCreator;

public partial class App : Application
{
    private ServiceProvider serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        serviceProvider = ConfigureServices();

        UnhandledException();

        var mainWindow = serviceProvider.GetRequiredService<MainView>();
        mainWindow.Show();
    }

    public static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IConfigService, ConfigService>();
        services.AddSingleton<ILoggerService, LoggerService>();
        services.AddSingleton<IUiService, UiService>();
        services.AddSingleton<IValidateService, ValidateService>();
        services.AddSingleton<IFileService, FileService>();

        services.AddTransient<MainViewModel>();
        services.AddTransient<MainView>();

        return services.BuildServiceProvider();
    }

    private void UnhandledException()
    {
        var loggerService = serviceProvider.GetService<ILoggerService>();

        DispatcherUnhandledException += (sender, args) =>
        {
            args.Handled = true;
            loggerService?.Error($"DispatcherUnhandledException", args.Exception);
        };

        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            var exception = args.ExceptionObject as Exception;
            loggerService?.Error($"UnhandledException", exception ?? new Exception("Unknown"));
        };

        TaskScheduler.UnobservedTaskException += (sender, args) =>
        {
            loggerService?.Error($"UnobservedTaskException", args.Exception);
        };
    }

    protected override void OnExit(ExitEventArgs e)
    {
        serviceProvider?.Dispose();
        base.OnExit(e);
    }
}
