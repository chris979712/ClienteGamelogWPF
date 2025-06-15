using GameLogEscritorio.Utilidades;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using static GameLogEscritorio.Ventanas.VentanaMiReseña;

namespace GameLogEscritorio.Ventanas
{
    
    public partial class VentanaHistorialDeReseñas : Window
    {
        public VentanaHistorialDeReseñas(ObservableCollection<ReseñaJugador> reseñasObtenidas)
        {
            InitializeComponent();
            ic_reseñas.ItemsSource = reseñasObtenidas;
            Estaticas.GuardarMedidasUltimaVentana(this);
        }

        private void VerReseña_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Image imagen && imagen.DataContext is ReseñaJugador reseña && reseña != null)
            {
                grd_OverlayCarga.Visibility = Visibility.Visible;
                VentanaMiReseña ventanaReseña = new VentanaMiReseña(reseña);
                AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaReseña);
                grd_OverlayCarga.Visibility = Visibility.Collapsed;
                this.Close();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            grd_OverlayCarga.Visibility = Visibility.Visible;
            MenuPrincipal menuPrincipal = new MenuPrincipal();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, menuPrincipal);
            grd_OverlayCarga.Visibility = Visibility.Collapsed;
            this.Close();
        }
    }
}
