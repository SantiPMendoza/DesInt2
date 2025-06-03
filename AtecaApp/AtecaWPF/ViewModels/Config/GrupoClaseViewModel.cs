using System.Collections.ObjectModel;
using System.Windows;

namespace AtecaWPF.ViewModels.Config
{
    public partial class GrupoClaseViewModel : ViewModel
    {
        private readonly HttpJsonClient _httpJsonClient;

        [ObservableProperty]
        private ObservableCollection<GrupoClaseDTO> gruposClase = [];

        [ObservableProperty]
        private GrupoClaseDTO? grupoSeleccionado;

        [ObservableProperty]
        private string? nuevoNombreGrupo;

        public GrupoClaseViewModel(HttpJsonClient httpJsonClient)
        {
            _httpJsonClient = httpJsonClient;
        }

        [RelayCommand]
        public async Task CargarGruposClaseAsync()
        {
            try
            {
                var lista = await _httpJsonClient.GetListAsync<GrupoClaseDTO>("api/GrupoClase");
                GruposClase = new ObservableCollection<GrupoClaseDTO>(lista);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar grupos clase: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task AddGrupoClaseAsync()
        {
            if (string.IsNullOrWhiteSpace(NuevoNombreGrupo))
            {
                MessageBox.Show("El nombre del grupo no puede estar vacío.");
                return;
            }

            try
            {
                var nuevoGrupo = new GrupoClaseDTO { Nombre = NuevoNombreGrupo };
                await _httpJsonClient.PostAsync<GrupoClaseDTO, GrupoClaseDTO>("api/GrupoClase", nuevoGrupo);
                NuevoNombreGrupo = string.Empty;
                await CargarGruposClaseAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al añadir grupo clase: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task EliminarGrupoClaseAsync(int id)
        {
            try
            {
                await _httpJsonClient.DeleteAsync($"api/GrupoClase/{id}");
                await CargarGruposClaseAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar grupo clase: {ex.Message}");
            }
        }
    }
}
