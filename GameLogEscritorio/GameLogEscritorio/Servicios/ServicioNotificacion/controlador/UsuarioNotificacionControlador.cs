using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Juegos;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Servicios.ServicioNotificacion.Mensaje;
using GameLogEscritorio.Utilidades;
using GameLogEscritorio.Ventanas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameLogEscritorio.Servicios.ServicioNotificacion.controlador
{
    public class UsuarioNotificacionControlador
    {
        private static readonly IApiRestRespuestaFactory apiRespuestasRestFactory = new FactoryRespuestasAPI();

        public UsuarioNotificacionControlador() 
        {
           
        }

        public async Task DeterminarTipoNotificacion(MensajeNotificacion notificacion)
        {
            switch (notificacion.accion!)
            {
                case Constantes.AccionSocialDarMeGusta:
                    MostrarNotificacion(notificacion.mensaje!);
                    break;
                case Constantes.AccionSocialAgregarSeguidor:
                    MostrarNotificacion(notificacion.mensaje!);
                    //TODO
                    break;
                case Constantes.AccionSocialEliminarSeguidor:
                    //TODO
                    break;
                case Constantes.AccionSocialBanearUsuario:
                    MostrarAdvertencia(notificacion.mensaje!);
                    await CerrarSesionUsuarioBaneado();
                    break;
            }
        }

        private void MostrarNotificacion(string mensaje)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                VentanaEmergenteNotificacion ventanaEmergenteNotificacion = new VentanaEmergenteNotificacion(mensaje);
                ventanaEmergenteNotificacion.Show();
            });
        }

        private void MostrarAdvertencia(string mensaje)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoAdvertencia, mensaje, Constantes.CodigoErrorAcceso);
                AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaEmergente);
            });
        }

        private async Task CerrarSesionUsuarioBaneado()
        {
            var respuesta = await ServicioLogin.CerrarSesion(UsuarioSingleton.Instancia.correo!,apiRespuestasRestFactory);
            UsuarioSingleton.Instancia.CerrarSesion();
            SesionToken.CerrarSesion();
            Estaticas.juegosPendientes = new ObservableCollection<JuegoCompleto>();
            Estaticas.juegosFavoritos = new List<Juego>();
            Estaticas.idJugadoresSeguido = new List<int>();
            await ServicioNotificacion.Desconectar();
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var ventanaInicioDeSesion = new VentanaInicioDeSesion();
                AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(Estaticas.ultimoTopVentana,Estaticas.ultimoLeftVentana,Estaticas.ultimoWidthVentana,Estaticas.ultimoHeightVentana,ventanaInicioDeSesion);
                Application.Current.MainWindow?.Close();
                Application.Current.MainWindow = ventanaInicioDeSesion;
            });
        }


    }
}
