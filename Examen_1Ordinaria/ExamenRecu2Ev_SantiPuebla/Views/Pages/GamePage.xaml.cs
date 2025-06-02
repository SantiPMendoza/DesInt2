using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ExamenRecu2Ev_SantiPuebla.ViewModels;   // si lo necesitas para INavigableView

namespace ExamenRecu2Ev_SantiPuebla.Views.Pages
{
    public partial class GamePage : INavigableView<GameViewModel>
    {
        public GameViewModel ViewModel { get; }

        public GamePage(GameViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel; 
            InitializeComponent();
        }



    }
}
