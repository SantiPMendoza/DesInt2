using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Examen2Evaluacion.Models;
using Examen2Evaluacion.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Wpf.Ui;

namespace Examen2Evaluacion.ViewModels
{
    public partial class ProductosViewModel : ViewModel
    {
        private readonly HttpJsonClient _httpJsonClient;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private ObservableCollection<ProductoDTO> _productos = [];

        public ProductosViewModel(HttpJsonClient httpJsonClient, INavigationService navigationService)
        {
            _httpJsonClient = httpJsonClient;
            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task CargarProductos()
        {
            try
            {
                var productosList = await _httpJsonClient.GetListAsync<ProductoDTO>("api/Producto");
                Productos = new ObservableCollection<ProductoDTO>(productosList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando productos: {ex.Message}");
            }

        }

        public void OnPageLoaded()
        {
            CargarProductos();
        }
    }
}
