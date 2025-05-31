using AtecaWPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
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


        // Propiedades FlyOut NuevaReserva:

        [ObservableProperty] private bool isFlyoutOpen;
        [ObservableProperty] private DateTime? nuevaReservaFecha = DateTime.Today;

        [ObservableProperty] private ObservableCollection<FranjaHorariaDTO> franjas = [];
        [ObservableProperty] private ObservableCollection<ProfesorDTO> profesores = [];
        [ObservableProperty] private ObservableCollection<GrupoClaseDTO> grupos = [];

        [ObservableProperty] private FranjaHorariaDTO? franjaSeleccionada;
        [ObservableProperty] private ProfesorDTO? profesorSeleccionado;
        [ObservableProperty] private GrupoClaseDTO? grupoSeleccionado;

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

        [RelayCommand]
        private async Task AbrirFlyout()
        {
            await CargarDatosParaFormulario();
            IsFlyoutOpen = true;
        }

        private async Task CargarDatosParaFormulario()
        {
            Franjas = new(await _httpJsonClient.GetListAsync<FranjaHorariaDTO>("api/FranjaHoraria"));
            Profesores = new(await _httpJsonClient.GetListAsync<ProfesorDTO>("api/Profesor"));
            Grupos = new(await _httpJsonClient.GetListAsync<GrupoClaseDTO>("api/GrupoClase"));
        }


        [RelayCommand]
        private async Task GuardarReserva()
        {
            if (NuevaReservaFecha == null || FranjaSeleccionada == null || ProfesorSeleccionado == null || GrupoSeleccionado == null)
            {
                MessageBox.Show("Rellene todos los campos.");
                return;
            }

            var nueva = new CreateReservaDTO
            {
                Fecha = DateOnly.FromDateTime(NuevaReservaFecha.Value),
                FranjaHorariaId = FranjaSeleccionada.Id,
                ProfesorId = ProfesorSeleccionado.Id,
                GrupoClaseId = GrupoSeleccionado.Id
            };

            try
            {
                await _httpJsonClient.PostAsync<CreateReservaDTO, ReservaDTO>("api/Reserva", nueva);
                MessageBox.Show("Reserva creada correctamente");
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
            {
                MessageBox.Show(ex.Message); // Muestra el mensaje de error 409
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                MessageBox.Show(ex.Message); // Muestra el mensaje de error 400
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}");
            }


        }

        [RelayCommand]
        private void CerrarFlyout()
        {
            IsFlyoutOpen = false;
        }

        public void OnPageLoaded()
        {
            _ = CargarReservas();
        }
    }
}
