using Microsoft.Extensions.DependencyInjection;
using PokeRogue_SantiagoPuebla.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PokeRogue_SantiagoPuebla
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Services = ConfigureServices();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = Current.Services.GetService<MainWindow>();
            mainWindow?.Show();
        }

        public new static App Current => (App)Application.Current;
        public IServiceProvider Services { get; }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Servicios para las vistas
            services.AddTransient<MainWindow>();

            // Servicios para los ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<BattleViewModel>();
            services.AddTransient<HistoricViewModel>();
            services.AddTransient<TeamViewModel>();
            services.AddTransient<InfoViewModel>();

            return services.BuildServiceProvider();
        }
    }

}
