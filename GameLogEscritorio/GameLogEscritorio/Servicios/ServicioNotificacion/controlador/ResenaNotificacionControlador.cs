using GameLogEscritorio.Servicios.GameLogAPIGRPC.Respuesta;
using GameLogEscritorio.Servicios.GameLogAPIGRPC.Servicio;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Reseñas;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Servicios.ServicioNotificacion.Mensaje;
using GameLogEscritorio.Utilidades;
using GameLogEscritorio.Ventanas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameLogEscritorio.Servicios.ServicioNotificacion.controlador
{
    public class ResenaNotificacionControlador
    {

        private static readonly IApiRestRespuestaFactory apiRespuestasRestFactory = new FactoryRespuestasAPI();

        public ResenaNotificacionControlador() 
        {

        }

        public void DeterminarTipoNotificacion(MensajeResenaNotificacion notificacion)
        {
            switch (notificacion.accion!)
            {
                case Constantes.AccionResenaDarMeGusta:
                    if (!notificacion.mensaje!.Contains(UsuarioSingleton.Instancia.nombreDeUsuario!))
                    {
                        ActualizarContadorMeGustaReseña(true, notificacion.idResena!);
                    }
                    break;
                case Constantes.AccionResenaEliminarResena:
                    ActualizarVentanaEliminarReseña(notificacion.idResena!);
                    break;
                case Constantes.AccionResenaQuitarMeGusta:
                    if (!notificacion.mensaje!.Contains(UsuarioSingleton.Instancia.nombreDeUsuario!))
                    {
                        ActualizarContadorMeGustaReseña(false, notificacion.idResena!);
                    }
                    break;
                case Constantes.AccionResenaInsertarResena:
                    MostrarNotificacion(notificacion.mensaje!);
                    MostrarNuevasReseñas();
                    break;
            }
        }

        private void ActualizarContadorMeGustaReseña(bool asignarLike, int idReseña)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var ventana = Application.Current.Windows.OfType<VentanaReseñasJugadores>().FirstOrDefault(ventana => ventana.IsVisible || ventana.IsLoaded);
                if (ventana != null && ventana.IsVisible)
                {
                    var reseña = VentanaReseñasJugadores.Reseñas.Where(reseña => reseña.idResenia == idReseña).FirstOrDefault();
                    if (reseña != null)
                    {
                        if (asignarLike)
                        {
                            reseña.totalDeMeGustaReseña++;
                        }
                        else
                        {
                            reseña.totalDeMeGustaReseña--;
                        }
                    }
                }
            });
        }

        private void ActualizarVentanaEliminarReseña(int idReseña)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var ventana = Application.Current.Windows.OfType<VentanaReseñasJugadores>().FirstOrDefault(ventana => ventana.IsVisible || ventana.IsLoaded);
                if(ventana != null && ventana.IsVisible)
                {
                    var reseña = VentanaReseñasJugadores.Reseñas.Where(reseña => reseña.idResenia == idReseña).FirstOrDefault();
                    if(reseña != null)
                    {
                        VentanaReseñasJugadores.Reseñas.Remove(reseña);
                    }
                }
            });
        }

        private void MostrarNuevasReseñas()
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                var ventana = Application.Current.Windows.OfType<VentanaReseñasJugadores>().FirstOrDefault(ventana => ventana.IsVisible || ventana.IsLoaded);
                if(ventana != null && ventana.IsVisible)
                {
                    int idJuegoABuscar = ventana._modeloJuego.id;
                    ApiReseñaJugadoresRespuesta respuestaReseñas = await ServicioReseña.ObtenerReseñasDeUnJuego(idJuegoABuscar, UsuarioSingleton.Instancia.idJugador, apiRespuestasRestFactory);
                    bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasNotificacionDespachador(respuestaReseñas);
                    if (!esRespuestaCritica)
                    {
                        if(respuestaReseñas.estado == Constantes.CodigoExito)
                        {
                            MostrarReseñasCorrespondientes(ventana, respuestaReseñas.reseñaJugadores!);
                        }
                    }
                    else
                    {
                        await ManejadorSesion.RegresarInicioDeSesionDesdeDespachador();
                        Application.Current.Dispatcher.Invoke(() => ventana.Close());
                    }
                }
            });
        }

        private async void MostrarReseñasCorrespondientes(VentanaReseñasJugadores ventanaReseñas, List<ReseñaJugadores> reseñasObtenidas)
        {
            bool seVisualizanReseñasGlobales = VentanaReseñasJugadores.visualizacionDeTodasLasReseñas;
            if (seVisualizanReseñasGlobales)
            {
                await MostrarReseñasGlobales(ventanaReseñas, reseñasObtenidas);
            }
            else
            {
                await MostrarReseñasSeguidos(ventanaReseñas, reseñasObtenidas);
            }
        }

        private async Task MostrarReseñasGlobales(VentanaReseñasJugadores ventana, List<ReseñaJugadores> reseñasObtenidas)
        {
            foreach(var reseña in reseñasObtenidas)
            {
                bool yaExiste = VentanaReseñasJugadores.Reseñas.Any(reseñaObtenida => reseñaObtenida.idResenia == reseña.idResenia);
                if(!yaExiste)
                {
                    var fotoUsuario = await CargarFotoDePerfilUsuario(reseña.foto!);
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        ReseñaCompleta reseñaCompleta = CrearReseñaCompleta(fotoUsuario,reseña);
                        VentanaReseñasJugadores.Reseñas.Add(reseñaCompleta);
                    });
                }
            }
        }

        private async Task MostrarReseñasSeguidos(VentanaReseñasJugadores ventana, List<ReseñaJugadores> reseñasObtenidas)
        {
            foreach(var reseña in reseñasObtenidas)
            {
                bool esReseñaDeSeguido = Estaticas.idJugadoresSeguido.Any( idJugador => idJugador == reseña.idJugador);
                if (esReseñaDeSeguido)
                {
                    bool yaExiste = VentanaReseñasJugadores.Reseñas.Any(reseñaObtenida => reseñaObtenida.idResenia == reseña.idResenia);
                    if (!yaExiste)
                    {
                        var fotoUsuario = await CargarFotoDePerfilUsuario(reseña.foto!);
                        await Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            ReseñaCompleta reseñaCompleta = CrearReseñaCompleta(fotoUsuario, reseña);
                            VentanaReseñasJugadores.Reseñas.Add(reseñaCompleta);
                        });
                    }
                }
            }
        }

        private ReseñaCompleta CrearReseñaCompleta(byte[] fotoUsuarioReseña, ReseñaJugadores reseña)
        {
            return new ReseñaCompleta
            {
                idJuego = reseña.idJuego,
                idJugador = reseña.idJugador,
                idResenia = reseña.idResenia,
                opinion = reseña.opinion,
                fecha = reseña.fecha,
                calificacion = reseña.calificacion,
                totalDeMeGustaReseña = reseña.totalDeMeGusta,
                existeMeGustaReseña = reseña.existeMeGusta,
                nombreDeUsuario = reseña.nombreDeUsuario,
                nombre = reseña.nombre,
                foto = reseña.foto,
                totalDeMeGusta = reseña.totalDeMeGusta,
                existeMeGusta = reseña.existeMeGusta,
                fotoJugador = fotoUsuarioReseña
            };
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

        private void MostrarNotificacion(string mensaje)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                VentanaEmergenteNotificacion ventanaEmergenteNotificacion = new VentanaEmergenteNotificacion(mensaje);
                ventanaEmergenteNotificacion.Show();
            });
        }

    }
}
