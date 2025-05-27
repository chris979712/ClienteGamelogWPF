using GameLogEscritorio.Utilidades;
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
    /// Lógica de interacción para VentanaMiReseña.xaml
    /// </summary>
    public partial class VentanaMiReseña : Window
    {
        private ReseñaJugador _reseñaJugador = new ReseñaJugador();
        public VentanaMiReseña(ReseñaJugador reseña)
        {
            InitializeComponent();
            _reseñaJugador = reseña;
            this.DataContext = _reseñaJugador;
        }


        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            new VentanaHistorialDeReseñas(Estaticas.reseñasJugador).Show();
            this.Close();
        }

        public partial class ReseñaJugador
        {
            public string? fotoVideojuego {  get; set; }
            public string? nombre { get; set; }
            public decimal calificacion {  get; set; }
            public string? fecha { get; set; }
            public string? opinion { get; set; }
        }
    }
}
