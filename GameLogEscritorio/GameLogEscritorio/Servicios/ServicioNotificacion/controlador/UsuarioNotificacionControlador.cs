using GameLogEscritorio.Servicios.GameLogAPIGRPC.Respuesta;
using GameLogEscritorio.Servicios.GameLogAPIGRPC.Servicio;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Juegos;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Notificacion;
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
                    await ActualizarNuevasNotificaciones();
                    if (!notificacion.mensaje!.Contains(UsuarioSingleton.Instancia.nombreDeUsuario!))
                    {
                        MostrarNotificacion(notificacion.mensaje!);
                    }
                    break;
                case Constantes.AccionSocialAgregarSeguidor:
                    await ActualizarNuevasNotificaciones();
                    MostrarNotificacion(notificacion.mensaje!);
                    ActualizarVentanaSeguidores(notificacion);
                    break;
                case Constantes.AccionSocialEliminarSeguidor:
                    await ActualizarNuevasNotificaciones();
                    ActualizarEliminacionListaDeSeguidosSeguidores(notificacion);
                    ActualizarVentanaDescripcionPerfil(notificacion);
                    break;
                case Constantes.AccionSocialBanearUsuario:
                    MostrarAdvertencia(notificacion.mensaje!);
                    await CerrarSesionUsuarioBaneado();
                    break;
            }
        }

        private async Task ActualizarNuevasNotificaciones()
        {
            ApiNotificacionRespuesta notificacionRespuesta = await ServicioNotificaciones.ObtenerNotificacionesDeJugador(UsuarioSingleton.Instancia.idJugador, apiRespuestasRestFactory);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasNotificacionDespachador(notificacionRespuesta);
            if (!esRespuestaCritica)
            {
                List<Notificaciones> notificaciones = notificacionRespuesta.notificaciones ?? new List<Notificaciones>();
                FiltrarNuevasReseñas(notificaciones);
                ActualizarVentanaNotificaciones();
            }
            else
            {
                await Application.Current.Dispatcher.InvokeAsync(async () =>
                {
                    var ventana = Application.Current.Windows.OfType<Window>().FirstOrDefault(ventana => ventana.IsVisible || ventana.IsLoaded);
                    if (ventana != null)
                    {
                        await ManejadorSesion.RegresarInicioDeSesionDesdeDespachador();
                        ventana.Close();
                    }
                });
            }
        }

        private void FiltrarNuevasReseñas(List<Notificaciones> notificaciones)
        {
            Estaticas.notificaciones = EliminarNotificacionesAntiguas(notificaciones);   
            foreach(var notificacionNueva in notificaciones)
            {
                bool existeNotificacion = Estaticas.notificaciones.Any(notificacion => notificacion.Id == notificacionNueva.idNotificacion);
                if (!existeNotificacion)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        NotificacionCompleta nuevaNotificacion = new NotificacionCompleta()
                        {
                            Id = notificacionNueva.idNotificacion,
                            Mensaje = notificacionNueva.mensajeNotificacion,
                            fecha = notificacionNueva.fechaNotificacion
                        };
                        Estaticas.notificaciones.Insert(0, nuevaNotificacion);
                    });
                }
            }
        }

        private ObservableCollection<NotificacionCompleta> EliminarNotificacionesAntiguas(List<Notificaciones> notificaciones)
        {
            var notificacionesAEliminar = new List<NotificacionCompleta>();
            foreach (var notificacion in Estaticas.notificaciones)
            {
                if (notificacion.Id != 0)
                {
                    bool existe = notificaciones?.Any(notificacionAChecar => notificacionAChecar.idNotificacion == notificacion.Id) ?? false;
                    if (!existe)
                    {
                        notificacionesAEliminar.Add(notificacion);
                    }
                }
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var notificacion in notificacionesAEliminar)
                {
                    Estaticas.notificaciones.Remove(notificacion);
                }
            });
            return Estaticas.notificaciones;
        }

        private void ActualizarVentanaNotificaciones()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var ventana = Application.Current.Windows.OfType<MenuPrincipal>().FirstOrDefault(ventana => ventana.IsVisible || ventana.IsLoaded);
                if (ventana != null)
                {
                    ventana.ic_Notificaciones.ItemsSource = Estaticas.notificaciones;
                }
            });
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
                Estaticas.idJugadoresSeguido.Remove(notificacion!.idJugadorSeguido);
            });
        }

        private void ActualizarVentanaSeguidores(MensajeNotificacion notificacion)
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                var ventana = Application.Current.Windows.OfType<VentanaSocial>().FirstOrDefault(ventana => ventana.IsVisible || ventana.IsLoaded);
                if (ventana != null)
                {
                    ApiSeguidoresRespuesta respuesta = await ServicioSeguidor.ObtenerJugadoresSeguidores(UsuarioSingleton.Instancia.idJugador, apiRespuestasRestFactory);
                    bool esCritica = ManejadorRespuestas.ManejarRespuestasNotificacionDespachador(respuesta);
                    if (!esCritica)
                    {
                        if (respuesta.estado == Constantes.CodigoExito)
                        {
                            await CargarJugadoresSeguidores(respuesta.jugadoresSeguidores!, ventana);
                        }
                    }
                    else
                    {
                        await ManejadorSesion.RegresarInicioDeSesionDesdeDespachador();
                        ventana.Close();
                    }
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
                    var foto = await CargarFotoDePerfilUsuario(jugador.foto!);
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        VentanaSocial.Seguidores.Add(new JugadorDetalle
                        {
                            idUsuario = jugador.idJugador,
                            nombre = jugador.nombreDeUsuario,
                            foto = foto
                        });
                        ventana.ic_Seguidores.ItemsSource = VentanaSocial.Seguidores;
                    });
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
