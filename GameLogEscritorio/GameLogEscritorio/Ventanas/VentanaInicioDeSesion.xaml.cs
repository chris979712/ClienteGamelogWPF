using GameLogEscritorio.Servicios.APIRawg.Servicio;
using GameLogEscritorio.Servicios.GameLogAPIGRPC.Respuesta;
using GameLogEscritorio.Servicios.GameLogAPIGRPC.Servicio;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Juegos;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Login;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Utilidades;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameLogEscritorio.Ventanas
{
    public partial class VentanaInicioDeSesion : Window
    {
        private readonly IApiRestRespuestaFactory apiRestCreadorRespuesta = new FactoryRespuestasAPI();
        public VentanaInicioDeSesion()
        {
            InitializeComponent();
            Estaticas.GuardarMedidasUltimaVentana(this);
        }

        private async void IniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            if(ValidarDatos())
            {
                CambiarColorCampos();
                string correo = txtb_Correo.Text;
                string contraseniaHasheada = Encriptador.hasheoA256(pb_Contrasenia.Password);
                var datosSolicitud = new PostLoginSolicitud
                {
                    correo = correo,
                    contrasenia = contraseniaHasheada,
                    tipoDeUsuario = Properties.Resources.tipoDeUsuarioPorDefecto.ToString()
                };
                var respuesta = await ServicioLogin.IniciarSesion(datosSolicitud,apiRestCreadorRespuesta);
                if (respuesta.estado == 200)
                {
                    UsuarioSingleton.Instancia.IniciarSesion(respuesta.cuenta!.FirstOrDefault()!);
                    Estaticas.juegosPendientes = await ServicioBuscarJuego.ObtenerJuegosPendientesJugador(UsuarioSingleton.Instancia.idJugador);
                    ApiSeguidosRespuesta seguidosRespuesta = await ServicioSeguidor.ObtenerJugadoresSeguidos(UsuarioSingleton.Instancia.idJugador,apiRestCreadorRespuesta);
                    ApiJuegosRespuesta juegosFavoritosObtenidos = await ServicioJuego.ObtenerJuegosFavoritos(UsuarioSingleton.Instancia.idJugador,apiRestCreadorRespuesta);
                    await CargarFotoDePerfilUsuario();
                    VerificarCargaCorrectaDeElementos(juegosFavoritosObtenidos, seguidosRespuesta, Estaticas.juegosPendientes);
                }
                else
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuesta.mensaje!, respuesta.estado);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaEmergente);
                }
            }
            else
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, Constantes.ContenidoDatosInvalidos, Constantes.CodigoErrorSolicitud);
                AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaEmergente);
            }
        }

        private void VerificarCargaCorrectaDeElementos(ApiJuegosRespuesta juegosFavoritosObtenidos,ApiSeguidosRespuesta seguidosObtenidosRespuesta,ObservableCollection<JuegoCompleto> juegosPendientes)
        {
            bool errorEnPendientes = Estaticas.juegosPendientes.Count == 1 && Estaticas.juegosPendientes[0].idJuego == Constantes.CodigoErrorSolicitud;
            bool errorEnFavoritos = juegosFavoritosObtenidos.estado == Constantes.CodigoErrorSolicitud || juegosFavoritosObtenidos.estado == Constantes.CodigoErrorServidor;
            bool errorEnSeguidos =seguidosObtenidosRespuesta.estado == Constantes.CodigoErrorSolicitud || seguidosObtenidosRespuesta.estado == Constantes.CodigoErrorServidor;
            bool errorEnJuegoPendiente = juegosPendientes.Count >= 1 && (juegosPendientes[0].idJuego == Constantes.ErrorEnLaOperacion ||
                 juegosPendientes[0].idJuego == Constantes.CodigoErrorSolicitud || juegosPendientes[0].idJuego == Constantes.CodigoErrorServidor);

            if (errorEnPendientes || errorEnFavoritos || errorEnSeguidos || errorEnJuegoPendiente)
            {
                new VentanaEmergente(Constantes.TipoAdvertencia, Properties.Resources.ErrorEnLaCargaDatosUsuario, Constantes.CodigoErrorServidor);
            }
            else
            {
                CargarListaJugadoresSeguidos(seguidosObtenidosRespuesta);
                CargarListaDeJuegos(juegosFavoritosObtenidos);
                MenuPrincipal menuPrincipal = new MenuPrincipal();
                AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left,this.Width,this.Height, menuPrincipal);
                this.Close();
            }

        }

        private void CargarListaJugadoresSeguidos(ApiSeguidosRespuesta jugadoresSeguidos)
        {
            if(jugadoresSeguidos.jugadoresSeguidos?.Count >= 1)
            {
                foreach(var jugadorSeguido in jugadoresSeguidos.jugadoresSeguidos)
                {
                    Estaticas.idJugadoresSeguido.Add(jugadorSeguido.idJugador);
                }
            }
        }

        public void TextoSugeridoGtFocus (object sender, RoutedEventArgs e)
        {
            txtb_Sugerencia.Visibility = Visibility.Collapsed;
        }
        public void TextoSugeridoLosFocus(object sender, RoutedEventArgs e)
        {
            txtb_Sugerencia.Visibility = Visibility.Visible;
        }

        private void PasswordVisible(object sender, RoutedEventArgs e)
        {
            txtb_Sugerencia.Visibility = string.IsNullOrEmpty(pb_Contrasenia.Password) ? Visibility.Visible : Visibility.Collapsed; 
        }


        private async Task<bool> CargarFotoDePerfilUsuario()
        {
            bool fotoEncontrada = false;
            RespuestaGRPC respuestaGRPC = await ServicioFotoDePerfil.ObtenerFotoJugador(UsuarioSingleton.Instancia.foto!);
            if(respuestaGRPC.codigo == Constantes.CodigoExito)
            {
                UsuarioSingleton.Instancia.fotoDePerfil = respuestaGRPC.datosBinario;
                fotoEncontrada = true;
            }
            else
            {
                UsuarioSingleton.Instancia.fotoDePerfil = FotoPorDefecto.ObtenerFotoDePerfilPorDefecto();
                ManejadorRespuestas.ManejadorRespuestasGRPC(respuestaGRPC.codigo);
                fotoEncontrada = false;
            }
            return fotoEncontrada;
        }

        private void CargarListaDeJuegos(ApiJuegosRespuesta juegosFavoritosObtenidos)
        {
            if(Estaticas.juegosFavoritos != null)
            {
                Estaticas.juegosFavoritos = new List<Juego>();
            }
            else
            {
                Estaticas.juegosFavoritos = juegosFavoritosObtenidos.juegos!;
            }
        }


        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove(); 
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); 
        }

        private void IrVentanaRecuperarContraseña(object sender, MouseButtonEventArgs e)
        {
            VentanaRecuperarContrasenia ventanaRecuperarContrasenia = new VentanaRecuperarContrasenia();
            AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(this.Top, this.Left,this.Width,this.Height, ventanaRecuperarContrasenia);
        }

        private void CrearCuenta_Click(object sender, RoutedEventArgs e)
        {
            VentanaRegistroDeCuenta ventanaRegistroDeCuenta = new VentanaRegistroDeCuenta();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left,this.Width,this.Height, ventanaRegistroDeCuenta);
            this.Close();
        }

        private bool ValidarDatos()
        {
            bool correoValidacion = Validador.ValidarCorreo(txtb_Correo.Text);
            bool contraseniaValidacion = Validador.ValidarContrasenia(pb_Contrasenia.Password);

            if(!correoValidacion)
            {
                txtb_Correo.BorderBrush = Brushes.Red;
            }

            if(!contraseniaValidacion)
            {
                pb_Contrasenia.BorderBrush = Brushes.Red;
            }

            return correoValidacion && contraseniaValidacion;
        }

        private void CambiarColorCampos()
        {
            txtb_Correo.BorderBrush = Brushes.White;
            pb_Contrasenia.BorderBrush = Brushes.White;
        }

        private void CambiarVisibilidadContrasenia(object sender, RoutedEventArgs e)
        {
            if (tbtn_VisibilidadContrasenia.IsChecked == true)
            {
                txtb_Contrasenia.Text = pb_Contrasenia.Password;
                pb_Contrasenia.Visibility = Visibility.Collapsed;
                txtb_Contrasenia.Visibility = Visibility.Visible;

                img_Visibilidad.Source = new BitmapImage(new Uri("pack://application:,,,/Imagenes/Iconos/IconoVisibleOscuro.png"));
            }
            else
            {
                pb_Contrasenia.Password = txtb_Contrasenia.Text;
                pb_Contrasenia.Visibility = Visibility.Visible;
                txtb_Contrasenia.Visibility = Visibility.Collapsed;

                img_Visibilidad.Source = new BitmapImage(new Uri("pack://application:,,,/Imagenes/Iconos/IconoNoVisibleOscuro.png"));
            }
        }

    }
}
