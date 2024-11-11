using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorArchivos_SantiagoPueblaMendoza.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        private ViewModelBase? _selectedViewModel;

        public MainViewModel(InfoViewModel infoViewModel, FileViewModel fileViewModel)
        {
            _selectedViewModel = infoViewModel;
            InfoViewModel = infoViewModel;
            FileViewModel = fileViewModel;
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
        public FileViewModel FileViewModel { get; }

        public async override Task LoadAsync()
        {
            if (SelectedViewModel is not null)
            {
                await SelectedViewModel.LoadAsync();
            }
        }
    }
}
