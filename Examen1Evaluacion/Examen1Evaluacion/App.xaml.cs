using Examen1Evaluacion.Services;
using Examen1Evaluacion.ViewModels;
using Examen1Evaluacion.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Threading;
using Wpf.Ui;
using Wpf.Ui.DependencyInjection;

namespace Examen1Evaluacion
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly IHost _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(c =>
            {
                var basePath = Path.GetDirectoryName(AppContext.BaseDirectory)
                               ?? throw new DirectoryNotFoundException("Unable to find base directory.");
                _ = c.SetBasePath(basePath);
            })
            .ConfigureServices((context, services) =>
            {
                _ = services.AddNavigationViewPageProvider();

                // API Service
                services.AddSingleton(new HttpClient { BaseAddress = new Uri("https://localhost:7060/") });


                // App Host
                _ = services.AddHostedService<ApplicationHostService>();

                // Navigation service
                _ = services.AddSingleton<INavigationService, NavigationService>();


                // Main Window with Navigation
                _ = services.AddSingleton<INavigationWindow, Views.MainWindow>();
                _ = services.AddSingleton<MainViewModel>();


                // ViewModels
                _ = services.AddSingleton<GalaxyViewModel>();
                _ = services.AddSingleton<GridViewModel>();



                // Views
                _ = services.AddSingleton<GalaxyView>();
                _ = services.AddSingleton<GridView>();

                //_ = services.AddSingleton<Views.SplashScreen>();

            })
            .Build();

        /// <summary>
        /// Gets the application's service provider.
        /// </summary>
        public static IServiceProvider Services
        {
            get { return _host.Services; }
        }

        /// <summary>
        /// Occurs when the application starts.
        /// </summary>
        private async void OnStartup(object sender, StartupEventArgs e)
        {

            await _host.StartAsync();
        }


        /// <summary>
        /// Occurs when the application exits.
        /// </summary>
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        /// <summary>
        /// Handles unhandled exceptions.
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
        }
    }
}