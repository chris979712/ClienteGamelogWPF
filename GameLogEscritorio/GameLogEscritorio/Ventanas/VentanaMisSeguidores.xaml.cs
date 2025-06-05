using GameLogEscritorio.Utilidades;
using System.Windows;

namespace GameLogEscritorio.Ventanas
{
    
    public partial class VentanaMisSeguidores : Window
    {

        public VentanaMisSeguidores()
        {
            InitializeComponent();
            Estaticas.GuardarMedidasUltimaVentana(this);
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
