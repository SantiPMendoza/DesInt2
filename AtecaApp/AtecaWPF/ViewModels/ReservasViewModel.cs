using System.Net;

namespace AtecaWPF.ViewModels
{
    public partial class ReservasViewModel : ViewModel
    {
        private readonly HttpJsonClient _httpJsonClient;
        private readonly INavigationService _navigationService;

        // Variables de Reserva: propiedades para manejar filtros y listas de reservas
        [ObservableProperty]
        private DateOnly? fechaSeleccionada;

        [ObservableProperty]
        private DateTime? fechaSeleccionadaDateTime;

        [ObservableProperty]
        private string? estadoSeleccionado;

        [ObservableProperty]
        private ObservableCollection<string> estadosDisponibles = new(["Pendiente", "Aceptada", "Rechazada"]);

        [ObservableProperty]
        private ObservableCollection<ReservaDTO> _reservas = [];

        private List<ReservaDTO> todasLasReservas = [];

        // Variables para controlar el Flyout y los datos relacionados con la creación de reservas
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

        /// <summary>
        /// Actualiza la propiedad FechaSeleccionada al cambiar el valor del DatePicker.
        /// </summary>
        /// <param name="value">Nuevo valor de fecha seleccionado como DateTime.</param>
        partial void OnFechaSeleccionadaDateTimeChanged(DateTime? value)
        {
            FechaSeleccionada = value.HasValue ? DateOnly.FromDateTime(value.Value.Date) : null;
        }

        /// <summary>
        /// Carga la lista de reservas desde el servidor.
        /// </summary>
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

        /// <summary>
        /// Aplica filtros a la lista de reservas basándose en la fecha y estado seleccionados.
        /// </summary>
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

        /// <summary>
        /// Acepta una reserva y recarga la lista actualizada.
        /// </summary>
        /// <param name="reserva">Reserva a aceptar.</param>
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

        /// <summary>
        /// Rechaza una reserva y recarga la lista actualizada.
        /// </summary>
        /// <param name="reserva">Reserva a rechazar.</param>
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

        /// <summary>
        /// Abre el Flyout para crear una nueva reserva, cargando los datos necesarios.
        /// </summary>
        [RelayCommand]
        private async Task AbrirFlyout()
        {
            await CargarDatosParaFormulario();
            IsFlyoutOpen = true;
        }

        /// <summary>
        /// Carga datos necesarios para el formulario de creación de reservas.
        /// </summary>
        private async Task CargarDatosParaFormulario()
        {
            Franjas = new(await _httpJsonClient.GetListAsync<FranjaHorariaDTO>("api/FranjaHoraria"));
            Profesores = new(await _httpJsonClient.GetListAsync<ProfesorDTO>("api/Profesor"));
            Grupos = new(await _httpJsonClient.GetListAsync<GrupoClaseDTO>("api/GrupoClase"));
        }

        /// <summary>
        /// Valida y guarda una nueva reserva.
        /// </summary>
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

        /// <summary>
        /// Cierra el Flyout de creación de reserva.
        /// </summary>
        [RelayCommand]
        private void CerrarFlyout()
        {
            IsFlyoutOpen = false;
        }

        /// <summary>
        /// Método que se llama al cargar la página para iniciar la carga de reservas.
        /// </summary>
        public void OnPageLoaded()
        {
            _ = CargarReservas();
        }
    }

}
