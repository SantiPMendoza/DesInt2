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

namespace AtecaWPF.Views.Pages
{
    /// <summary>
    /// Lógica de interacción para ConfigView.xaml
    /// </summary>
    public partial class ConfigView : INavigableView<ConfigViewModel>
    {
        public ConfigViewModel ViewModel { get; }
        public ConfigView(ConfigViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

    }
}
