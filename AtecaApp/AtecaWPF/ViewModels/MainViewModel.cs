using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Wpf.Ui.Controls;
using Wpf.Ui;

namespace AtecaWPF.ViewModels
{
    public partial class MainViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _applicationTitle = "AtecApp";

        [ObservableProperty]
        private ObservableCollection<object> _navigationItems = [];

        [ObservableProperty]
        private ObservableCollection<object> _navigationFooter = [];

        [ObservableProperty]
        private Visibility navigationVisibility = Visibility.Hidden;

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            if (!_isInitialized)
            {
                InitializeViewModel();

                _ = ShowNavigationAfterDelay();
            }
        }

        private void InitializeViewModel()
        {
            NavigationItems =
            [
                new NavigationViewItem()
                {
                    Content = "Reservas",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Add16 },
                    TargetPageType = typeof(ReservasView)
                },
                new NavigationViewItem()
                {
                    Content = "Calendario?",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Album20 },
                    TargetPageType = typeof(CalendarView)
                },

            ];

            NavigationFooter =
[
                new NavigationViewItem()
                {
                    Content = "Configuración",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Clock12 },
                    TargetPageType = typeof(ConfigView)
                },
        ];

            _isInitialized = true;
            
        }

        public async Task ShowNavigationAfterDelay()
        {
            await Task.Delay(750);
            NavigationVisibility = Visibility.Visible;
        }

    }
}
