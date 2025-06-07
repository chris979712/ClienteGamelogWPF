using GameLogEscritorio.Servicios.ServicioNotificacion.Mensaje;
using GameLogEscritorio.Utilidades;
using GameLogEscritorio.Ventanas;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocketIO.Core;
using SocketIOClient;
using SocketIOClient.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameLogEscritorio.Servicios.ServicioNotificacion
{
    public static class ServicioNotificacion
    {

        private static SocketIOClient.SocketIO? _socket;

        public static async Task Conectar()
        {
            try
            {
                string socketUrl = Properties.Resources.SocketURL;
                string usuarioHasheado = Encriptador.hasheoA256(Properties.Resources.UsuarioServicioNotificacion);
                string contraseniaHasheada = Encriptador.hasheoA256(Properties.Resources.ContrasenaServicioNotificacion);
                var options = new SocketIOOptions
                {
                    Query = new Dictionary<string, string>
                    {
                        {"usuario",usuarioHasheado},
                        {"contrasenia",contraseniaHasheada},
                        {"b64", "1"}
                    },
                    Transport = TransportProtocol.WebSocket,
                    Reconnection = true,
                    ReconnectionAttempts = 5,
                    ReconnectionDelay = 1000,
                    EIO = EngineIO.V3,
                    ConnectionTimeout = TimeSpan.FromSeconds(125)
                };
                _socket = new SocketIOClient.SocketIO(socketUrl, options);
                if (ConectarAEventosIniciales(_socket))
                {
                   await _socket.ConnectAsync();
                }
            }
            catch(HttpRequestException excepcion)
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente(Properties.Resources.TituloSocketExcepcion,Properties.Resources.SocketExcepcion,Constantes.CodigoErrorServidor);
                AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaEmergente);
            }
            catch(Exception excepcion)
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente(Properties.Resources.TituloExcepcion, Properties.Resources.Excepcion, Constantes.CodigoErrorServidor);
                AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaEmergente);
            }
        }

        private static bool ConectarAEventosIniciales(SocketIOClient.SocketIO socket)
        {
            bool resultadoConexionAEventos = false;
            try
            {
                socket.OnConnected += async (sender, e) => {
                    await _socket!.EmitAsync(Properties.Resources.EventoSuscribirNotificacionJugador, UsuarioSingleton.Instancia.idJugador);
                };

                socket.OnError += (sender, e) =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, Properties.Resources.SocketExcepcion, Constantes.CodigoErrorServidor);
                        AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaEmergente);
                    });
                };

                socket.OnReconnectFailed += (sender, e) =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, Properties.Resources.SocketExcepcion, Constantes.CodigoErrorServidor);
                        AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaEmergente);
                    });
                };

                socket.On(Properties.Resources.EventoNotificacionJugador, respuesta =>
                {
                    var jArray = JArray.Parse(respuesta.ToString());
                    var mensaje = jArray.First().ToObject<MensajeNotificacion>();
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoInformacion, mensaje!.mensaje!, Constantes.CodigoExito);
                        AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaEmergente);
                    });
                });

                socket.On(Properties.Resources.EventoNotificacionResenas, respuesta =>
                {

                });

                resultadoConexionAEventos = true;
            }
            catch (HttpRequestException excepcion)
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente(Properties.Resources.TituloSocketExcepcion, Properties.Resources.SocketExcepcion, Constantes.CodigoErrorServidor);
                AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaEmergente);
            }
            catch (Exception excepcion)
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente(Properties.Resources.TituloExcepcion, Properties.Resources.Excepcion, Constantes.CodigoErrorServidor);
                AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaEmergente);
            }
            return resultadoConexionAEventos;
        }

        public static async Task SuscribirseCanalInteraccionReseñasDeJuego(int idJuego)
        {
            if(_socket !=null && _socket!.Connected)
            {
                try
                {
                    await _socket!.EmitAsync(Properties.Resources.EventoSuscribirInteraccionReseña, idJuego);
                }
                catch (HttpRequestException excepcion)
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Properties.Resources.TituloSocketExcepcion, Properties.Resources.SocketExcepcion, Constantes.CodigoErrorServidor);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaEmergente);
                }
                catch (Exception excepcion)
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Properties.Resources.TituloExcepcion, Properties.Resources.Excepcion, Constantes.CodigoErrorServidor);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaEmergente);
                }
            }
        }

        public static async Task DesuscribirseCanalInteraccionReseñasDeJuego(int idJuego)
        {
            if(_socket != null && _socket!.Connected)
            {
                try
                {
                    await _socket!.EmitAsync(Properties.Resources.EventoDesuscribirInteraccionResena, idJuego);
                }
                catch (HttpRequestException excepcion)
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Properties.Resources.TituloSocketExcepcion, Properties.Resources.SocketExcepcion, Constantes.CodigoErrorServidor);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaEmergente);
                }
                catch (Exception excepcion)
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Properties.Resources.TituloExcepcion, Properties.Resources.Excepcion, Constantes.CodigoErrorServidor);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaEmergente);
                }
            }
        }

        public static async Task Desconectar()
        {
            try
            {
                if (_socket != null && _socket!.Connected)
                {
                    await _socket!.EmitAsync(Properties.Resources.EventoDesuscribirNotificacionJugador, UsuarioSingleton.Instancia.idJugador);
                    _socket.Options.Reconnection = false;
                    await _socket.DisconnectAsync();
                    _socket.Dispose();
                }
            }
            catch (HttpRequestException excepcion)
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente(Properties.Resources.TituloSocketExcepcion, Properties.Resources.SocketExcepcion, Constantes.CodigoErrorServidor);
                AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaEmergente);
            }
            catch (Exception)
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente(Properties.Resources.TituloExcepcion, Properties.Resources.Excepcion, Constantes.CodigoErrorServidor);
                AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaEmergente);
            }

        }
    }
}
