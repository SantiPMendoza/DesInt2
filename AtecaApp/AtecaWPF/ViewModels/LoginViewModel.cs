

using AtecaWPF.Models.UserDTO;

namespace AtecaWPF.ViewModels
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

        // Campos de entrada del usuario
        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        // Habilita o deshabilita el botón de login
        [ObservableProperty]
        private bool _isLoginEnabled = false;

        // Comando para intentar iniciar sesión
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
                    _navigationService.Navigate(typeof(ReservasView));
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

        // Detecta cambios en el usuario o contraseña para habilitar el login
        partial void OnUsernameChanged(string value) => ValidateLogin();
        partial void OnPasswordChanged(string value) => ValidateLogin();

        // Valida si se puede habilitar el botón de login
        private void ValidateLogin()
        {
            IsLoginEnabled = !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }
    }
}

