﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Wpf.Ui.DependencyInjection;

namespace ExamenWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
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
                services.AddSingleton(new HttpClient { BaseAddress = new Uri("https://localhost:7042/") }); //localhost cambiar puerto


                // App Host
                _ = services.AddHostedService<ApplicationHostService>();

                // Navigation service
                _ = services.AddSingleton<INavigationService, NavigationService>();

                // Services
                services.AddSingleton<HttpJsonClient>();
                services.AddSingleton<IAuthService, AuthService>();




                // Main Window with Navigation
                _ = services.AddSingleton<INavigationWindow, Views.MainWindow>();
                _ = services.AddSingleton<MainViewModel>();


                // ViewModels
                _ = services.AddSingleton<LoginViewModel>();
                _ = services.AddSingleton<CoursesViewModel>();



                // Views
                _ = services.AddSingleton<LoginView>();
                _ = services.AddSingleton<CoursesView>();

                //_ = services.AddSingleton<Views.SplashScreen>();


                // Configuration
                //_ = services.Configure<Utils.AppConfig>(context.Configuration.GetSection(nameof(Utils.AppConfig)));
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
