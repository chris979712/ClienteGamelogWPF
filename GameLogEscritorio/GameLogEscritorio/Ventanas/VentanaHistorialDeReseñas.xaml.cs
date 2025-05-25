using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Reseñas;
using GameLogEscritorio.Utilidades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static GameLogEscritorio.Ventanas.VentanaMiReseña;

namespace GameLogEscritorio.Ventanas
{
    /// <summary>
    /// Lógica de interacción para VentanaHistorialDeReseñas.xaml
    /// </summary>
    public partial class VentanaHistorialDeReseñas : Window
    {
        public VentanaHistorialDeReseñas(ObservableCollection<ReseñaJugador> reseñasObtenidas)
        {
            InitializeComponent();
            ic_reseñas.ItemsSource = reseñasObtenidas;
        }

        private void VerReseña_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Image imagen && imagen.DataContext is ReseñaJugador reseña)
            {
                if (reseña != null)
                {
                    new VentanaMiReseña(reseña).Show();
                    this.Close();
                }
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            new MenuPrincipal().Show();
            this.Close();
        }
    }
}
