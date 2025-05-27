using AtecaWPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui;

namespace AtecaWPF.ViewModels
{
    public partial class ReservasViewModel : ViewModel
    {
        private readonly HttpJsonClient _httpJsonClient;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private ObservableCollection<ReservaDTO> _reservas = [];

        public ReservasViewModel(HttpJsonClient httpJsonClient, INavigationService navigationService)
        {
            _httpJsonClient = httpJsonClient;
            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task CargarReservas()
        {
            try
            {
                var reservasList = await _httpJsonClient.GetListAsync<ReservaDTO>("api/Reserva");
                Reservas = new ObservableCollection<ReservaDTO>(reservasList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando reservas: {ex.Message}");
            }

        }

        public void OnPageLoaded()
        {
            _ = CargarReservas();
        }
    }
}
