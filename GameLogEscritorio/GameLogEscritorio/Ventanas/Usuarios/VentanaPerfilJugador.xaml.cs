using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Acceso;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Social;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Utilidades;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameLogEscritorio.Ventanas
{
    
    public partial class VentanaPerfilJugador : Window
    {

        private readonly IApiRestRespuestaFactory apiRestCreadorRespuesta = new FactoryRespuestasApi();

        public PerfilJugador perfilJugador = new PerfilJugador();

        private ObservableCollection<JuegoCompleto>? _juegosFavoritos = new ObservableCollection<JuegoCompleto>();

        public VentanaPerfilJugador(PerfilJugador perfil, ObservableCollection<JuegoCompleto> juegosFavoritos)
        {
            InitializeComponent();
            this.perfilJugador = perfil;
            this._juegosFavoritos = juegosFavoritos;
            DecorarNombre();
            CargarPermisosUsuario();
            CargarBotonesDeSeguimiento();
            CargarDatosUsuario();
            CargarJuegosFavoritos();
            Estaticas.GuardarMedidasUltimaVentana(this);
        }

        public void CargarPermisosUsuario()
        {
            if(UsuarioSingleton.Instancia.tipoDeAcceso!.Equals(Constantes.tipoJugadorPorDefecto))
            {
                btn_Banear.Visibility = Visibility.Collapsed;
                btn_Desbanear.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (!perfilJugador.tipoDeAcceso!.Equals(Constantes.tipoJugadorPorDefecto))
                {
                    btn_Banear.Visibility = Visibility.Collapsed;
                    btn_Desbanear.Visibility = Visibility.Collapsed;
                }
                else if (perfilJugador.estado!.Equals(Constantes.TipoDeEstadoPorDefecto))
                {
                    btn_Banear.Visibility = Visibility.Visible;
                    btn_Desbanear.Visibility= Visibility.Collapsed;
                }
                else
                {
                    btn_Banear.Visibility = Visibility.Collapsed;
                    btn_Desbanear.Visibility = Visibility.Visible;
                }
            }
        }

        public void CargarBotonesDeSeguimiento()
        {
            if(perfilJugador.idJugador == UsuarioSingleton.Instancia.idJugador)
            {
                btn_DejarDeSeguir.Visibility = Visibility.Collapsed;
                btn_Seguir.Visibility = Visibility.Collapsed;
            }
            else if (Estaticas.idJugadoresSeguido.Contains(perfilJugador.idJugador))
            {
                btn_DejarDeSeguir.Visibility = Visibility.Visible;
                btn_Seguir.Visibility = Visibility.Collapsed;
            }
            else
            {
                btn_DejarDeSeguir.Visibility = Visibility.Collapsed;
                btn_Seguir.Visibility = Visibility.Visible;
            }
        }

        public void CargarJuegosFavoritos()
        {
            if (_juegosFavoritos?.Count >= 1)
            {
                ic_JuegosFavoritos.Visibility = Visibility.Visible;
                ic_JuegosFavoritos.ItemsSource = _juegosFavoritos;
            }
            else
            {
                txbl_SinJuegosFavoritos.Visibility= Visibility.Visible;
            }
            
        }

        public void CargarDatosUsuario()
        {
            txbl_Descripcion.Text = perfilJugador.descripcion;
            txbl_NombreCompleto.Text = perfilJugador.nombreCompleto;
            txbl_NombreUsuario.Text = perfilJugador.nombreDeUsuario;
            img_FotoPerfil.Source = ConvertirBytesAImagen(perfilJugador.fotoDePerfil!);
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


        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            VentanaBuscarJugador ventanaBuscarJugador = new VentanaBuscarJugador();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaBuscarJugador);
            this.Close();
        }


        private async void Seguir_Click(object sender, RoutedEventArgs e)
        {
            grd_OverlayCarga.Visibility = Visibility.Visible;
            PostSeguidorSolicitud postSeguidorSolicitud = new PostSeguidorSolicitud()
            {
                idJugadorSeguidor = UsuarioSingleton.Instancia.idJugador,
                idJugadorSeguido = perfilJugador.idJugador
            };
            ApiRespuestaBase respuestaBase = await ServicioSeguidor.RegistrarNuevoSeguidor(postSeguidorSolicitud, apiRestCreadorRespuesta);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasBase(respuestaBase);
            if (!esRespuestaCritica)
            {
                btn_Seguir.Visibility = Visibility.Collapsed;
                btn_DejarDeSeguir.Visibility = Visibility.Visible;
                grd_OverlayCarga.Visibility = Visibility.Collapsed;
                Estaticas.idJugadoresSeguido.Add(perfilJugador.idJugador);
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                grd_OverlayCarga.Visibility = Visibility.Collapsed;
                this.Close();
            }
        }

        private async void Banear_Click(object sender, RoutedEventArgs e)
        {
            VentanaDeConfirmacion ventanaDeConfirmacion = new VentanaDeConfirmacion(Properties.Resources.ConfirmacionBanearUsuario, this);
            bool? resultadoConfirmacion = ventanaDeConfirmacion.ShowDialog();
            if (resultadoConfirmacion == true)
            {
                await PonerEnListaNegraUsuarioSeleccionado();
            }
        }

        private async Task PonerEnListaNegraUsuarioSeleccionado()
        {
            grd_OverlayCarga.Visibility = Visibility.Visible;
            PatchAccesoSolicitud datosSolicitud = new PatchAccesoSolicitud()
            {
                estadoAcceso = Constantes.Baneado
            };
            ApiRespuestaBase respuestaBase = await ServicioAcceso.CambiarEstadoDeAcceso(datosSolicitud, perfilJugador.idCuenta, apiRestCreadorRespuesta);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasBase(respuestaBase);
            if (!esRespuestaCritica)
            {
                btn_Banear.Visibility = Visibility.Collapsed;
                btn_Desbanear.Visibility = Visibility.Visible;
                grd_OverlayCarga.Visibility = Visibility.Collapsed;
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                grd_OverlayCarga.Visibility = Visibility.Collapsed;
                this.Close();
            }
        }

        private async void Desbanear_Click(object sender, RoutedEventArgs e)
        {
            grd_OverlayCarga.Visibility = Visibility.Visible;
            PatchAccesoSolicitud datosSolicitud = new PatchAccesoSolicitud()
            {
                estadoAcceso = Constantes.Desbaneado
            };
            ApiRespuestaBase respuestaBase = await ServicioAcceso.CambiarEstadoDeAcceso(datosSolicitud, perfilJugador.idCuenta,apiRestCreadorRespuesta);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasBase(respuestaBase);
            if (!esRespuestaCritica)
            {
                btn_Banear.Visibility = Visibility.Visible;
                btn_Desbanear.Visibility = Visibility.Collapsed;
                grd_OverlayCarga.Visibility = Visibility.Collapsed;
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                grd_OverlayCarga.Visibility = Visibility.Collapsed;
                this.Close();
            }
        }

        private async void DejarDeSeguir_Click(object sender, RoutedEventArgs e)
        {
            VentanaDeConfirmacion ventanaDeConfirmacion = new VentanaDeConfirmacion(Properties.Resources.ConfirmaciónEliminacionSeguido, this);
            bool? resultadoConfirmacion = ventanaDeConfirmacion.ShowDialog();
            if (resultadoConfirmacion == true)
            {
                await DejarDeSeguirUsuario();
            }
        }

        private async Task DejarDeSeguirUsuario()
        {
            grd_OverlayCarga.Visibility = Visibility.Visible;
            ApiRespuestaBase respuestaBase = await ServicioSeguidor.EliminarJugadorSeguido(UsuarioSingleton.Instancia.idJugador, perfilJugador.idJugador, apiRestCreadorRespuesta);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasBase(respuestaBase);
            if (!esRespuestaCritica)
            {
                btn_Seguir.Visibility = Visibility.Visible;
                btn_DejarDeSeguir.Visibility = Visibility.Collapsed;
                grd_OverlayCarga.Visibility = Visibility.Collapsed;
                Estaticas.idJugadoresSeguido.Remove(perfilJugador.idJugador);
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                grd_OverlayCarga.Visibility = Visibility.Collapsed;
                this.Close();
            }
        }

        public void DecorarNombre()
        {
            if (!perfilJugador.tipoDeAcceso!.Equals(Constantes.tipoJugadorPorDefecto))
            {
                LinearGradientBrush purpuraLiderazgo = new LinearGradientBrush();
                purpuraLiderazgo.StartPoint = new Point(0, 0);
                purpuraLiderazgo.EndPoint = new Point(1, 0);
                purpuraLiderazgo.GradientStops.Add(new GradientStop(Color.FromRgb(75, 0, 130), 0.0));
                purpuraLiderazgo.GradientStops.Add(new GradientStop(Color.FromRgb(148, 0, 211), 0.5));
                purpuraLiderazgo.GradientStops.Add(new GradientStop(Color.FromRgb(255, 0, 255), 1.0));
                txbl_NombreUsuario.Foreground = purpuraLiderazgo;
            }
            else
            {
                txbl_NombreUsuario.Foreground = new SolidColorBrush(Colors.White);
            }
        }

    }

    public partial class PerfilJugador
    {
        public int idCuenta { get; set; }

        public int idJugador { get; set; }

        public string? nombre { get; set; }

        public string? primerApellido { get; set; }

        public string? segundoApellido { get; set; }

        public string? nombreDeUsuario { get; set; }

        public string? descripcion { get; set; }

        public string? foto { get; set; }

        public byte[]? fotoDePerfil { get; set; }

        public string? tipoDeAcceso { get; set; }

        public string? estado { get; set; }

        public string nombreCompleto => $"{nombre} {primerApellido} {segundoApellido}";
    }
}
