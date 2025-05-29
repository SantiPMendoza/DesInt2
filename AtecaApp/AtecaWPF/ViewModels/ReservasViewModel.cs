using AtecaWPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui;

namespace AtecaWPF.ViewModels
{
    public partial class ReservasViewModel : ViewModel
    {
        private readonly HttpJsonClient _httpJsonClient;
        private readonly INavigationService _navigationService;

        // Este es el DateOnly que se usa para filtrar realmente
        [ObservableProperty]
        private DateOnly? fechaSeleccionada;

        // Este es el que el DatePicker coge
        [ObservableProperty]
        private DateTime? fechaSeleccionadaDateTime;

        [ObservableProperty]
        private string? estadoSeleccionado;

        [ObservableProperty]
        private ObservableCollection<string> estadosDisponibles = new(["Pendiente", "Aceptada", "Rechazada"]);


        [ObservableProperty]
        private ObservableCollection<ReservaDTO> _reservas = [];

        private List<ReservaDTO> todasLasReservas = [];

        public ReservasViewModel(HttpJsonClient httpJsonClient, INavigationService navigationService)
        {
            _httpJsonClient = httpJsonClient;
            _navigationService = navigationService;
        }

        // Cuando cambia la fecha seleccionada en el DatePicker
        partial void OnFechaSeleccionadaDateTimeChanged(DateTime? value)
        {
            FechaSeleccionada = value.HasValue ? DateOnly.FromDateTime(value.Value.Date) : null;
        }

        [RelayCommand]
        private async Task CargarReservas()
        {
            try
            {
                var reservasList = await _httpJsonClient.GetListAsync<ReservaDTO>("api/Reserva");

                todasLasReservas = reservasList;
                Reservas = new ObservableCollection<ReservaDTO>(todasLasReservas);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando reservas: {ex.Message}");
            }
        }

        [RelayCommand]
        private void Filtrar()
        {
            IEnumerable<ReservaDTO> filtradas = todasLasReservas;

            if (FechaSeleccionada != null)
                filtradas = filtradas.Where(r => r.Fecha == FechaSeleccionada.Value);

            if (!string.IsNullOrWhiteSpace(EstadoSeleccionado))
                filtradas = filtradas.Where(r => string.Equals(r.Estado, EstadoSeleccionado, StringComparison.OrdinalIgnoreCase));

            var lista = filtradas.ToList();

            if (lista.Count == 0)
            {
                MessageBox.Show("No se encontraron reservas con los filtros aplicados.");
                Reservas = new ObservableCollection<ReservaDTO>(todasLasReservas);
                return;
            }

            Reservas = new ObservableCollection<ReservaDTO>(lista);
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
