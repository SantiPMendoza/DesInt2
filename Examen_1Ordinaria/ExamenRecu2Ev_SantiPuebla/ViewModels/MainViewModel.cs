using ExamenRecu2Ev_SantiPuebla.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Wpf.Ui.Controls;
using Wpf.Ui;

namespace ExamenRecu2Ev_SantiPuebla.ViewModels
{
    public partial class MainViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _applicationTitle = "Jueguitos";

        [ObservableProperty]
        private ObservableCollection<object> _navigationItems = [];

        [ObservableProperty]
        private ObservableCollection<object> _navigationFooter = [];

        [ObservableProperty]
        private Visibility navigationVisibility = Visibility.Hidden; // Oculto al inicio

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            if (!_isInitialized)
            {
                InitializeViewModel();
                _ = ShowNavigationAfterDelay(); // Ejecutar delay al iniciar
            }
        }

        private void InitializeViewModel()
        {
            ApplicationTitle = "Jueguitos";
            NavigationItems =
            [
                new NavigationViewItem()
                {
                    Content = "Dashboard",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Add16 },
                },
                new NavigationViewItem()
                {
                    Content = "Juego",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Album20 },
                },
            ];

            NavigationFooter =
            [
                new NavigationViewItem()
            {
                Content = "Perfil",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.ProfilePage)
            },
        ];
            _isInitialized = true;
        }

        public async Task ShowNavigationAfterDelay()
        {
            await Task.Delay(500);
            NavigationVisibility = Visibility.Visible;
        }
    }
}