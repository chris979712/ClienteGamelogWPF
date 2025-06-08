using GameLogEscritorio.Servicios.ServicioNotificacion.Mensaje;
using GameLogEscritorio.Utilidades;
using GameLogEscritorio.Ventanas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameLogEscritorio.Servicios.ServicioNotificacion.controlador
{
    public class UsuarioNotificacionControlador
    {
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
                    await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
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


    }
}
