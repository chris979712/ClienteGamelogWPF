using GameLogEscritorio.Servicios.APIRawg.Modelo;
using GameLogEscritorio.Servicios.APIRawg.Servicio;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Notificacion;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Reseñas;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Utilidades;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using static GameLogEscritorio.Ventanas.VentanaMiReseña;

namespace GameLogEscritorio.Ventanas
{

    public partial class MenuPrincipal : Window
    {

        private readonly IApiRestRespuestaFactory apiRestCreadorRespuesta = new FactoryRespuestasAPI();
        private bool notificacionesVisible = false;


        public MenuPrincipal()
        {
            InitializeComponent();
            txt_Jugador.Text = Properties.Resources.Bienvenido + UsuarioSingleton.Instancia.nombreDeUsuario!.ToString();
            if (Estaticas.juegosPendientes.Count >= 1)
            {
                ic_JuegosPendientes.ItemsSource = Estaticas.juegosPendientes;
                ic_JuegosPendientes.Visibility = Visibility.Visible;
                txt_SinJuegos.Visibility = Visibility.Collapsed;
            }
            else
            {
                ic_JuegosPendientes.Visibility = Visibility.Collapsed;
                txt_SinJuegos.Visibility = Visibility.Visible;
            }
            img_FotoDePerfil.Source = ConvertirBytesAImagen(UsuarioSingleton.Instancia.fotoDePerfil!);
            DecorarNombre();
            Estaticas.GuardarMedidasUltimaVentana(this);
            pnl_Notificaciones.Margin = new Thickness(0, 0, -350, 0);
            CargarBotonesCorrespondientes();
        }

        private void CargarBotonesCorrespondientes()
        {
            if(UsuarioSingleton.Instancia.tipoDeAcceso != Constantes.tipoJugadorPorDefecto)
            {
                btn_MisReseñas.Visibility = Visibility.Collapsed;
                txt_SinJuegos.Visibility = Visibility.Collapsed;
                txtbl_JuegosPendientes.Visibility = Visibility.Collapsed;
            }
        }

        public void DecorarNombre()
        {
            if (!UsuarioSingleton.Instancia.tipoDeAcceso!.Equals(Constantes.tipoJugadorPorDefecto))
            {
                LinearGradientBrush purpuraLiderazgo = new LinearGradientBrush();
                purpuraLiderazgo.StartPoint = new Point(0, 0);
                purpuraLiderazgo.EndPoint = new Point(1, 0);
                purpuraLiderazgo.GradientStops.Add(new GradientStop(Color.FromRgb(75, 0, 130), 0.0));
                purpuraLiderazgo.GradientStops.Add(new GradientStop(Color.FromRgb(148, 0, 211), 0.5));
                purpuraLiderazgo.GradientStops.Add(new GradientStop(Color.FromRgb(255, 0, 255), 1.0));
                txt_Jugador.Foreground = purpuraLiderazgo;
            }
            else
            {
                txt_Jugador.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC000"));
            }
        }

        public static ImageSource ConvertirBytesAImagen(byte[] imagenBytes)
        {
            using (var ms = new MemoryStream(imagenBytes))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                bitmap.Freeze(); 
                return bitmap;
            }
        }

        public void IrVentanaEditarPerfil_Click(object sender, RoutedEventArgs e)
        {
            VentanaEditarPerfil ventanaEditarPerfil = new VentanaEditarPerfil();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.ActualWidth, this.ActualHeight, ventanaEditarPerfil);
            this.Close();
        }

        public void IrVentanaBuscarJuego_Click(object sender, RoutedEventArgs e)
        {
            VentanaBuscarJuego ventanaBuscarJuego = new VentanaBuscarJuego();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaBuscarJuego);
            this.Close();
        }

        public void IrVentanaBuscarUsuario_Click(object sender, RoutedEventArgs e)
        {
            VentanaBuscarJugador ventanaBuscarJugador = new VentanaBuscarJugador();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaBuscarJugador);
            this.Close();
        }

        public async void IrVentanaMisReseñas_Click(object sender, RoutedEventArgs e)
        {
            ApiReseñaPersonalRespuesta respuestaReseñasObtenidas = await ServicioReseña.ObtenerReseñasDeUnJugador(UsuarioSingleton.Instancia.idJugador, UsuarioSingleton.Instancia.idJugador,apiRestCreadorRespuesta);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasConDatosODiferentesAlCodigoDeExito(respuestaReseñasObtenidas);
            if (!esRespuestaCritica)
            { 
                if(respuestaReseñasObtenidas.estado == Constantes.CodigoExito)
                {
                    List<ReseñaPersonal> reseñasObtenidas = respuestaReseñasObtenidas.reseñasPersonales!;
                    BuscarJuegosReseñadosPorUsuarioUsuario(reseñasObtenidas);
                }  
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                this.Close();
            }
        }

        private async void BuscarJuegosReseñadosPorUsuarioUsuario(List<ReseñaPersonal> reseñasJugador) 
        {
            ObservableCollection<ReseñaJugador> reseñasObtenidas = new ObservableCollection<ReseñaJugador>();
            foreach (var reseña in reseñasJugador)
            {
                JuegoModelo juegoObtenido = await ServicioBuscarJuego.BuscarJuegoPorID(reseña.idJuego);
                if(juegoObtenido.id == Constantes.ErrorEnLaOperacion)
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, juegoObtenido.detail!,Constantes.CodigoErrorServidor);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaEmergente);
                    break;
                }
                else
                {
                    DateTime fecha = DateTime.Parse(reseña.fecha!);
                    string fecharFormateada = fecha.ToString("yyyy-MM-dd");
                    reseñasObtenidas.Add(new ReseñaJugador()
                    {
                        calificacion = reseña.calificacion,
                        opinion = reseña.opinion,
                        fotoVideojuego = juegoObtenido.backgroundImage,
                        fecha = fecharFormateada,
                        nombre = juegoObtenido.name
                    });
                }
            }
            Estaticas.reseñasJugador = reseñasObtenidas;
            VentanaHistorialDeReseñas ventanaHistorialDeReseñas = new VentanaHistorialDeReseñas(reseñasObtenidas);
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaHistorialDeReseñas);
            this.Close();
        }

        public void IrVentanaTendencias_Click(object sender, RoutedEventArgs e)
        {
            VentanaReporteTendencias ventanaReporteTendencias = new VentanaReporteTendencias();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaReporteTendencias);
            this.Close();
        }

        public void IrVentanaSocial_Click(object sender,RoutedEventArgs e)
        {
            VentanaSocial ventanaMisSeguidores = new VentanaSocial();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaMisSeguidores);
            this.Close();
        }

        public void IrAVentanaeseñarJuego_Click(object sender, RoutedEventArgs e)
        {
            Image? imagen = sender as Image;
            if (imagen != null)
            {
                var juegoSeleccionado = imagen.DataContext as JuegoCompleto;
                if (juegoSeleccionado != null)
                {
                    JuegoModelo modeloJuego = new JuegoModelo()
                    {
                        id = juegoSeleccionado.idJuego,
                        description = juegoSeleccionado.descripcion,
                        backgroundImage = juegoSeleccionado.imagenFondo,
                        name = juegoSeleccionado.nombre
                    };
                    VentanaReseñarJuego reseñarJuego = new VentanaReseñarJuego(modeloJuego, "Menu");
                    AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, reseñarJuego);
                    this.Close();
                }
            }
        }

        private void VerNotificaciones_Click(object sender, RoutedEventArgs e)
        {
            if (notificacionesVisible)
            {
                CerrarPanelNotificaciones();
            }
            else
            {
                AbrirPanelNotificaciones();
            }
        }

        private void AbrirPanelNotificaciones()
        {
            var showAnimation = new ThicknessAnimation
            {
                To = new Thickness(0, 0, 0, 0), 
                Duration = TimeSpan.FromSeconds(0.3),
                AccelerationRatio = 0.2
            };
            pnl_Notificaciones.BeginAnimation(FrameworkElement.MarginProperty, showAnimation);
            notificacionesVisible = true;
            ObtenerNotificaciones();
        }

        private void CerrarPanelNotificaciones()
        {
            var hideAnimation = new ThicknessAnimation
            {
                To = new Thickness(0, 0, -350, 0),
                Duration = TimeSpan.FromSeconds(0.3),
                AccelerationRatio = 0.2
            };
            pnl_Notificaciones.BeginAnimation(FrameworkElement.MarginProperty, hideAnimation);
            notificacionesVisible = false;
        }

        private void CerrarNotificaciones_Click(object sender, RoutedEventArgs e)
        {
            CerrarPanelNotificaciones();
        }

        private async void ObtenerNotificaciones()
        {
            ApiNotificacionRespuesta apiNotificacionRespuesta = await ServicioNotificaciones.ObtenerNotificacionesDeJugador(UsuarioSingleton.Instancia.idJugador, apiRestCreadorRespuesta);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasConDatosODiferentesAlCodigoDeExito(apiNotificacionRespuesta);
            if (!esRespuestaCritica)
            {
                if (apiNotificacionRespuesta.estado == Constantes.CodigoExito)
                {
                    CargarNotificaciones(apiNotificacionRespuesta.notificaciones!);
                }
                else if(apiNotificacionRespuesta.estado == Constantes.CodigoSinResultadosEncontrados)
                {
                    CerrarPanelNotificaciones();
                    notificacionesVisible = false;
                }
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                this.Close();
            }
        }


        private void CargarNotificaciones(List<Notificaciones> notificacionesObtenidas)
        {
            foreach(var notificacion in notificacionesObtenidas)
            {
                bool existeNotificacion = Estaticas.notificaciones.Any(notificacionGuardada => notificacionGuardada.Id == notificacion.idNotificacion);
                if (!existeNotificacion)
                {
                    NotificacionCompleta notificacionCompleta = new NotificacionCompleta()
                    {
                        Id = notificacion.idNotificacion,
                        Mensaje = notificacion.mensajeNotificacion,
                        fecha = notificacion.fechaNotificacion
                    };
                    Estaticas.notificaciones.Insert(0,notificacionCompleta);
                }
            }
            ic_Notificaciones.ItemsSource = Estaticas.notificaciones;
        }

        private async void EliminarNotificacion_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is NotificacionCompleta notificacionObtenida)
            {
                if(button != null)
                {
                    ApiRespuestaBase apiRespuestaBase = await ServicioNotificaciones.EliminarNotificacion(notificacionObtenida.Id, apiRestCreadorRespuesta);
                    bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasConDatosODiferentesAlCodigoDeExito(apiRespuestaBase);
                    if (!esRespuestaCritica)
                    {
                        if(apiRespuestaBase.estado == Constantes.CodigoExito)
                        {
                            NotificacionCompleta? notificacionAEliminar = Estaticas.notificaciones.Where(notificacion => notificacion.Id == notificacionObtenida.Id || notificacion.Mensaje == notificacionObtenida.Mensaje).FirstOrDefault();
                            if(notificacionAEliminar!= null)
                            {
                                Estaticas.notificaciones.Remove(notificacionAEliminar);
                            }
                        }
                    }
                    else
                    {
                        await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                        this.Close();
                    }
                }
            }
        }


        public async void CerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            await ManejadorSesion.RegresarInicioDeSesionUsuario();
            this.Close();
        }
    }

    public class NotificacionCompleta 
    {
        public string FechaFormateada
        {
            get
            {
                if (DateTime.TryParse(fecha, out var fechaConvertida))
                {
                    fecha = fechaConvertida.ToString("dd/MM/yyyy");
                }
                return fecha!;
            }
        }
        public int Id { get; set; }
        public string? Mensaje { get; set; }
        public string? fecha { get; set; }
    }

}
