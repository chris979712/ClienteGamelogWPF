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

namespace GameLogEscritorio.Ventanas
{
    /// <summary>
    /// Lógica de interacción para VentanaMisSeguidores.xaml
    /// </summary>
    public partial class VentanaMisSeguidores : Window
    {
        public VentanaMisSeguidores()
        {
            InitializeComponent();
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_eliminarSeguidor(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_MostrarTodos_Click(object sender, RoutedEventArgs e)
        {
            itemsControlTodos.Visibility = Visibility.Visible;
            itemsControlSeguidos.Visibility = Visibility.Collapsed;
        }

        private void Btn_MostrarSeguidos_Click(object sender, RoutedEventArgs e)
        {
            itemsControlTodos.Visibility = Visibility.Collapsed;
            itemsControlSeguidos.Visibility = Visibility.Visible;
        }

    }
}
