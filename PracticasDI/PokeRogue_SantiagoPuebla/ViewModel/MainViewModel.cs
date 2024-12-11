using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PokeRogue_SantiagoPuebla.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        private ViewModelBase? _selectedViewModel;

        public MainViewModel(InfoViewModel infoViewModel, BattleViewModel battleViewModel, HistoricViewModel historicViewModel,TeamViewModel teamViewModel)
        {
            InfoViewModel = infoViewModel;
            BattleViewModel = battleViewModel;
            HistoricViewModel= historicViewModel;
            TeamViewModel= teamViewModel;
            //SelectedViewModel = battleViewModel;
        }
        public ViewModelBase? SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                SetProperty(ref _selectedViewModel, value);
            }
        }

        public InfoViewModel InfoViewModel { get; }
        public BattleViewModel BattleViewModel { get; }
        public HistoricViewModel HistoricViewModel { get; }
        public TeamViewModel TeamViewModel { get; }

        public async override Task LoadAsync()
        {
            if (SelectedViewModel is not null)
            {
                await SelectedViewModel.LoadAsync();
            }
        }

        [RelayCommand]
        private async void SelectViewModel(object? parameter)
        {
            if (parameter is ViewModelBase viewModel)
            {
                SelectedViewModel = viewModel;
                await LoadAsync();
            }
        }


        [RelayCommand]
        private void ExitApplication(object? obj)
        {
            Application.Current.Shutdown();
        }
    }
    
}
