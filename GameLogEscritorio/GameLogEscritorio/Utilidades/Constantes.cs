using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Juegos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Utilidades
{
    public static class Constantes
    {
        public const string TipoInformacion = "Informacion";
        public const string TipoError = "Error";
        public const string TipoAdvertencia = "Advertencia";
        public const string TipoExito = "Exito";
        public const string TipoConfirmarAccion = "Confirmacion";
        public const int ErrorEnLaOperacion = -1;
        public const int OperacionExitosa = 1;
        public const int ValorPorDefecto = 0;
        public const string TituloExcepcionServidor = "Error de servidor";
        public const string ContenidoDatosInvalidos = "Los datos ingresados son inválidos, por favor verifique los campos marcados.";
        public const string ContenidoExcepcionServidor = "El servidor esta inactivo. Por favor inténtelo más tarde.";
        public const string TituloExcepcionBD = "Error de base de datos";
        public const string ContenidoExcepcionBD = "No hay conexión con la base de datos. Por favor, inténtelo más tarde.";
        public const int CodigoExito = 200;
        public const int CodigoErrorSolicitud = 400;
        public const int CodigoErrorServidor = 500;
        public const int CodigoErrorAcceso = 401;
        public const int CodigoSinResultadosEncontrados = 404;
        public const string tipoJugadorPorDefecto = "Jugador";
        public const string TipoDeEstadoPorDefecto = "Desbaneado";
        public const string JuegoNoEncontradoRawg = "Not found.";
        public static ObservableCollection<JuegoCompleto> juegosPendientes = new ObservableCollection<JuegoCompleto>();
        public static List<Juego> juegosFavoritos = new List<Juego>();
    }
}
