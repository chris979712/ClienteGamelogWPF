using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Utilidades;
using System.Net.Http;
using System.Net.Http.Headers;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Servicio
{
    public static class ServicioNotificaciones
    {
        private static readonly string _ApiUrlNotificacion = Properties.Resources.ApiURLNotificacion;

        public static async Task<ApiRespuestaBase> EliminarNotificacion(int idNotificacion, IApiRestRespuestaFactory apiRestRespuestaFactory)
        {
            ApiRespuestaBase respuestaBase = new ApiRespuestaBase();
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using(var clienteHttp = new HttpClient(handler))
            {
                string tokenUsuario = SesionToken.LeerToken();
                try
                {
                    var mensajeHttp = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(string.Concat(_ApiUrlNotificacion,$"/{idNotificacion}")),
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    respuestaBase = await apiRestRespuestaFactory.CrearRespuestaHTTP<ApiRespuestaBase>(mensajeObtenido);
                }
                catch(Exception excepcion)
                {
                    respuestaBase = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiRespuestaBase>(excepcion);
                }
            }
            return respuestaBase;
        }

        public static async Task<ApiNotificacionRespuesta> ObtenerNotificacionesDeJugador(int idJugador, IApiRestRespuestaFactory apiRestRespuestaFactory)
        {
            ApiNotificacionRespuesta apiNotificacionRespuesta = new ApiNotificacionRespuesta();
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using (var clienteHttp = new HttpClient(handler))
            {
                string tokenUsuario = SesionToken.LeerToken();
                try
                {
                    var mensajeHttp = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Concat(_ApiUrlNotificacion, $"/jugador/{idJugador}")),
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    apiNotificacionRespuesta = await apiRestRespuestaFactory.CrearRespuestaHTTP<ApiNotificacionRespuesta>(mensajeObtenido);
                }
                catch (Exception excepcion)
                {
                    apiNotificacionRespuesta = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiNotificacionRespuesta>(excepcion);
                }
            }
            return apiNotificacionRespuesta;

        }
    }
}
