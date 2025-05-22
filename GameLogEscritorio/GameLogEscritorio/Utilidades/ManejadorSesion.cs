using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Ventanas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Utilidades
{
    public static class ManejadorSesion
    {
        public static async Task<bool> CerrarSesionForzadaDeUsuario()
        {
            await ServicioLogin.CerrarSesion(UsuarioSingleton.Instancia.correo!);
            UsuarioSingleton.Instancia.CerrarSesion();
            SesionToken.CerrarSesion();
            return true;
        }

        public static async Task<bool> RegresarInicioDeSesionUsuario()
        {
            ApiRespuestaBase respuesta = await ServicioLogin.CerrarSesion(UsuarioSingleton.Instancia.correo!);
            UsuarioSingleton.Instancia.CerrarSesion();
            SesionToken.CerrarSesion();
            VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoExito, respuesta.mensaje!, respuesta.estado);
            VentanaInicioDeSesion ventanaInicioDeSesion = new VentanaInicioDeSesion();
            ventanaInicioDeSesion.Show();
            return true;
        }

        public static async Task<bool> RegresarInicioDeSesionSinAcceso(string mensajeSinAcceso)
        {
            ApiRespuestaBase respuesta = await ServicioLogin.CerrarSesion(UsuarioSingleton.Instancia.correo!);
            UsuarioSingleton.Instancia.CerrarSesion();
            SesionToken.CerrarSesion();
            VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, mensajeSinAcceso, respuesta.estado);
            VentanaInicioDeSesion ventanaInicioDeSesion = new VentanaInicioDeSesion();
            ventanaInicioDeSesion.Show();
            return true;
        }

        public static async Task<bool> RegresarInicioDeSesionSinAcceso()
        {
            ApiRespuestaBase respuesta = await ServicioLogin.CerrarSesion(UsuarioSingleton.Instancia.correo!);
            UsuarioSingleton.Instancia.CerrarSesion();
            SesionToken.CerrarSesion();
            VentanaInicioDeSesion ventanaInicioDeSesion = new VentanaInicioDeSesion();
            ventanaInicioDeSesion.Show();
            return true;
        }
    }

}
