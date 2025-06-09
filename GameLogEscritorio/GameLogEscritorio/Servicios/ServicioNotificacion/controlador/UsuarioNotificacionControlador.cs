using GameLogEscritorio.Servicios.GameLogAPIGRPC.Respuesta;
using GameLogEscritorio.Servicios.GameLogAPIGRPC.Servicio;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Juegos;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Social;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Servicios.ServicioNotificacion.Mensaje;
using GameLogEscritorio.Utilidades;
using GameLogEscritorio.Ventanas;
using System.Collections.ObjectModel;
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
                    ActualizarVentanaSeguidores(notificacion);
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
                var ventana = Application.Current.Windows.OfType<VentanaSocial>().FirstOrDefault(ventana => ventana.IsVisible || ventana.IsLoaded);
                if (ventana != null)
                {
                    if (notificacion.idJugadorSeguido == UsuarioSingleton.Instancia.idJugador)
                    {
                        var informacionJugador = VentanaSocial.Seguidores.Where(jugador => jugador.idUsuario == notificacion.idJugadorSeguidor).FirstOrDefault();
                        if (informacionJugador != null)
                        {
                            VentanaSocial.Seguidores.Remove(informacionJugador);
                        }
                    }
                    else if (notificacion.idJugadorSeguidor == UsuarioSingleton.Instancia.idJugador)
                    {
                        var informacionJugador = VentanaSocial.Seguidos.Where(jugador => jugador.idUsuario == notificacion.idJugadorSeguido).FirstOrDefault();
                        if (informacionJugador != null)
                        {
                            VentanaSocial.Seguidos.Remove(informacionJugador);
                        }
                    }
                }
            });
        }

        private void ActualizarVentanaSeguidores(MensajeNotificacion notificacion)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var ventana = Application.Current.Windows.OfType<VentanaSocial>().FirstOrDefault(ventana => ventana.IsVisible || ventana.IsLoaded);
                if(ventana != null)
                {
                    Task.Run(async () =>
                    {
                        ApiSeguidoresRespuesta jugadoresSeguidoresRespuesta = await ServicioSeguidor.ObtenerJugadoresSeguidores(UsuarioSingleton.Instancia.idJugador, apiRespuestasRestFactory);
                        bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasConDatosODiferentesAlCodigoDeExito(jugadoresSeguidoresRespuesta);
                        if (!esRespuestaCritica)
                        {
                            if (jugadoresSeguidoresRespuesta.estado == Constantes.CodigoExito)
                            {
                                await CargarJugadoresSeguidores(jugadoresSeguidoresRespuesta.jugadoresSeguidores!,ventana);
                            }
                        }
                        else
                        {
                            await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                            ventana.Close();
                        }
                    });
                }
            });
        }

        public async Task CargarJugadoresSeguidores(List<Seguidor> jugadoresSeguidores,VentanaSocial ventana)
        {
            foreach (var jugador in jugadoresSeguidores)
            {
                bool yaExiste = VentanaSocial.Seguidores.Any(jugadorEncontrado => jugadorEncontrado.idUsuario == jugador.idJugador);
                if (!yaExiste)
                {
                    JugadorDetalle informacionJugador = new JugadorDetalle()
                    {
                        idUsuario = jugador.idJugador,
                        nombre = jugador.nombreDeUsuario,
                        foto = await CargarFotoDePerfilUsuario(jugador.foto!)
                    };
                    VentanaSocial.Seguidores.Add(informacionJugador);
                }
            }
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
