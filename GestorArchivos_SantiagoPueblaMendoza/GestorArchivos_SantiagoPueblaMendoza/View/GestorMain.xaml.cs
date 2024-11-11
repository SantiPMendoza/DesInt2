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
using System.Windows.Shapes;


namespace GestorArchivos_SantiagoPueblaMendoza.View
{
    public partial class GestorMain : UserControl
    {
        public GestorMain()
        {
            InitializeComponent();
        }

        // Métodos para el menú contextual en la pestaña "File"
        private void MenuGuardar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Guardar opción seleccionada.");
            // Lógica para guardar el archivo
        }

        private void MenuBorrar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Borrar opción seleccionada.");
            // Lógica para borrar el archivo
        }

        private void MenuAñadir_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Añadir opción seleccionada.");
            // Lógica para añadir un nuevo archivo o elemento
        }

        // Métodos para el menú principal
        private void MenuAbrir_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Abrir opción seleccionada.");
            // Lógica para abrir un archivo
        }

        private void MenuSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Cierra la aplicación
        }

        private void MenuAcercaDe_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aplicación Gestor de Archivos, versión 1.0");
            // Información adicional sobre la aplicación
        }
    }
}
