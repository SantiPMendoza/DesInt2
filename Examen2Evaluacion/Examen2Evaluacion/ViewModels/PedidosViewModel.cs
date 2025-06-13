using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Examen2Evaluacion.Models;
using Examen2Evaluacion.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui;

namespace Examen2Evaluacion.ViewModels
{
    public partial class PedidosViewModel : ObservableObject
    {
        private readonly HttpJsonClient _httpJsonClient;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private ObservableCollection<PedidoDTO> _pedidos = [];

        [ObservableProperty]
        private PedidoDTO? _pedidoSeleccionado;

        [ObservableProperty]
        private ProductoDTO? _productoSeleccionado;

        [ObservableProperty]
        private ObservableCollection<ProductoDTO> _productos = [];

        public PedidosViewModel(HttpJsonClient httpJsonClient, INavigationService navigationService)
        {
            _httpJsonClient = httpJsonClient;
            _navigationService = navigationService;
            _ = CargarPedidos();
            _ = CargarProductos();
        }

        [RelayCommand]
        private async Task CargarPedidos()
        {
            try
            {
                var pedidosList = await _httpJsonClient.GetListAsync<PedidoDTO>("api/Pedido");
                Pedidos = new ObservableCollection<PedidoDTO>(pedidosList);

                MessageBox.Show($"Se cargaron {Pedidos.Count} pedidos");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando pedidos: {ex.Message}");
            }
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

        [RelayCommand(CanExecute = nameof(CanAgregarProducto))]
        private async Task AgregarProducto()
        {
            if (PedidoSeleccionado == null || ProductoSeleccionado == null)
                return;

            if (!PedidoSeleccionado.Productos.Any(p => p.Id == ProductoSeleccionado.Id))
            {
                PedidoSeleccionado.Productos.Add(ProductoSeleccionado);

                var updatedPedido = new
                {
                    UsuarioId = PedidoSeleccionado.UsuarioId,
                    Fecha = PedidoSeleccionado.Fecha,
                    ProductosId = PedidoSeleccionado.Productos.Select(p => p.Id).ToList()
                };

                await _httpJsonClient.PutAsync($"api/Pedido/{PedidoSeleccionado.Id}", updatedPedido);
            }
        }

        private bool CanAgregarProducto() =>
            PedidoSeleccionado != null && ProductoSeleccionado != null;

        [RelayCommand]
        private async Task EliminarProducto(ProductoDTO producto)
        {
            if (PedidoSeleccionado == null || producto == null)
                return;

            PedidoSeleccionado.Productos.Remove(producto);

            var updatedPedido = new
            {
                UsuarioId = PedidoSeleccionado.UsuarioId,
                Fecha = PedidoSeleccionado.Fecha,
                ProductosId = PedidoSeleccionado.Productos.Select(p => p.Id).ToList()
            };

            await _httpJsonClient.PutAsync($"api/Pedido/{PedidoSeleccionado.Id}", updatedPedido);
        }
    }
}
