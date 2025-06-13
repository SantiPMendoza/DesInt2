using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamenWPF.Models;
using ExamenWPF.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ExamenWPF.ViewModels
{
    public partial class CoursesViewModel : ObservableObject
    {
        private readonly HttpJsonClient _httpJsonClient;

        [ObservableProperty]
        private ObservableCollection<CourseDTO> cursos = new();

        private List<CourseDTO> todosLosCursos = new();

        [ObservableProperty]
        private string filtroTitulo = "";

        public CoursesViewModel(HttpJsonClient httpJsonClient)
        {
            _httpJsonClient = httpJsonClient;
        }

        [RelayCommand]
        public async Task CargarCursos()
        {
            try
            {
                var lista = await _httpJsonClient.GetListAsync<CourseDTO>("api/Course");
                todosLosCursos = lista;
                Cursos = new ObservableCollection<CourseDTO>(todosLosCursos);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error cargando cursos: {ex.Message}");
            }
        }

        [RelayCommand]
        public void Filtrar()
        {
            if (string.IsNullOrWhiteSpace(FiltroTitulo))
            {
                Cursos = new ObservableCollection<CourseDTO>(todosLosCursos);
                return;
            }

            var filtrados = todosLosCursos
                .Where(c => c.Title != null && c.Title.Contains(FiltroTitulo, System.StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (filtrados.Count == 0)
            {
                MessageBox.Show("No se encontraron cursos con ese filtro.");
                Cursos = new ObservableCollection<CourseDTO>(todosLosCursos);
            }
            else
            {
                Cursos = new ObservableCollection<CourseDTO>(filtrados);
            }
        }

        [RelayCommand]
        public void LimpiarFiltros()
        {
            FiltroTitulo = "";
            Cursos = new ObservableCollection<CourseDTO>(todosLosCursos);
        }

        // Aquí irían otros comandos para CRUD, si quieres.
    }
}
