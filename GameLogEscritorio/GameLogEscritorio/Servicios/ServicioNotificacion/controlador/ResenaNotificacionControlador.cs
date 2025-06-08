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

        public async Task DeterminarTipoNotificacion(MensajeResenaNotificacion notificacion)
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
                    if (!notificacion.mensaje!.Contains(UsuarioSingleton.Instancia.nombreDeUsuario!))
                    {
                        ActualizarContadorMeGustaReseña(false, notificacion.idResena!);
                    }
                    break;
                case Constantes.AccionResenaQuitarMeGusta:
                    break;
                case Constantes.AccionResenaInsertarResena:
                    break;
            }
        }

        private void ActualizarContadorMeGustaReseña(bool asignarLike, int idReseña)
        {
            var ventana = Application.Current.Windows.OfType<VentanaReseñasJugadores>().FirstOrDefault();
            if (ventana != null && ventana.IsVisible)
            {
                var reseña = VentanaReseñasJugadores.Reseñas.Where(reseña => reseña.idResenia == idReseña).FirstOrDefault();
                if (reseña != null)
                {
                    if (asignarLike)
                    {
                        reseña.totalDeMeGustaReseña++;
                        reseña.existeMeGustaReseña = true;
                    }
                    else
                    {
                        reseña.totalDeMeGustaReseña--;
                        reseña.existeMeGustaReseña = false;
                    }
                }
            }
        }

    }
}
