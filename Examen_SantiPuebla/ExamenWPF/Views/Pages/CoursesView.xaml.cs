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

namespace ExamenWPF.Views.Pages
{
    /// <summary>
    /// Lógica de interacción para CoursesView.xaml
    /// </summary>
    public partial class CoursesView : INavigableView<CoursesViewModel>
    {
        public CoursesViewModel ViewModel { get; }
        public CoursesView(CoursesViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = viewModel;

            InitializeComponent();
        }
    }
}
