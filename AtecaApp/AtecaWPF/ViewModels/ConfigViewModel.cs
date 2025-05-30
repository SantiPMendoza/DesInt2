using AtecaWPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui;
using static System.Net.WebRequestMethods;

namespace AtecaWPF.ViewModels
{
    public partial class ConfigViewModel : ViewModel
    {
        private readonly HttpJsonClient _httpJsonClient;
        private readonly INavigationService _navigationService;

        // Propiedades de Franjas Horarias

        [ObservableProperty]
        private ObservableCollection<FranjaHorariaDTO> franjas = [];

        [ObservableProperty]
        private FranjaHorariaDTO selectedFranja = new();

        [ObservableProperty]
        private string nuevaHoraInicioTexto = "08:00";

        [ObservableProperty]
        private string nuevaHoraFinTexto = "10:00";


        // Propiedades de Días No Lectivos 

        [ObservableProperty]
        private ObservableCollection<DiaNoLectivoDTO> diasNoLectivos = new();

        [ObservableProperty]
        private DiaNoLectivoDTO? selectedDiaNoLectivo;

        [ObservableProperty]
        private DateTime? nuevaFechaNoLectiva = DateTime.Today;

        //------------------------------------------------------------------//

        public ConfigViewModel(HttpJsonClient httpJsonClient, INavigationService navigationService)
        {
            _httpJsonClient = httpJsonClient;
            _navigationService = navigationService;

            _ = CargarFranjasAsync();
        }


        //Métodos para Franjas Horarias:

        [RelayCommand]
        public async Task CargarFranjasAsync()
        {
            try
            {
                var franjasList = await _httpJsonClient.GetListAsync<FranjaHorariaDTO>("api/FranjaHoraria");

                Franjas = new ObservableCollection<FranjaHorariaDTO>(franjasList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando reservas: {ex.Message}");
            }
        }


        [RelayCommand]
        public async Task AddFranjaAsync()
        {
            if (!TimeOnly.TryParse(NuevaHoraInicioTexto, out var inicio) ||
                !TimeOnly.TryParse(NuevaHoraFinTexto, out var fin))
            {
                MessageBox.Show("Formato de hora inválido. Use HH:mm.");
                return;
            }

            if (inicio >= fin)
            {
                MessageBox.Show("La hora de inicio debe ser anterior a la hora de fin.");
                return;
            }

            var nuevaFranja = new CreateFranjaHorariaDTO
            {
                HoraInicio = inicio,
                HoraFin = fin
            };

            try
            {
                await _httpJsonClient.PostAsync<CreateFranjaHorariaDTO, FranjaHorariaDTO>("api/FranjaHoraria", nuevaFranja);
                MessageBox.Show("Franja añadida correctamente.");
                await CargarFranjasAsync();

                // Limpia
                NuevaHoraInicioTexto = "";
                NuevaHoraFinTexto = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al añadir franja: {ex.Message}");
            }
        }





        [RelayCommand]
        public async Task UpdateFranjaAsync()
        {
            if (SelectedFranja == null)
                return;

            if (!TimeOnly.TryParse(NuevaHoraInicioTexto, out var inicio) ||
                !TimeOnly.TryParse(NuevaHoraFinTexto, out var fin))
            {
                MessageBox.Show("Formato de hora inválido. Use HH:mm.");
                return;
            }

            if (inicio >= fin)
            {
                MessageBox.Show("La hora de inicio debe ser anterior a la hora de fin.");
                return;
            }

            SelectedFranja.HoraInicio = inicio;
            SelectedFranja.HoraFin = fin;

            try
            {
                await _httpJsonClient.PutAsync($"api/FranjaHoraria/{SelectedFranja.Id}", SelectedFranja);
                MessageBox.Show("Franja horaria modificada correctamente.");
                await CargarFranjasAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar la franja: {ex.Message}");
            }
        }



        [RelayCommand]
        public async Task DeleteFranjaAsync()
        {
            if (SelectedFranja == null)
                return;

            try
            {
                await _httpJsonClient.DeleteAsync($"api/FranjaHoraria/{SelectedFranja.Id}");
                MessageBox.Show("Franja horaria eliminada correctamente.");
                await CargarFranjasAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar la franja: {ex.Message}");
            }
        }


        partial void OnSelectedFranjaChanged(FranjaHorariaDTO value)
        {
            if (value != null)
            {
                NuevaHoraInicioTexto = value.HoraInicio.ToString("HH:mm");
                NuevaHoraFinTexto = value.HoraFin.ToString("HH:mm");
            }
        }


        // Métodos para Días No Lectivos:
        [RelayCommand]
        public async Task CargarDiasNoLectivosAsync()
        {
            try
            {
                var lista = await _httpJsonClient.GetListAsync<DiaNoLectivoDTO>("api/DiaNoLectivo");
                DiasNoLectivos = new ObservableCollection<DiaNoLectivoDTO>(lista);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando días no lectivos: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task AddDiaNoLectivoAsync()
        {
            if (NuevaFechaNoLectiva == null)
            {
                MessageBox.Show("Seleccione una fecha válida.");
                return;
            }

            var nuevoDia = new CreateDiaNoLectivoDTO
            {
                Fecha = DateOnly.FromDateTime(NuevaFechaNoLectiva.Value.Date)
            };

            try
            {
                await _httpJsonClient.PostAsync<CreateDiaNoLectivoDTO, DiaNoLectivoDTO>("api/DiaNoLectivo", nuevoDia);
                MessageBox.Show("Día no lectivo añadido correctamente.");
                await CargarDiasNoLectivosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al añadir día no lectivo: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task DeleteDiaNoLectivoAsync()
        {
            if (SelectedDiaNoLectivo == null)
                return;

            try
            {
                await _httpJsonClient.DeleteAsync($"api/DiaNoLectivo/{SelectedDiaNoLectivo.Id}");
                MessageBox.Show("Día no lectivo eliminado correctamente.");
                await CargarDiasNoLectivosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar día no lectivo: {ex.Message}");
            }
        }

        // Asegúrate de cargar también los días no lectivos al cargar la página
        public void OnPageLoaded()
        {
            _ = CargarFranjasAsync();
            _ = CargarDiasNoLectivosAsync();
        }
    }


}

