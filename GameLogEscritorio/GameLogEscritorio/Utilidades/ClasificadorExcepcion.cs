using GameLogEscritorio.Log4net;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using Newtonsoft.Json;
using System.Net.Http;

namespace GameLogEscritorio.Utilidades
{
    public static class ClasificadorExcepcion
    {

        public static T DeterminarTipoExcepcionAPIRest<T>(Exception excepcion) where T : ApiRespuestaBase, new()
        {
            T apiRespuestaBase = new T
            {
                error = true,
                estado = 500,
            };
            switch (excepcion)
            {
                case HttpRequestException httpExcepcion:
                    LoggerManejador.Error(excepcion.Message);
                    apiRespuestaBase.mensaje = Properties.Resources.HttpExcepcion;
                    break;
                case TaskCanceledException cancelExcepcion:
                    LoggerManejador.Error(excepcion.Message);
                    apiRespuestaBase.mensaje = Properties.Resources.TaskCanceledExcepcion;
                    break;

                case JsonException jsonExcepcion:
                    LoggerManejador.Informacion(excepcion.Message);
                    apiRespuestaBase.mensaje = Properties.Resources.JsonExcepcion;
                    break;

                case ArgumentNullException argExcepcion:
                    LoggerManejador.Error(excepcion.Message);
                    apiRespuestaBase.mensaje = Properties.Resources.ArgumentNullExcepcion;
                    break;

                default:
                    LoggerManejador.Fatal(excepcion.Message);
                    apiRespuestaBase.mensaje = Properties.Resources.Excepcion;
                    break;
            }
            return apiRespuestaBase;
        }
    }
}
