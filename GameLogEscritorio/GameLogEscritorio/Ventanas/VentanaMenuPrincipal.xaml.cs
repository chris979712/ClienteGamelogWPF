using GameLogEscritorio.Servicios.APIRawg.Modelo;
using GameLogEscritorio.Servicios.APIRawg.Servicio;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Reseñas;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Utilidades;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static GameLogEscritorio.Ventanas.VentanaMiReseña;

namespace GameLogEscritorio.Ventanas
{

    public partial class MenuPrincipal : Window
    {

        private readonly IApiRestRespuestaFactory apiRestCreadorRespuesta = new FactoryRespuestasAPI();

        public MenuPrincipal()
        {
            InitializeComponent();
            txt_Jugador.Text = "Bienvenido " + UsuarioSingleton.Instancia.nombreDeUsuario!.ToString();
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

        public void IrVentanaSeguidores_Click(object sender,RoutedEventArgs e)
        {

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

        public async void CerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            await ManejadorSesion.RegresarInicioDeSesionUsuario();
            this.Close();
        }
    }

}
