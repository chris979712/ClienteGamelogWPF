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
            if (sender is Image imagen && imagen.DataContext is ReseñaJugador reseña)
            {
                if (reseña != null)
                {
                    VentanaMiReseña ventanaReseña = new VentanaMiReseña(reseña);
                    AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaReseña);
                    this.Close();
                }
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MenuPrincipal menuPrincipal = new MenuPrincipal();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, menuPrincipal);
            this.Close();
        }
    }
}
