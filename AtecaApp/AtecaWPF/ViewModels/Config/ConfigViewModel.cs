using AtecaWPF.Services;
using AtecaWPF.ViewModels.Config;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Wpf.Ui;
using static System.Net.WebRequestMethods;

namespace AtecaWPF.ViewModels.Config
{




        public partial class ConfigViewModel : ViewModel
        {
            public FranjasViewModel FranjasVM { get; }
            public DiasNoLectivosViewModel DiasNoLectivosVM { get; }

            private readonly INavigationService _navigationService;

        public ConfigViewModel(HttpJsonClient httpJsonClient, INavigationService navigationService)
            {
                FranjasVM = new FranjasViewModel(httpJsonClient);
                DiasNoLectivosVM = new DiasNoLectivosViewModel(httpJsonClient);

            _navigationService = navigationService;
        }

            [RelayCommand]
            public async Task CargarTodoAsync()
            {
                await Task.WhenAll(
                    FranjasVM.CargarFranjasAsync(),
                    DiasNoLectivosVM.CargarDiasNoLectivosAsync()
                );
            }

            public void OnPageLoaded()
            {
                _ = CargarTodoAsync();
            }
        }


    }



