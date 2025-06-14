using GameLogEscritorio.Utilidades;
using System.Windows;
using System.Windows.Media;

namespace GameLogEscritorio.Ventanas
{
    
    public partial class VentanaMiReseña : Window
    {

        private ReseñaJugador _reseñaJugador = new ReseñaJugador();

        public VentanaMiReseña(ReseñaJugador reseña)
        {
            InitializeComponent();
            _reseñaJugador = reseña;
            if(string.IsNullOrEmpty(reseña.opinion))
            {
                reseña.opinion = Constantes.SinOpinionAsignada;
                txb_Opinion.Foreground = Brushes.Gray;
            }
            this.DataContext = _reseñaJugador;
            Estaticas.GuardarMedidasUltimaVentana(this);
        }

        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            grd_OverlayCarga.Visibility = Visibility.Visible;
            VentanaHistorialDeReseñas ventanaHistorialDeReseñas = new VentanaHistorialDeReseñas(Estaticas.reseñasJugador);
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaHistorialDeReseñas);
            grd_OverlayCarga.Visibility = Visibility.Collapsed;
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
