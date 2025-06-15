using GameLogEscritorio.Servicios.GameLogAPIGRPC.Respuesta;
using GameLogEscritorio.Servicios.GameLogAPIGRPC.Servicio;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Social;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Utilidades;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace GameLogEscritorio.Ventanas
{
    
    public partial class VentanaSocial : Window
    {

        private static readonly IApiRestRespuestaFactory apiRestCreadorRespuesta = new FactoryRespuestasApi();
        public static ObservableCollection<JugadorDetalle> Seguidos { get; set; } = new ObservableCollection<JugadorDetalle>();
        public static ObservableCollection<JugadorDetalle> Seguidores { get; set; } = new ObservableCollection<JugadorDetalle>();

        public VentanaSocial()
        {
            InitializeComponent();
            Estaticas.GuardarMedidasUltimaVentana(this);
            this.DataContext = this;
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            grd_OverlayCarga.Visibility = Visibility.Visible;
            MenuPrincipal menuPrincipal = new MenuPrincipal();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, menuPrincipal);
            grd_OverlayCarga.Visibility = Visibility.Collapsed;
            this.Close();
        }

        private static async void btn_eliminarSeguidor(object sender, RoutedEventArgs e)
        {
            var boton = sender as Button;
            var informacionJugador = boton?.DataContext as JugadorDetalle;
            if (informacionJugador != null)
            {
                VentanaDeConfirmacion ventanaDeConfirmacion = new VentanaDeConfirmacion(Properties.Resources.ConfirmacionEliminarSeguidor, this);
                bool? resultadoConfirmacion = ventanaDeConfirmacion.ShowDialog();
                if (resultadoConfirmacion == true)
                {
                    await EliminarSeguidor(informacionJugador);
                }
            }
        }

        private async Task EliminarSeguidor(JugadorDetalle informacionJugador)
        {
            grd_OverlayCarga.Visibility = Visibility.Visible;
            ApiRespuestaBase respuestaBase = await ServicioSeguidor.EliminarJugadorSeguido(informacionJugador.idUsuario, UsuarioSingleton.Instancia.idJugador, apiRestCreadorRespuesta);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasConDatosODiferentesAlCodigoDeExito(respuestaBase);
            if (!esRespuestaCritica)
            {
                ActualizarListaDeSeguidores(respuestaBase, informacionJugador.idUsuario);
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                grd_OverlayCarga.Visibility = Visibility.Collapsed;
                this.Close();
            }
        }

        private void ActualizarListaDeSeguidores(ApiRespuestaBase apiRespuestaBase, int idUsuario)
        {
            if (apiRespuestaBase.estado == Constantes.CodigoExito)
            {
                var informacionJugador = Seguidores.FirstOrDefault(seguidor => seguidor.idUsuario == idUsuario);
                if (informacionJugador != null)
                {
                    Seguidores.Remove(informacionJugador);
                }
            }
            grd_OverlayCarga.Visibility = Visibility.Collapsed;
        }

        private async void btn_eliminarSeguido(object sender, RoutedEventArgs e)
        {
            var boton = sender as Button;
            var informacionJugador = boton?.DataContext as JugadorDetalle;
            if (informacionJugador != null)
            {
                VentanaDeConfirmacion ventanaDeConfirmacion = new VentanaDeConfirmacion(Properties.Resources.ConfirmaciónEliminacionSeguido, this);
                bool? resultadoConfirmacion = ventanaDeConfirmacion.ShowDialog();
                if (resultadoConfirmacion == true)
                {
                    await EliminarSeguido(informacionJugador);
                }
            }
        }

        private async Task EliminarSeguido(JugadorDetalle informacionJugador)
        {
            grd_OverlayCarga.Visibility = Visibility.Visible;
            ApiRespuestaBase respuestaBase = await ServicioSeguidor.EliminarJugadorSeguido(UsuarioSingleton.Instancia.idJugador, informacionJugador.idUsuario, apiRestCreadorRespuesta);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasConDatosODiferentesAlCodigoDeExito(respuestaBase);
            if (!esRespuestaCritica)
            {
                ActualizarListaDeSeguidos(respuestaBase, informacionJugador.idUsuario);
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                grd_OverlayCarga.Visibility = Visibility.Collapsed;
                this.Close();
            }
        }


        private void ActualizarListaDeSeguidos(ApiRespuestaBase apiRespuestaBase, int idUsuario)
        {
            if (apiRespuestaBase.estado == Constantes.CodigoExito)
            {
                var informacionJugador = Seguidos.FirstOrDefault(seguidor => seguidor.idUsuario == idUsuario);
                if (informacionJugador != null)
                {
                    Seguidos.Remove(informacionJugador);
                }
            }
            grd_OverlayCarga.Visibility = Visibility.Collapsed;
        }

        private async void Btn_MostrarSeguidores_Click(object sender, RoutedEventArgs e)
        {
            ic_Seguidores.Visibility = Visibility.Visible;
            ic_Seguidos.Visibility = Visibility.Collapsed;
            grd_OverlayCarga.Visibility = Visibility.Visible;
            ApiSeguidoresRespuesta jugadoresSeguidoresRespuesta = await ServicioSeguidor.ObtenerJugadoresSeguidores(UsuarioSingleton.Instancia.idJugador, apiRestCreadorRespuesta);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasConDatosODiferentesAlCodigoDeExito(jugadoresSeguidoresRespuesta);
            if(!esRespuestaCritica)
            {
                if (jugadoresSeguidoresRespuesta.estado == Constantes.CodigoExito)
                {
                    await CargarJugadoresSeguidores(jugadoresSeguidoresRespuesta.jugadoresSeguidores!);
                }
                txbl_VistaActual.Text = Properties.Resources.JugadoresSeguidores;
                grd_OverlayCarga.Visibility = Visibility.Collapsed;
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                grd_OverlayCarga.Visibility = Visibility.Collapsed;
                this.Close();
            }
        }

        private async void Btn_MostrarSeguidos_Click(object sender, RoutedEventArgs e)
        {
            ic_Seguidores.Visibility = Visibility.Collapsed;
            ic_Seguidos.Visibility = Visibility.Visible;
            grd_OverlayCarga.Visibility = Visibility.Visible;
            ApiSeguidosRespuesta jugadoresSeguidosRespuesta = await ServicioSeguidor.ObtenerJugadoresSeguidos(UsuarioSingleton.Instancia.idJugador, apiRestCreadorRespuesta);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasConDatosODiferentesAlCodigoDeExito(jugadoresSeguidosRespuesta);
            if (!esRespuestaCritica)
            {
                if(jugadoresSeguidosRespuesta.estado == Constantes.CodigoExito)
                {
                    await CargarJugadoresSeguidos(jugadoresSeguidosRespuesta.jugadoresSeguidos!);
                }
                txbl_VistaActual.Text = Properties.Resources.JugadoresSeguidos;
                grd_OverlayCarga.Visibility = Visibility.Collapsed;
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                grd_OverlayCarga.Visibility = Visibility.Collapsed;
                this.Close();
            }
        }

        public async Task CargarJugadoresSeguidos(List<Seguido> jugadoresSeguidos)
        {
            foreach(var jugador in jugadoresSeguidos)
            {
                bool esSeguidoRepetido = Seguidos.Any(jugadorSeguidoNuevo => jugadorSeguidoNuevo.idUsuario == jugador.idJugador);
                if (!esSeguidoRepetido)
                {
                    JugadorDetalle informacionJugador = new JugadorDetalle()
                    {
                        idUsuario = jugador.idJugador,
                        nombre = jugador.nombreDeUsuario,
                        foto = await CargarFotoDePerfilUsuario(jugador.foto!)
                    };
                    Seguidos.Add(informacionJugador);
                }
            }
            ic_Seguidos.ItemsSource = Seguidos;
        }

        public async Task CargarJugadoresSeguidores(List<Seguidor> jugadoresSeguidores)
        {
            foreach(var jugador in jugadoresSeguidores)
            {
                bool esSeguidorRepetido = Seguidores.Any(jugadorEncontrado => jugadorEncontrado.idUsuario == jugador.idJugador);
                if (!esSeguidorRepetido)
                {
                    JugadorDetalle informacionJugador = new JugadorDetalle()
                    {
                        idUsuario = jugador.idJugador,
                        nombre = jugador.nombreDeUsuario,
                        foto = await CargarFotoDePerfilUsuario(jugador.foto!)
                    };
                    Seguidores.Add(informacionJugador);
                }
            }
            ic_Seguidores.ItemsSource = Seguidores;
        }

        private async Task<byte[]> CargarFotoDePerfilUsuario(string rutaFoto)
        {
            byte[] fotoEncontrada = FotoPorDefecto.ObtenerFotoDePerfilPorDefecto();
            RespuestaGRPC respuestaGRPC = await ServicioFotoDePerfil.ObtenerFotoJugador(rutaFoto);
            if (respuestaGRPC.codigo == Constantes.CodigoExito)
            {
                fotoEncontrada = respuestaGRPC.datosBinario!;
            }
            return fotoEncontrada;
        }

    }

    public class JugadorDetalle
    {
        
        public string? rutaFoto {  get; set; }

        public int idUsuario { get; set; }

        public byte[]? foto {  get; set; }

        public string? nombre { get; set; }

    }

}
