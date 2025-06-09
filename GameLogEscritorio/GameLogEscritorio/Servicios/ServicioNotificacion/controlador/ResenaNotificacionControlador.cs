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
