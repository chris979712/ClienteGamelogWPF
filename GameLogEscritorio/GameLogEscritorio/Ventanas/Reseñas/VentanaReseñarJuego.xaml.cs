using GameLogEscritorio.Servicios.APIRawg.Modelo;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Reseñas;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Utilidades;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameLogEscritorio.Ventanas
{
    
    public partial class VentanaReseñarJuego : Window
    {

        private readonly IApiRestRespuestaFactory apiRestCreadorRespuesta = new FactoryRespuestasAPI();
        private JuegoModelo _modeloJuegoAReseñar = new JuegoModelo();
        private string _ventanaPrecedente;
        private Decimal _calificacionSeleccionada = 0;

        public VentanaReseñarJuego(JuegoModelo juegoAReseñar, string ventanaPrecedente)
        {
            InitializeComponent();
            this._modeloJuegoAReseñar = juegoAReseñar;
            this._ventanaPrecedente = ventanaPrecedente;
            Estaticas.GuardarMedidasUltimaVentana(this);
        }

        private async void Reseniar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarDatos())
            {
                Decimal calificacion = ObtenerCalificacion();
                PostReseñaSolicitud datosSolicitud = new PostReseñaSolicitud()
                {
                    idJuego = _modeloJuegoAReseñar.id,
                    idJugador = UsuarioSingleton.Instancia.idJugador,
                    calificacion = calificacion,
                    opinion = txb_Opinion.Text
                };
                grd_OverlayCarga.Visibility = Visibility.Visible;
                ApiRespuestaBase respuestaApi = await ServicioReseña.RegistrarReseña(datosSolicitud,apiRestCreadorRespuesta);
                bool esCritico = ManejadorRespuestas.ManejarRespuestasBase(respuestaApi);
                if (!esCritico)
                {
                    if(respuestaApi.estado == Constantes.CodigoExito)
                    {
                        EliminarJuegoPendiente();
                        DesplegarVentanaCorrespondiente();
                    }
                    grd_OverlayCarga.Visibility = Visibility.Collapsed;
                }
                else
                {
                    await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                    grd_OverlayCarga.Visibility = Visibility.Collapsed;
                    this.Close();
                }
            }
            else
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, Properties.Resources.ContenidoDatosInvalidos, Constantes.CodigoErrorSolicitud);
                AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
            }
        }

        private void EliminarJuegoPendiente()
        {
            var juegoAEliminar = Estaticas.juegosPendientes.FirstOrDefault(juego => juego.idJuego == _modeloJuegoAReseñar.id);
            if (juegoAEliminar != null)
            {
                Estaticas.juegosPendientes.Remove(juegoAEliminar);
            }
        }

        private bool ValidarDatos()
        {
            Decimal calificacion = ObtenerCalificacion();
            bool calificacionValida = calificacion > 0 && calificacion <= 5; 
            bool opinionValida = Validador.ValidarOpinion(txb_Opinion.Text);
            if (!calificacionValida)
            {
                AnimacionesVentana.RebotarImagen(img_EstrellaUno);
                AnimacionesVentana.RebotarImagen(img_EstrellaDos);
                AnimacionesVentana.RebotarImagen(img_EstrellaTres);
                AnimacionesVentana.RebotarImagen(img_EstrellaCuatro);
                AnimacionesVentana.RebotarImagen(img_EstrellaCinco);
            }
            if (!opinionValida)
            {
                txb_Opinion.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txb_Opinion);
            }
            return calificacionValida && opinionValida;
        }

        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            DesplegarVentanaCorrespondiente();
        }

        private void DesplegarVentanaCorrespondiente()
        {
            grd_OverlayCarga.Visibility = Visibility.Visible;
            if (_ventanaPrecedente == Constantes.Menu)
            {
                MenuPrincipal menuPrincipal = new MenuPrincipal();
                AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, menuPrincipal);
            }
            else if (_ventanaPrecedente == Constantes.Descripcion)
            {
                VentanaDescripcionJuego ventanaDescripcionJuego = new VentanaDescripcionJuego(_modeloJuegoAReseñar);
                AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaDescripcionJuego);
            }
            grd_OverlayCarga.Visibility = Visibility.Collapsed;
            this.Close();
        }

        private void Estrella_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image estrella)
            {
                if (Decimal.TryParse(estrella.Tag?.ToString(), out decimal seleccion))
                {
                    _calificacionSeleccionada = seleccion;
                    for (int i = 0; i < RatingPanel.Children.Count; i++)
                    {
                        if (RatingPanel.Children[i] is Image img)
                        {
                            img.Source = new BitmapImage(new Uri(
                                i < seleccion ? Properties.Resources.EstrellaLlena.ToString() : Properties.Resources.EstrellaVacia.ToString(),
                                UriKind.Relative));
                        }
                    }
                }
            }
        }

        private Decimal ObtenerCalificacion()
        {
            return _calificacionSeleccionada;
        }

    }
}
