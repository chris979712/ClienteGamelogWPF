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
    
    public partial class VentanaMisSeguidores : Window
    {

        private static readonly IApiRestRespuestaFactory apiRestCreadorRespuesta = new FactoryRespuestasAPI();
        public static ObservableCollection<JugadorDetalle> Seguidos { get; set; } = new ObservableCollection<JugadorDetalle>();
        public static ObservableCollection<JugadorDetalle> Seguidores { get; set; } = new ObservableCollection<JugadorDetalle>();

        public VentanaMisSeguidores()
        {
            InitializeComponent();
            Estaticas.GuardarMedidasUltimaVentana(this);
            this.DataContext = this;
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            MenuPrincipal menuPrincipal = new MenuPrincipal();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, menuPrincipal);
            this.Close();
        }

        private async void btn_eliminarSeguidor(object sender, RoutedEventArgs e)
        {
            var boton = sender as Button;
            var informacionJugador = boton?.DataContext as JugadorDetalle;
            if (informacionJugador != null)
            {
                ApiRespuestaBase respuestaBase = await ServicioSeguidor.EliminarJugadorSeguido(informacionJugador.idUsuario, UsuarioSingleton.Instancia.idJugador, apiRestCreadorRespuesta);
                bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasConDatosODiferentesAlCodigoDeExito(respuestaBase);
                if (!esRespuestaCritica)
                {
                    ActualizarListaDeSeguidores(respuestaBase, informacionJugador.idUsuario);
                }
                else
                {
                    await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                    this.Close();
                }
            }
        }

        private void ActualizarListaDeSeguidores(ApiRespuestaBase apiRespuestaBase, int idUsuario)
        {
            if (apiRespuestaBase.estado == Constantes.CodigoExito)
            {
                var informacionJugador = Seguidores.Where(seguidor => seguidor.idUsuario == idUsuario).FirstOrDefault();
                if (informacionJugador != null)
                {
                    Seguidores.Remove(informacionJugador);
                }
            }
        }

        private async void btn_eliminarSeguido(object sender, RoutedEventArgs e)
        {
            var boton = sender as Button;
            var informacionJugador = boton?.DataContext as JugadorDetalle;
            if (informacionJugador != null)
            {
                ApiRespuestaBase respuestaBase = await ServicioSeguidor.EliminarJugadorSeguido(UsuarioSingleton.Instancia.idJugador,informacionJugador.idUsuario, apiRestCreadorRespuesta);
                bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasConDatosODiferentesAlCodigoDeExito(respuestaBase);
                if (!esRespuestaCritica)
                {
                    ActualizarListaDeSeguidos(respuestaBase, informacionJugador.idUsuario);
                }
                else
                {
                    await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                    this.Close();
                }
            }
        }

        private void ActualizarListaDeSeguidos(ApiRespuestaBase apiRespuestaBase, int idUsuario)
        {
            if (apiRespuestaBase.estado == Constantes.CodigoExito)
            {
                var informacionJugador = Seguidos.Where(seguidor => seguidor.idUsuario == idUsuario).FirstOrDefault();
                if (informacionJugador != null)
                {
                    Seguidos.Remove(informacionJugador);
                }
            }
        }


        private async void Btn_MostrarSeguidores_Click(object sender, RoutedEventArgs e)
        {
            itemsControlSeguidores.Visibility = Visibility.Visible;
            itemsControlSeguidos.Visibility = Visibility.Collapsed;
            ApiSeguidoresRespuesta jugadoresSeguidoresRespuesta = await ServicioSeguidor.ObtenerJugadoresSeguidores(UsuarioSingleton.Instancia.idJugador, apiRestCreadorRespuesta);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasConDatosODiferentesAlCodigoDeExito(jugadoresSeguidoresRespuesta);
            if(!esRespuestaCritica)
            {
                if (jugadoresSeguidoresRespuesta.estado == Constantes.CodigoExito)
                {
                    await CargarJugadoresSeguidores(jugadoresSeguidoresRespuesta.jugadoresSeguidores!);
                }
                txb_VistaActual.Text = Properties.Resources.JugadoresSeguidores;
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                this.Close();
            }
        }

        private async void Btn_MostrarSeguidos_Click(object sender, RoutedEventArgs e)
        {
            itemsControlSeguidores.Visibility = Visibility.Collapsed;
            itemsControlSeguidos.Visibility = Visibility.Visible;
            ApiSeguidosRespuesta jugadoresSeguidosRespuesta = await ServicioSeguidor.ObtenerJugadoresSeguidos(UsuarioSingleton.Instancia.idJugador, apiRestCreadorRespuesta);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasConDatosODiferentesAlCodigoDeExito(jugadoresSeguidosRespuesta);
            if (!esRespuestaCritica)
            {
                if(jugadoresSeguidosRespuesta.estado == Constantes.CodigoExito)
                {
                    await CargarJugadoresSeguidos(jugadoresSeguidosRespuesta.jugadoresSeguidos!);
                }
                txb_VistaActual.Text = Properties.Resources.JugadoresSeguidos;
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                this.Close();
            }
        }

        public async Task CargarJugadoresSeguidos(List<Seguido> jugadoresSeguidos)
        {
            Seguidos.Clear();
            foreach(var jugador in jugadoresSeguidos)
            {
                JugadorDetalle informacionJugador = new JugadorDetalle()
                {
                    idUsuario = jugador.idJugador,
                    nombre = jugador.nombreDeUsuario,
                    foto = await CargarFotoDePerfilUsuario(jugador.foto!)
                };
                Seguidos.Add(informacionJugador);
            }
            itemsControlSeguidos.ItemsSource = Seguidos;
        }

        public async Task CargarJugadoresSeguidores(List<Seguidor> jugadoresSeguidores)
        {
            Seguidores.Clear();
            foreach(var jugador in jugadoresSeguidores)
            {
                JugadorDetalle informacionJugador = new JugadorDetalle()
                {
                    idUsuario = jugador.idJugador,
                    nombre = jugador.nombreDeUsuario,
                    foto = await CargarFotoDePerfilUsuario(jugador.foto!)
                };
                Seguidores.Add(informacionJugador);
            }
            itemsControlSeguidores.ItemsSource = Seguidores;
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
