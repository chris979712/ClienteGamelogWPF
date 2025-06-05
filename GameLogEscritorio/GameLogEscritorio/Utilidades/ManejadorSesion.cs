using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Juegos;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Ventanas;
using System.Collections.ObjectModel;


namespace GameLogEscritorio.Utilidades
{
    public static class ManejadorSesion
    {

        private static readonly IApiRestRespuestaFactory apiRestCreadorRespuesta = new FactoryRespuestasAPI();

        public static async Task<bool> CerrarSesionForzadaDeUsuario()
        {
            await ServicioLogin.CerrarSesion(UsuarioSingleton.Instancia.correo!, apiRestCreadorRespuesta);
            UsuarioSingleton.Instancia.CerrarSesion();
            Estaticas.juegosPendientes = new ObservableCollection<JuegoCompleto>();
            Estaticas.juegosFavoritos = new List<Juego>();
            Estaticas.idJugadoresSeguido = new List<int>();
            SesionToken.CerrarSesion();
            return true;
        }

        public static async Task<bool> RegresarInicioDeSesionUsuario()
        {
            ApiRespuestaBase respuesta = await ServicioLogin.CerrarSesion(UsuarioSingleton.Instancia.correo!,apiRestCreadorRespuesta);
            UsuarioSingleton.Instancia.CerrarSesion();
            SesionToken.CerrarSesion();
            Estaticas.juegosPendientes = new ObservableCollection<JuegoCompleto>();
            Estaticas.juegosFavoritos = new List<Juego>();
            Estaticas.idJugadoresSeguido = new List<int>(); ;
            VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoExito, respuesta.mensaje!, respuesta.estado);
            AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaEmergente);
            VentanaInicioDeSesion ventanaInicioDeSesion = new VentanaInicioDeSesion();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaInicioDeSesion);
            return true;
        }

        public static async Task<bool> RegresarInicioDeSesionSinAcceso(string mensajeSinAcceso)
        {
            ApiRespuestaBase respuesta = await ServicioLogin.CerrarSesion(UsuarioSingleton.Instancia.correo!, apiRestCreadorRespuesta);
            UsuarioSingleton.Instancia.CerrarSesion();
            SesionToken.CerrarSesion();
            Estaticas.juegosPendientes = new ObservableCollection<JuegoCompleto>();
            Estaticas.juegosFavoritos = new List<Juego>();
            Estaticas.idJugadoresSeguido = new List<int>();
            VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, mensajeSinAcceso, respuesta.estado);
            AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaEmergente);
            VentanaInicioDeSesion ventanaInicioDeSesion = new VentanaInicioDeSesion();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaInicioDeSesion);
            return true;
        }

        public static async Task<bool> RegresarInicioDeSesionSinAcceso()
        {
            ApiRespuestaBase respuesta = await ServicioLogin.CerrarSesion(UsuarioSingleton.Instancia.correo!, apiRestCreadorRespuesta);
            UsuarioSingleton.Instancia.CerrarSesion();
            SesionToken.CerrarSesion();
            Estaticas.juegosPendientes = new ObservableCollection<JuegoCompleto>();
            Estaticas.juegosFavoritos = new List<Juego>();
            Estaticas.idJugadoresSeguido = new List<int>();
            VentanaInicioDeSesion ventanaInicioDeSesion = new VentanaInicioDeSesion();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaInicioDeSesion);
            return true;
        }
    }

}
