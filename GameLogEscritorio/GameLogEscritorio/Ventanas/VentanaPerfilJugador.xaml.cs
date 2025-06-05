using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Acceso;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Seguidor;
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

        private readonly IApiRestRespuestaFactory apiRestCreadorRespuesta = new FactoryRespuestasAPI();
        private PerfilJugador _perfilJugador = new PerfilJugador();
        private ObservableCollection<JuegoCompleto>? _juegosFavoritos = new ObservableCollection<JuegoCompleto>();

        public VentanaPerfilJugador(PerfilJugador perfil, ObservableCollection<JuegoCompleto> juegosFavoritos)
        {
            InitializeComponent();
            this._perfilJugador = perfil;
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
                if (!_perfilJugador.tipoDeAcceso!.Equals(Constantes.tipoJugadorPorDefecto))
                {
                    btn_Banear.Visibility = Visibility.Collapsed;
                    btn_Desbanear.Visibility = Visibility.Collapsed;
                }
                else if (_perfilJugador.estado!.Equals(Constantes.TipoDeEstadoPorDefecto))
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
            if(_perfilJugador.idJugador == UsuarioSingleton.Instancia.idJugador)
            {
                btn_DejarDeSeguir.Visibility = Visibility.Collapsed;
                btn_Seguir.Visibility = Visibility.Collapsed;
            }
            else if (Estaticas.idJugadoresSeguido.Contains(_perfilJugador.idJugador))
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
                txt_SinJuegosFavoritos.Visibility= Visibility.Visible;
            }
            
        }

        public void CargarDatosUsuario()
        {
            txt_Descripcion.Text = _perfilJugador.descripcion;
            txt_NombreCompleto.Text = _perfilJugador.nombreCompleto;
            txt_NombreUsuario.Text = _perfilJugador.nombreDeUsuario;
            img_FotoPerfil.Source = ConvertirBytesAImagen(_perfilJugador.fotoDePerfil!);
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


        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            VentanaBuscarJugador ventanaBuscarJugador = new VentanaBuscarJugador();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaBuscarJugador);
            this.Close();
        }


        private async void Seguir_Click(object sender, RoutedEventArgs e)
        {
            PostSeguidorSolicitud postSeguidorSolicitud = new PostSeguidorSolicitud()
            {
                idJugadorSeguidor = UsuarioSingleton.Instancia.idJugador,
                idJugadorSeguido = _perfilJugador.idJugador
            };
            ApiRespuestaBase respuestaBase = await ServicioSeguidor.RegistrarNuevoSeguidor(postSeguidorSolicitud, apiRestCreadorRespuesta);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasBase(respuestaBase);
            if (!esRespuestaCritica)
            {
                btn_Seguir.Visibility = Visibility.Collapsed;
                btn_DejarDeSeguir.Visibility = Visibility.Visible;
                Estaticas.idJugadoresSeguido.Add(_perfilJugador.idJugador);
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                this.Close();
            }
        }

        private async void Banear_Click(object sender, RoutedEventArgs e)
        {
            PatchAccesoSolicitud datosSolicitud = new PatchAccesoSolicitud()
            {
                estadoAcceso = "Baneado"
            };
            ApiRespuestaBase respuestaBase = await ServicioAcceso.CambiarEstadoDeAcceso(datosSolicitud, _perfilJugador.idCuenta,apiRestCreadorRespuesta);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasBase(respuestaBase);
            if (!esRespuestaCritica)
            {
                btn_Banear.Visibility = Visibility.Collapsed;
                btn_Desbanear.Visibility = Visibility.Visible;
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                this.Close();
            }
        }

        private async void Desbanear_Click(object sender, RoutedEventArgs e)
        {
            PatchAccesoSolicitud datosSolicitud = new PatchAccesoSolicitud()
            {
                estadoAcceso = "Desbaneado"
            };
            ApiRespuestaBase respuestaBase = await ServicioAcceso.CambiarEstadoDeAcceso(datosSolicitud, _perfilJugador.idCuenta,apiRestCreadorRespuesta);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasBase(respuestaBase);
            if (!esRespuestaCritica)
            {
                btn_Banear.Visibility = Visibility.Visible;
                btn_Desbanear.Visibility = Visibility.Collapsed;
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                this.Close();
            }
        }

        private async void DejarDeSeguir_Click(object sender, RoutedEventArgs e)
        {
            ApiRespuestaBase respuestaBase = await ServicioSeguidor.EliminarJugadorSeguido(UsuarioSingleton.Instancia.idJugador, _perfilJugador.idJugador,apiRestCreadorRespuesta);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasBase(respuestaBase);
            if (!esRespuestaCritica)
            {
                btn_Seguir.Visibility = Visibility.Visible;
                btn_DejarDeSeguir.Visibility = Visibility.Collapsed;
                Estaticas.idJugadoresSeguido.Remove(_perfilJugador.idJugador);
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                this.Close();
            }
        }

        public void DecorarNombre()
        {
            if (!_perfilJugador.tipoDeAcceso!.Equals(Constantes.tipoJugadorPorDefecto))
            {
                LinearGradientBrush arcoiris = new LinearGradientBrush();
                arcoiris.StartPoint = new Point(0, 0);
                arcoiris.EndPoint = new Point(1, 0);
                arcoiris.GradientStops.Add(new GradientStop(Colors.Red, 0.0));
                arcoiris.GradientStops.Add(new GradientStop(Colors.Orange, 0.2));
                arcoiris.GradientStops.Add(new GradientStop(Colors.Yellow, 0.4));
                arcoiris.GradientStops.Add(new GradientStop(Colors.Green, 0.6));
                arcoiris.GradientStops.Add(new GradientStop(Colors.Blue, 0.8));
                arcoiris.GradientStops.Add(new GradientStop(Colors.Purple, 1.0));
                txt_NombreUsuario.Foreground = arcoiris;
            }
            else
            {
                txt_NombreUsuario.Foreground = new SolidColorBrush(Colors.White);
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
