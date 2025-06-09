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
                    if (!notificacion.mensaje!.Contains(UsuarioSingleton.Instancia.nombreDeUsuario!))
                    {
                        MostrarNotificacion(notificacion.mensaje!);
                    }
                    break;
                case Constantes.AccionSocialAgregarSeguidor:
                    MostrarNotificacion(notificacion.mensaje!);
                    //TODO
                    break;
                case Constantes.AccionSocialEliminarSeguidor:
                    ActualizarEliminacionListaDeSeguidosSeguidores(notificacion);
                    ActualizarVentanaDescripcionPerfil(notificacion);
                    break;
                case Constantes.AccionSocialBanearUsuario:
                    MostrarAdvertencia(notificacion.mensaje!);
                    await CerrarSesionUsuarioBaneado();
                    break;
            }
        }

        private void ActualizarEliminacionListaDeSeguidosSeguidores(MensajeNotificacion notificacion)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var ventana = Application.Current.Windows.OfType<VentanaMisSeguidores>().FirstOrDefault(ventana => ventana.IsVisible || ventana.IsLoaded);
                if (ventana != null)
                {
                    if (notificacion.idJugadorSeguido == UsuarioSingleton.Instancia.idJugador)
                    {
                        var informacionJugador = VentanaMisSeguidores.Seguidores.Where(jugador => jugador.idUsuario == notificacion.idJugadorSeguidor).FirstOrDefault();
                        if (informacionJugador != null)
                        {
                            VentanaMisSeguidores.Seguidores.Remove(informacionJugador);
                        }
                    }
                    else if (notificacion.idJugadorSeguidor == UsuarioSingleton.Instancia.idJugador)
                    {
                        var informacionJugador = VentanaMisSeguidores.Seguidos.Where(jugador => jugador.idUsuario == notificacion.idJugadorSeguido).FirstOrDefault();
                        if (informacionJugador != null)
                        {
                            VentanaMisSeguidores.Seguidos.Remove(informacionJugador);
                        }
                    }
                }
            });
        }

        private void ActualizarVentanaDescripcionPerfil(MensajeNotificacion notificacion)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var ventana = Application.Current.Windows.OfType<VentanaPerfilJugador>().FirstOrDefault(ventana => ventana.IsVisible || ventana.IsLoaded);
                if(ventana != null)
                {
                    if(ventana.perfilJugador.idJugador == notificacion.idJugadorSeguido)
                    {
                        ventana.btn_Seguir.Visibility = Visibility.Visible;
                        ventana.btn_DejarDeSeguir.Visibility=Visibility.Collapsed;
                    }
                }
            });
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

        public async Task CerrarSesionUsuarioBaneado()
        {
            var respuesta = await ServicioLogin.CerrarSesion(UsuarioSingleton.Instancia.correo!, apiRespuestasRestFactory);
            UsuarioSingleton.Instancia.CerrarSesion();
            SesionToken.CerrarSesion();
            Estaticas.juegosPendientes = new ObservableCollection<JuegoCompleto>();
            Estaticas.juegosFavoritos = new List<Juego>();
            Estaticas.idJugadoresSeguido = new List<int>();
            await ServicioNotificacion.Desconectar();
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var ventanaInicioDeSesion = new VentanaInicioDeSesion();
                AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaInicioDeSesion);
                foreach (Window ventana in Application.Current.Windows)
                {
                    if (ventana is not VentanaInicioDeSesion)
                    {
                        ventana.Close();
                    }
                }
                Application.Current.MainWindow = ventanaInicioDeSesion;
            });
        }

    }
}
