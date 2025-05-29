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

        [RelayCommand]
        private async Task AceptarReserva(ReservaDTO reserva)
        {
            try
            {
                await _httpJsonClient.PutAsync($"api/Reserva/{reserva.Id}/aceptar");
                await CargarReservas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al aceptar la reserva: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task RechazarReserva(ReservaDTO reserva)
        {
            try
            {
                await _httpJsonClient.PutAsync($"api/Reserva/{reserva.Id}/rechazar");
                await CargarReservas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al rechazar la reserva: {ex.Message}");
            }
        }



        public void OnPageLoaded()
        {
            _ = CargarReservas();
        }
    }
}
