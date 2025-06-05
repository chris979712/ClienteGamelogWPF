using GameLogEscritorio.Utilidades;
using System.Windows;

namespace GameLogEscritorio.Ventanas
{
    
    public partial class VentanaMiReseña : Window
    {

        private ReseñaJugador _reseñaJugador = new ReseñaJugador();

        public VentanaMiReseña(ReseñaJugador reseña)
        {
            InitializeComponent();
            _reseñaJugador = reseña;
            this.DataContext = _reseñaJugador;
            Estaticas.GuardarMedidasUltimaVentana(this);
        }


        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            VentanaHistorialDeReseñas ventanaHistorialDeReseñas = new VentanaHistorialDeReseñas(Estaticas.reseñasJugador);
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaHistorialDeReseñas);
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
