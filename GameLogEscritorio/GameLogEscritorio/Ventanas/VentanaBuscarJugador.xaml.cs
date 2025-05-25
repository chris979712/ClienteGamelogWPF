using GameLogEscritorio.Servicios.APIRawg.Modelo;
using GameLogEscritorio.Servicios.APIRawg.Servicio;
using GameLogEscritorio.Servicios.GameLogAPIGRPC.Respuesta;
using GameLogEscritorio.Servicios.GameLogAPIGRPC.Servicio;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Jugador;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Utilidades;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameLogEscritorio.Ventanas
{

    public partial class VentanaBuscarJugador : Window
    {
        private byte[] _fotoDePerfilJugador = new byte[0];

        private Perfil _PerfilJugador = new Perfil();

        public VentanaBuscarJugador()
        {
            InitializeComponent();
            txb_Busqueda.Focus();
        }

        private void LimpiarCampos()
        {
            txb_Busqueda.BorderBrush = Brushes.Yellow;
            grd_resultadoJugador.Visibility = Visibility.Collapsed;
            txb_nombreJugador.Text = "";
            img_jugador.Source = null;
        }

        private async void Buscar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
            if (ValidarDatos())
            {
                await BuscarJugador();
            }
            else
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoInformacion, Constantes.ContenidoDatosInvalidos, Constantes.CodigoErrorSolicitud);
            }
        }

        private async Task BuscarJugador()
        {
            ApiJugadorRespuesta jugadorRespuesta = await ServicioJugador.ObtenerJugadorPorNombreDeUsuario(txb_Busqueda.Text);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasConDatosODiferentesAlCodigoDeExito(jugadorRespuesta);
            if (!esRespuestaCritica)
            {
                if(jugadorRespuesta.estado == Constantes.CodigoExito)
                {
                    _PerfilJugador = jugadorRespuesta.jugador!.FirstOrDefault()!;
                    _fotoDePerfilJugador = await ObtenerFotoDePerfilJugador(_PerfilJugador.foto!);
                    CargarDatosJugador(_PerfilJugador);
                }
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                this.Close();
            }
        }

        private async Task<byte[]> ObtenerFotoDePerfilJugador(string rutaFotoDePerfil)
        {
            byte[] fotoEncontrada = FotoPorDefecto.ObtenerFotoDePerfilPorDefecto();
            RespuestaGRPC respuestaGRPC = await ServicioFotoDePerfil.ObtenerFotoJugador(rutaFotoDePerfil!);
            ManejadorRespuestas.ManejadorRespuestasGRPC(respuestaGRPC.codigo);
            if (respuestaGRPC.codigo == Constantes.CodigoExito)
            {
                fotoEncontrada = respuestaGRPC.datosBinario!;
            }
            return fotoEncontrada;
        }

        private void CargarDatosJugador(Perfil jugadorObtenido)
        {
            grd_resultadoJugador.Visibility = Visibility.Visible;
            if (jugadorObtenido.nombreDeUsuario!.Equals(UsuarioSingleton.Instancia.nombreDeUsuario))
            {
                txb_nombreJugador.Text = jugadorObtenido.nombreDeUsuario + " (TÚ)";
                CargarNombreEstetico(jugadorObtenido);
            }
            else
            {
                txb_nombreJugador.Text = jugadorObtenido.nombreDeUsuario;
                CargarNombreEstetico(jugadorObtenido);
            }
            img_jugador.Source = BytesAImagen(_fotoDePerfilJugador);
        }

        private void CargarNombreEstetico(Perfil jugadorObtenido)
        {
            string nombreDeUsuario = txb_nombreJugador.Text;
            if (!jugadorObtenido.tipoDeAcceso!.Equals(Constantes.tipoJugadorPorDefecto))
            {
                txb_nombreJugador.Text = nombreDeUsuario + " ADMIN";
                LinearGradientBrush arcoiris = new LinearGradientBrush();
                arcoiris.StartPoint = new Point(0, 0);
                arcoiris.EndPoint = new Point(1, 0);
                arcoiris.GradientStops.Add(new GradientStop(Colors.Red, 0.0));
                arcoiris.GradientStops.Add(new GradientStop(Colors.Orange, 0.2));
                arcoiris.GradientStops.Add(new GradientStop(Colors.Yellow, 0.4));
                arcoiris.GradientStops.Add(new GradientStop(Colors.Green, 0.6));
                arcoiris.GradientStops.Add(new GradientStop(Colors.Blue, 0.8));
                arcoiris.GradientStops.Add(new GradientStop(Colors.Purple, 1.0));
                txb_nombreJugador.Foreground = arcoiris;
            }
            else
            {
                txb_nombreJugador.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC000"));
            }
        }

        private void AvatarJugador_Click(object sender, MouseButtonEventArgs e)
        {
            var ventana = new VentanaImagen(_fotoDePerfilJugador); 
            ventana.ShowDialog(); 
        }

        public static BitmapImage BytesAImagen(byte[] imageDatos)
        { 
            using (var ms = new MemoryStream(imageDatos))
            {
                var imagen = new BitmapImage();
                imagen.BeginInit();
                imagen.CacheOption = BitmapCacheOption.OnLoad;
                imagen.StreamSource = ms;
                imagen.EndInit();
                imagen.Freeze();
                return imagen;
            }
        }

        private bool ValidarDatos()
        {
            bool nombreDeUsuarioValido = Validador.ValidarNombreDeUsuario(txb_Busqueda.Text);
            if (!nombreDeUsuarioValido)
            {
                txb_Busqueda.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txb_Busqueda);
            }
            return nombreDeUsuarioValido;
        }

        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            MenuPrincipal menuPrincipal = new MenuPrincipal();
            menuPrincipal.Show();
            this.Close();
        }

        private void Buscar_Enter(object sender,KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Buscar_Click(sender, e);
            }
        }

        private async void Detalles_Click(object sender, RoutedEventArgs e)
        {
            BloquearBotones();
            ObservableCollection<JuegoCompleto> juegosFavoritos = await ServicioBuscarJuego.ObtenerJuegosFavoritosJugador(_PerfilJugador.idJugador);
            bool errorAlObtenerJuegos = juegosFavoritos.Count >= 1 && (juegosFavoritos[0].idJuego == Constantes.CodigoErrorSolicitud || 
                                        juegosFavoritos[0].idJuego == Constantes.CodigoErrorServidor || juegosFavoritos[0].idJuego == Constantes.ErrorEnLaOperacion ||
                                        juegosFavoritos[0].idJuego == Constantes.CodigoErrorAcceso);
            DesbloquearBotones();
            if (errorAlObtenerJuegos)
            {
                if(juegosFavoritos[0].idJuego == Constantes.CodigoErrorAcceso){
                    await ManejadorSesion.CerrarSesionForzadaDeUsuario();
                    this.Close();
                }
                new VentanaEmergente(Constantes.TipoError, juegosFavoritos[0].descripcion!, Constantes.CodigoErrorServidor);

            }
            else
            {
                PerfilJugador perfilJugador = new PerfilJugador()
                {
                    idJugador = _PerfilJugador.idJugador,
                    idCuenta = _PerfilJugador.idCuenta,
                    nombre = _PerfilJugador.nombre,
                    primerApellido = _PerfilJugador.primerApellido,
                    segundoApellido = _PerfilJugador.segundoApellido,
                    nombreDeUsuario = _PerfilJugador.nombreDeUsuario,
                    descripcion = _PerfilJugador.descripcion,
                    foto = _PerfilJugador.foto,
                    fotoDePerfil = await ObtenerFotoDePerfilJugador(_PerfilJugador.foto!),
                    tipoDeAcceso = _PerfilJugador.tipoDeAcceso,
                    estado = _PerfilJugador.estado
                   
                };
                new VentanaPerfilJugador(perfilJugador, juegosFavoritos).Show();
                this.Close();
            }
        }

        public void BloquearBotones()
        {
            btn_Buscar.IsEnabled = false;
            btn_VerPerfil.IsEnabled = false;
        }

        public void DesbloquearBotones()
        {
            btn_Buscar.IsEnabled = true;
            btn_VerPerfil.IsEnabled = true;
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            MenuPrincipal menuPrincipal = new MenuPrincipal();
            menuPrincipal.Show();
            this.Close();
        }
    }
}
