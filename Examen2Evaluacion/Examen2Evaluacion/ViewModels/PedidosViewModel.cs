using Examen2Evaluacion.Models;
using Examen2Evaluacion.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui;

namespace Examen2Evaluacion.ViewModels
{
    public partial class PedidosViewModel : ViewModel
    {
        private readonly HttpJsonClient _httpJsonClient;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private ObservableCollection<PedidoDTO> _pedidos = [];

        [ObservableProperty]
        private PedidoDTO? _pedidoSeleccionado;

        public PedidosViewModel(HttpJsonClient httpJsonClient, INavigationService navigationService)
        {
            _httpJsonClient = httpJsonClient;
            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task CargarPedidos()
        {
            try
            {
                var pedidosList = await _httpJsonClient.GetListAsync<PedidoDTO>("api/Pedido");
                Pedidos = new ObservableCollection<PedidoDTO>(pedidosList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando productos: {ex.Message}");
            }

        }
    }
}
