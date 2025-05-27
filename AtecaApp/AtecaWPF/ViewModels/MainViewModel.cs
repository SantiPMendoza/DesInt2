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
        private string _applicationTitle = "Ateca";

        [ObservableProperty]
        private ObservableCollection<object> _navigationItems = [];

        [ObservableProperty]
        private ObservableCollection<object> _navigationFooter = [];

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            if (!_isInitialized)
            {
                InitializeViewModel();
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
                    Content = "Placeholder :v",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Album20 },
                    //TargetPageType = typeof(PedidosView)
                },

            ];

            NavigationFooter =
[
                new NavigationViewItem()
                {
                    Content = "Perfil?",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.CalendarDay24 },
                    //TargetPageType = typeof(DatosView)
                },
        ];

            _isInitialized = true;
            
        }
            
    }
}
