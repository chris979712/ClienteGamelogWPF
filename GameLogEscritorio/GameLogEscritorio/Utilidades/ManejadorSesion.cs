using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Juegos;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Servicios.ServicioNotificacion;
using GameLogEscritorio.Ventanas;
using System.Collections.ObjectModel;
using System.Windows;


namespace GameLogEscritorio.Utilidades
{
    public static class ManejadorSesion
    {

        private static readonly IApiRestRespuestaFactory apiRestCreadorRespuesta = new FactoryRespuestasAPI();

        public static async Task CerrarSesionForzadaDeUsuario()
        {
            await ServicioLogin.CerrarSesion(UsuarioSingleton.Instancia.correo!, apiRestCreadorRespuesta);
            UsuarioSingleton.Instancia.CerrarSesion();
            await ServicioNotificacion.Desconectar();
            Estaticas.juegosPendientes = new ObservableCollection<JuegoCompleto>();
            Estaticas.juegosFavoritos = new List<Juego>();
            Estaticas.idJugadoresSeguido = new List<int>();
            SesionToken.CerrarSesion();
        }

        public static async Task RegresarInicioDeSesionUsuario()
        {
            ApiRespuestaBase respuesta = await ServicioLogin.CerrarSesion(UsuarioSingleton.Instancia.correo!,apiRestCreadorRespuesta);
            UsuarioSingleton.Instancia.CerrarSesion();
            SesionToken.CerrarSesion();
            Estaticas.juegosPendientes = new ObservableCollection<JuegoCompleto>();
            Estaticas.juegosFavoritos = new List<Juego>();
            Estaticas.idJugadoresSeguido = new List<int>();
            await ServicioNotificacion.Desconectar();
            VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoExito, respuesta.mensaje!, respuesta.estado);
            AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
            VentanaInicioDeSesion ventanaInicioDeSesion = new VentanaInicioDeSesion();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaInicioDeSesion);
        }

        public static async Task RegresarInicioDeSesionDesdeDespachador()
        {
            ApiRespuestaBase respuesta = await ServicioLogin.CerrarSesion(UsuarioSingleton.Instancia.correo!, apiRestCreadorRespuesta);
            UsuarioSingleton.Instancia.CerrarSesion();
            SesionToken.CerrarSesion();
            Estaticas.juegosPendientes = new ObservableCollection<JuegoCompleto>();
            Estaticas.juegosFavoritos = new List<Juego>();
            Estaticas.idJugadoresSeguido = new List<int>();
            await ServicioNotificacion.Desconectar();
            Application.Current.Dispatcher.Invoke(() =>
            {
                VentanaInicioDeSesion ventanaInicioDeSesion = new VentanaInicioDeSesion();
                AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaInicioDeSesion);
            });
        }

        public static async Task RegresarInicioDeSesionSinAcceso()
        {
            ApiRespuestaBase respuesta = await ServicioLogin.CerrarSesion(UsuarioSingleton.Instancia.correo!, apiRestCreadorRespuesta);
            UsuarioSingleton.Instancia.CerrarSesion();
            SesionToken.CerrarSesion();
            Estaticas.juegosPendientes = new ObservableCollection<JuegoCompleto>();
            Estaticas.juegosFavoritos = new List<Juego>();
            Estaticas.idJugadoresSeguido = new List<int>();
            await ServicioNotificacion.Desconectar();
            VentanaInicioDeSesion ventanaInicioDeSesion = new VentanaInicioDeSesion();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaInicioDeSesion);
        }
    }

}
