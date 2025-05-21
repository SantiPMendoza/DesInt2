using System;
using System.Windows.Input;
using Wpf.Ui;
using Wpf.Ui.Abstractions;

using Wpf.Ui.Controls;
using Examen2Evaluacion.Views.Pages;
using System.Windows.Navigation;
using System.Net.Http;
using System.Net.Http.Json;
using Examen2Evaluacion.Models.UserDTO;

namespace Examen2Evaluacion.ViewModels
{
    public partial class LoginViewModel : ViewModel
    {

        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;


        public LoginViewModel(IAuthService authService, INavigationService navigationService)
        {
            _authService = authService;
            _navigationService = navigationService;
        }

        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private bool _isLoginEnabled = false;

        [RelayCommand]
        private async Task CheckLogin()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                System.Windows.MessageBox.Show("Por favor, completa todos los campos.");
                return;
            }

            var loginData = new UserLoginDTO
            {
                UserName = Username,
                Password = Password
            };

            try
            {
                var success = await _authService.LoginAsync(loginData);

                if (success)
                {
                    var user = _authService.GetCurrentUser();
                    System.Windows.MessageBox.Show($"Bienvenido {user?.Name}");
                    _navigationService.Navigate(typeof(ProductosView));
                }
                else
                {
                    System.Windows.MessageBox.Show("Usuario o contraseña incorrectos.");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error de conexión: {ex.Message}");
            }
        }




        partial void OnUsernameChanged(string value) => ValidateLogin();
        partial void OnPasswordChanged(string value) => ValidateLogin();

        private void ValidateLogin()
        {
            IsLoginEnabled = !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

    }


}

