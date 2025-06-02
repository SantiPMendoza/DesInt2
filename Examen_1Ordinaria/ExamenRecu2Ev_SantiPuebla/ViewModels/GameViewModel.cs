using ExamenRecu2Ev_SantiPuebla.Models.DTOs;
using ExamenRecu2Ev_SantiPuebla.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamenRecu2Ev_SantiPuebla.Models;
using Wpf.Ui;

namespace ExamenRecu2Ev_SantiPuebla.ViewModels
{
    public partial class GameViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly HttpJsonClient _httpClient;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private List<JuegoDTO> _juegos = new();

        private DateTime gameStartTime;

        private bool _enableSpecialCircles = false;

        // Cambiado a ObservableCollection de CircleItem
        public ObservableCollection<CircleItem> CirclesImages { get; } = new();

        private Stopwatch _stopwatch;
        private int _greenCount;
        private int _hits;
        private int _misses;
        private bool _isFrozen = false;
        private double _scoreMultiplier = 1;

        public GameViewModel(HttpJsonClient httpClient, INavigationService navigationService, IAuthService authService)
        {
            _httpClient = httpClient;
            _navigationService = navigationService;
            _authService = authService;
        }

        [RelayCommand]
        public async Task GameInitAsync()
        {
            try
            {
                Juegos = await _httpClient.GetListAsync<JuegoDTO>("http://localhost:7777/api/Juego");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al cargar juegos: {ex.Message}");
            }
        }

        [RelayCommand]
        public void StartGame()
        {
            ResetGame();
            GenerateGameGrid();
            _stopwatch.Start();
            gameStartTime = DateTime.Now;
        }

        private void ResetGame()
        {
            _stopwatch = new Stopwatch();
            _hits = 0;
            _misses = 0;
            _scoreMultiplier = 1;
            _isFrozen = false;
            _greenCount = 0;
            CirclesImages.Clear();
        }

        public void GenerateGameGrid()
        {
            var rand = new Random();
            int totalCircles = 25;

            var positions = Enumerable.Range(0, totalCircles).ToList();
            for (int i = positions.Count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                (positions[i], positions[j]) = (positions[j], positions[i]);
            }

            var images = new string[totalCircles];
            string basePath = "pack://application:,,,/Resources/Circles/";

            int greenCount = rand.Next(5, 13);
            int redCount = totalCircles - greenCount;

            _greenCount = greenCount;

            int idx = 0;

            for (int i = 0; i < greenCount; i++)
                images[positions[idx++]] = basePath + "Green_Circle.png";

            for (int i = 0; i < redCount; i++)
                images[positions[idx++]] = basePath + "redCircle.jpg";

            if (_enableSpecialCircles)
            {
                int yellowCount = rand.Next(1, 3);
                int blueCount = rand.Next(1, 3);
                int replaced = 0;

                for (int i = 0; i < yellowCount && replaced < totalCircles; i++)
                {
                    int replaceIdx = rand.Next(totalCircles);
                    if (images[replaceIdx].EndsWith("Green_Circle.png") || images[replaceIdx].EndsWith("redCircle.jpg"))
                    {
                        images[replaceIdx] = basePath + "Yellow_Circle.png";
                        replaced++;
                    }
                }

                for (int i = 0; i < blueCount && replaced < totalCircles; i++)
                {
                    int replaceIdx = rand.Next(totalCircles);
                    if (images[replaceIdx].EndsWith("Green_Circle.png") || images[replaceIdx].EndsWith("redCircle.jpg"))
                    {
                        images[replaceIdx] = basePath + "blue_Circle.png";
                        replaced++;
                    }
                }
            }

            CirclesImages.Clear();
            foreach (var img in images)
            {
                CirclesImages.Add(new CircleItem(img));
            }
        }


        public async void CircleClicked(int index)
        {
            if (_isFrozen) return;

            var circle = CirclesImages[index];

            if (circle.ImagePath.EndsWith("Green_Circle.png"))
            {
                _hits++;
                _greenCount--;
                circle.ImagePath = ""; // Esto notificará el cambio y ocultará la imagen

                if (_greenCount <= 0)
                {
                    await FinishGameAsync();
                }
            }
            else if (circle.ImagePath.EndsWith("redCircle.jpg"))
            {
                _misses++;
                circle.ImagePath = "";
            }
            else if (circle.ImagePath.EndsWith("Yellow_Circle.png"))
            {
                _scoreMultiplier = 2;
                circle.ImagePath = "";

                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
                timer.Tick += (s, e) =>
                {
                    _scoreMultiplier = 1;
                    timer.Stop();
                };
                timer.Start();
            }
            else if (circle.ImagePath.EndsWith("blue_Circle.png"))
            {
                circle.ImagePath = "";

                _isFrozen = true;
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
                timer.Tick += (s, e) =>
                {
                    _isFrozen = false;
                    timer.Stop();
                };
                timer.Start();
            }
        }

        private async Task FinishGameAsync()
        {
            _stopwatch.Stop();
            double timeSeconds = _stopwatch.Elapsed.TotalSeconds;

            double baseScore = (1000 / timeSeconds) + (_hits * 10) - (_misses * 5);
            double finalScore = baseScore * _scoreMultiplier;

            var currentUser = _authService.GetCurrentUser();
            if (currentUser == null)
            {
                System.Windows.MessageBox.Show("Usuario no autenticado.");
                return;
            }

            var juegoNuevo = new CreateJuegoDTO
            {
                Name = "Partida del usuario",
                AppUserId = currentUser.Id,
                Fecha_Inicio = gameStartTime,
                Fecha_Fin = DateTime.Now,
                Resultado = finalScore
            };

            double averageScore = 400; // media arbitraria
            _enableSpecialCircles = finalScore > averageScore;

            System.Windows.MessageBox.Show($"¡Has terminado!\nPuntuación: {finalScore:F2}\nTiempo: {timeSeconds:F2} segundos");

            try
            {
                await _httpClient.PostAsync<CreateJuegoDTO, JuegoDTO>("http://localhost:7777/api/Juego", juegoNuevo);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al enviar resultado: {ex.Message}");
            }
        }

        [RelayCommand]
        public void CircleClick(CircleItem clickedCircle)
        {
            if (_isFrozen || clickedCircle == null) return;

            int index = CirclesImages.IndexOf(clickedCircle);
            if (index >= 0)
            {
                CircleClicked(index);
            }
        }

    }
}
