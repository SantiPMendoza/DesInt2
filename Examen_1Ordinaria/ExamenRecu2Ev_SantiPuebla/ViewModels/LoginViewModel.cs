using ExamenRecu2Ev_SantiPuebla.Models.DTOs.UserDTO;
using ExamenRecu2Ev_SantiPuebla.ViewModels;
using ExamenRecu2Ev_SantiPuebla.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui;

namespace ExamenRecu2Ev_SantiPuebla.ViewModels
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
                MessageBox.Show("Por favor, completa todos los campos.");
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
                    MessageBox.Show($"Bienvenido {user?.UserName}");

                    _navigationService.Navigate(typeof(GamePage));
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión: {ex.Message}");
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

