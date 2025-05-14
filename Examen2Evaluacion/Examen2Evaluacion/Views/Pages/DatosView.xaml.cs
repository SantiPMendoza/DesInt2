using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Examen2Evaluacion.Views.Pages
{
    /// <summary>
    /// Lógica de interacción para DatosView.xaml
    /// </summary>
    public partial class DatosView : INavigableView<DatosViewModel>
    {
        public DatosViewModel ViewModel { get; }
        public DatosView(DatosViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
