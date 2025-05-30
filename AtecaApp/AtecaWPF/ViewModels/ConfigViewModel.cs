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

        [ObservableProperty]
        private ObservableCollection<FranjaHorariaDTO> franjas = [];

        [ObservableProperty]
        private FranjaHorariaDTO selectedFranja = new();

        public ConfigViewModel(HttpJsonClient httpJsonClient, INavigationService navigationService)
        {
            _httpJsonClient = httpJsonClient;
            _navigationService = navigationService;

            _ = CargarFranjasAsync();
        }

        [RelayCommand]
        public async Task CargarFranjasAsync()
        {
            var data = await _httpJsonClient.GetAsync<List<FranjaHorariaDTO>>("api/FranjaHoraria");
            if (data is not null)
                Franjas = new ObservableCollection<FranjaHorariaDTO>(data);
        }

        [RelayCommand]
        public async Task AddFranjaAsync()
        {
            if (SelectedFranja is null)
                return;

            var nuevaFranja = new CreateFranjaHorariaDTO
            {
                HoraInicio = SelectedFranja.HoraInicio,
                HoraFin = SelectedFranja.HoraFin
            };


            try
            {
                var response = await _httpJsonClient.PostAsync<CreateFranjaHorariaDTO, FranjaHorariaDTO>("api/franjahoraria", nuevaFranja);

                if (response != null)
                {
                    MessageBox.Show("Franja horaria añadida correctamente");
                    await CargarFranjasAsync();
                }
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

    }
}
