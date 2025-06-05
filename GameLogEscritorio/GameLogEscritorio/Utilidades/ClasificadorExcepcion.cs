using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using Newtonsoft.Json;
using System.Net.Http;

namespace GameLogEscritorio.Utilidades
{
    public static class ClasificadorExcepcion
    {

        public static T DeterminarTipoExcepcionAPIRest<T>(Exception exception) where T : ApiRespuestaBase, new()
        {
            T apiRespuestaBase = new T
            {
                error = true,
                estado = 500,
            };
            switch (exception)
            {
                case HttpRequestException httpEx:
                    apiRespuestaBase.mensaje = Properties.Resources.HttpExcepcion;
                    break;
                case TaskCanceledException cancelEx:
                    apiRespuestaBase.mensaje = Properties.Resources.TaskCanceledExcepcion;
                    break;

                case JsonException jsonEx:
                    apiRespuestaBase.mensaje = Properties.Resources.JsonExcepcion;
                    break;

                case ArgumentNullException argEx:
                    apiRespuestaBase.mensaje = Properties.Resources.ArgumentNullExcepcion;
                    break;

                default:
                    apiRespuestaBase.mensaje = Properties.Resources.Excepcion;
                    break;
            }
            return apiRespuestaBase;
        }
    }
}
