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
    /// Lógica de interacción para CalendarView.xaml
    /// </summary>
    public partial class CalendarView : INavigableView<CalendarViewModel>
    {
        public CalendarViewModel ViewModel { get; }
        public CalendarView(CalendarViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
