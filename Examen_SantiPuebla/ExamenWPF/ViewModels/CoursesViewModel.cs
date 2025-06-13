using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamenWPF.Models.DTOs;
using ExamenWPF.Services;
using System;
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

        // --- Flyout properties ---
        [ObservableProperty] private bool isFlyoutOpen;
        [ObservableProperty] private string nuevoTitulo = "";

        [ObservableProperty] private ObservableCollection<StudentDTO> estudiantesDisponibles = new();
        [ObservableProperty] private ObservableCollection<TeacherDTO> profesoresDisponibles = new();

        [ObservableProperty] private ObservableCollection<StudentDTO> estudiantesSeleccionados = new();
        [ObservableProperty] private ObservableCollection<TeacherDTO> profesoresSeleccionados = new();

        [ObservableProperty] private StudentDTO? estudianteSeleccionado;
        [ObservableProperty] private TeacherDTO? profesorSeleccionado;

        // --- Selected course for deletion ---
        [ObservableProperty]
        private CourseDTO? cursoSeleccionado;

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
            catch (Exception ex)
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
                .Where(c => c.Title != null && c.Title.Contains(FiltroTitulo, StringComparison.OrdinalIgnoreCase))
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

        // --- Abrir y cerrar Flyout ---
        [RelayCommand]
        public async Task AbrirFlyout()
        {
            await CargarEstudiantesYProfesores();
            NuevoTitulo = "";
            EstudiantesSeleccionados = new();
            ProfesoresSeleccionados = new();
            EstudianteSeleccionado = null;
            ProfesorSeleccionado = null;
            IsFlyoutOpen = true;
        }

        [RelayCommand]
        public void CerrarFlyout()
        {
            IsFlyoutOpen = false;
        }

        private async Task CargarEstudiantesYProfesores()
        {
            EstudiantesDisponibles = new(await _httpJsonClient.GetListAsync<StudentDTO>("api/Student"));
            ProfesoresDisponibles = new(await _httpJsonClient.GetListAsync<TeacherDTO>("api/Teacher"));
        }

        [RelayCommand]
        public async Task GuardarCurso()
        {
            if (string.IsNullOrWhiteSpace(NuevoTitulo))
            {
                MessageBox.Show("Debe ingresar un título para el curso.");
                return;
            }

            var nuevoCurso = new CreateCourseDTO
            {
                Title = NuevoTitulo,
                StudentIds = EstudiantesSeleccionados.Select(e => e.Id).ToList(),
                TeacherIds = ProfesoresSeleccionados.Select(p => p.Id).ToList()
            };

            try
            {
                await _httpJsonClient.PostAsync<CreateCourseDTO, CourseDTO>("api/Course", nuevoCurso);
                MessageBox.Show("Curso creado correctamente.");
                IsFlyoutOpen = false;
                await CargarCursos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el curso: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task EliminarCurso()
        {
            if (CursoSeleccionado == null)
            {
                MessageBox.Show("Seleccione un curso para eliminar.");
                return;
            }

            if (MessageBox.Show($"¿Está seguro de eliminar el curso '{CursoSeleccionado.Title}'?", "Confirmación", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    await _httpJsonClient.DeleteAsync($"api/Course/{CursoSeleccionado.Id}");
                    MessageBox.Show("Curso eliminado correctamente.");
                    await CargarCursos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar el curso: {ex.Message}");
                }
            }
        }

        [RelayCommand]
        public void AñadirEstudiante()
        {
            if (EstudianteSeleccionado != null && !EstudiantesSeleccionados.Contains(EstudianteSeleccionado))
            {
                EstudiantesSeleccionados.Add(EstudianteSeleccionado);
            }
        }

        [RelayCommand]
        public void AñadirProfesor()
        {
            if (ProfesorSeleccionado != null && !ProfesoresSeleccionados.Contains(ProfesorSeleccionado))
            {
                ProfesoresSeleccionados.Add(ProfesorSeleccionado);
            }
        }
    }
}
