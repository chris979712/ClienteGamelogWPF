using GameLogEscritorio.Servicios.APIRawg.Modelo;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Juegos;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Servicios.ServicioNotificacion;
using GameLogEscritorio.Utilidades;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GameLogEscritorio.Ventanas
{
    
    public partial class VentanaDescripcionJuego : Window
    {

        private static readonly IApiRestRespuestaFactory apiRestCreadorRespuesta = new FactoryRespuestasAPI();
        private JuegoModelo _juegoObtenido = new JuegoModelo();

        public VentanaDescripcionJuego(JuegoModelo? juegoObtenido)
        {
            InitializeComponent();
            _juegoObtenido = juegoObtenido!;
            CargarDatosVentana();
            Estaticas.GuardarMedidasUltimaVentana(this);
            CargarBotonesCorrespondientes();
        }

        private void CargarBotonesCorrespondientes()
        {
            if(UsuarioSingleton.Instancia.tipoDeAcceso != Constantes.tipoJugadorPorDefecto)
            {
                btn_AñadirAPendientes.Visibility = Visibility.Collapsed;
                btn_QuitarPendientes.Visibility = Visibility.Collapsed;
                btn_Reseñar.Visibility = Visibility.Collapsed;
            }
        }


        public void CargarDatosVentana()
        {
            this.DataContext = _juegoObtenido;
            List<PlataformaEnmascarada> plataformasEnmascaradas = _juegoObtenido.platforms!.ToList();
            var creadorString = new StringBuilder();
            foreach (var plataforma in plataformasEnmascaradas)
            {
                creadorString.Append(plataforma.platform!.name).Append(" ");
            }
            lbl_plataformas.Content = creadorString.ToString().Trim();
            if(Estaticas.juegosPendientes.Any(juego => juego.idJuego == _juegoObtenido.id))
            {
                btn_AñadirAPendientes.Visibility = Visibility.Collapsed;
                btn_QuitarPendientes.Visibility = Visibility.Visible;
            }
            else
            {
                btn_QuitarPendientes.Visibility =Visibility.Collapsed;
                btn_AñadirAPendientes.Visibility=Visibility.Visible;
            }
            if(Estaticas.juegosFavoritos != null && Estaticas.juegosFavoritos.Any(juego => juego.idJuego == _juegoObtenido.id))
            {
                btn_AgregarAFavoritos.Visibility = Visibility.Collapsed;    
                btn_QuitarDeFavoritos.Visibility=Visibility.Visible;
            }
            else
            {
                btn_AgregarAFavoritos.Visibility = Visibility.Visible;
                btn_QuitarDeFavoritos.Visibility = Visibility.Collapsed;
            }
        }

        private async void ListaPendientes_Click(object sender, RoutedEventArgs e)
        {
            PostJuegoPendienteSolicitud datosSolicitud = new PostJuegoPendienteSolicitud()
            {
                idJugador = UsuarioSingleton.Instancia.idJugador,
                idJuego = _juegoObtenido.id
            };
            ApiRespuestaBase respuestaBase = await ServicioJuego.RegistrarJuegoPendiente(datosSolicitud,apiRestCreadorRespuesta);
            bool sinAcceso = ManejadorRespuestas.ManejarRespuestasBase(respuestaBase);
            if (!sinAcceso)
            {
                if (respuestaBase.estado == Constantes.CodigoExito)
                {
                    btn_AñadirAPendientes.Visibility = Visibility.Collapsed;
                    btn_QuitarPendientes.Visibility = Visibility.Visible;
                    Estaticas.juegosPendientes.Add(new JuegoCompleto()
                    {
                        idJuego = _juegoObtenido.id,
                        nombre = _juegoObtenido.name,
                        descripcion = _juegoObtenido.description,
                        fechaLanzamiento = _juegoObtenido.released,
                        imagenFondo = _juegoObtenido.backgroundImage,
                        platforms = _juegoObtenido.platforms,
                        rating = _juegoObtenido.rating
                    });
                }
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                this.Close();
            }
            
        }

        private async void QuitarListaPendientes_Click(object sender, EventArgs e)
        {
            ApiRespuestaBase respuestaBase = await ServicioJuego.EliminarJuegoPendiente(_juegoObtenido.id, UsuarioSingleton.Instancia.idJugador,apiRestCreadorRespuesta);
            bool sinAcceso = ManejadorRespuestas.ManejarRespuestasBase(respuestaBase);
            if (!sinAcceso)
            {
                if(respuestaBase.estado == Constantes.CodigoExito)
                {
                    btn_QuitarPendientes.Visibility = Visibility.Collapsed;
                    btn_AñadirAPendientes.Visibility = Visibility.Visible;
                    var juegoAEliminar = Estaticas.juegosPendientes.FirstOrDefault(juego => juego.idJuego == _juegoObtenido.id);
                    if (juegoAEliminar != null)
                    {
                        Estaticas.juegosPendientes.Remove(juegoAEliminar);
                    }
                }
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                this.Close();
            }
        }

        private async void ListaAgregarFavoritos_Click(object sender, RoutedEventArgs e)
        {
            PostJuegoFavorito solicitud = new PostJuegoFavorito()
            {
                idJuego = _juegoObtenido.id,
                idJugador = UsuarioSingleton.Instancia.idJugador
            };
            ApiRespuestaBase respuestaBase = await ServicioJuego.RegistrarJuegoFavorito(solicitud, apiRestCreadorRespuesta);
            bool sinAcceso = ManejadorRespuestas.ManejarRespuestasBase(respuestaBase);
            if (!sinAcceso)
            {
                if (respuestaBase.estado == Constantes.CodigoExito)
                {
                    btn_AgregarAFavoritos.Visibility = Visibility.Collapsed;
                    btn_QuitarDeFavoritos.Visibility = Visibility.Visible;
                    Estaticas.juegosFavoritos.Add(new Juego()
                    {
                        idJuego = _juegoObtenido.id,
                        nombre = _juegoObtenido.name,
                        fechaDeLanzamiento = _juegoObtenido.released
                    });
                }
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                this.Close();
            }  
        }

        private async void ListaQuitarFavoritos_Click(object sender, RoutedEventArgs e)
        {
            ApiRespuestaBase respuestaBase = await ServicioJuego.EliminarJuegoFavorito(_juegoObtenido.id, UsuarioSingleton.Instancia.idJugador,apiRestCreadorRespuesta);
            bool sinAcceso = ManejadorRespuestas.ManejarRespuestasBase(respuestaBase);
            if(!sinAcceso)
            {
                if(respuestaBase.estado == Constantes.CodigoExito)
                {
                    btn_AgregarAFavoritos.Visibility = Visibility.Visible;
                    btn_QuitarDeFavoritos.Visibility = Visibility.Collapsed;
                    var juegoAEliminar = Estaticas.juegosFavoritos.FirstOrDefault(juego => juego.idJuego == _juegoObtenido.id);
                    if (juegoAEliminar != null)
                    {
                        Estaticas.juegosFavoritos.Remove(juegoAEliminar);
                    }
                }
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                this.Close();
            }
        }


        private async void VerReseñas_Click(object sender, RoutedEventArgs e)
        {
            await ServicioNotificacion.SuscribirseCanalInteraccionReseñasDeJuego(_juegoObtenido.id);
            VentanaReseñasJugadores ventanaReseñasJugadores = new VentanaReseñasJugadores(_juegoObtenido);
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaReseñasJugadores);
            this.Close();
        }

        private void Reseñar_Click(object sender, RoutedEventArgs e)
        {
            VentanaReseñarJuego ventanaReseñarJuego = new VentanaReseñarJuego(_juegoObtenido,"Descripcion");
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaReseñarJuego);
            this.Close();
        }

        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            VentanaBuscarJuego ventanaBuscarJuego = new VentanaBuscarJuego();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaBuscarJuego);
            this.Close();
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            VentanaBuscarJuego ventanaBuscarJuego = new VentanaBuscarJuego();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaBuscarJuego);
            this.Close();
        }
    }

    public class EstrellaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float rating && parameter != null)
            {
                if (!int.TryParse(parameter.ToString(), out int estrellaNumero))
                    return null!;

                string imagen = "/Imagenes/Iconos/estrella_vacia.png";

                if (rating >= estrellaNumero)
                {
                    imagen = "/Imagenes/Iconos/estrella_llena.png";
                }
                else if (rating >= estrellaNumero - 0.5f)
                {
                    imagen = "/Imagenes/Iconos/estrella_media.png";
                }

                return new BitmapImage(new Uri(imagen, UriKind.Relative));
            }

            return null!;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
