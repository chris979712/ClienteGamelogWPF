using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Jugador;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Utilidades;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Servicio
{
    public static class ServicioJugador
    {

        private static readonly string _ApiURLJugador = Properties.Resources.ApiUrlJugador;

        public static async Task<ApiJugadorRespuesta> ObtenerJugadorPorNombreDeUsuario(string nombreDeUsuario, IApiRestRespuestaFactory apiRespuestaFactory)
        {
            ApiJugadorRespuesta respuestaJugador = new ApiJugadorRespuesta();
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
                        RequestUri = new Uri(string.Concat(_ApiURLJugador, $"/{nombreDeUsuario}"))
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    respuestaJugador = await apiRespuestaFactory.CrearRespuestaHTTP<ApiJugadorRespuesta>(mensajeObtenido);
                }
                catch (Exception excepcion)
                {
                    respuestaJugador = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiJugadorRespuesta>(excepcion);
                }
            }
            return respuestaJugador;
        }

        public static async Task<ApiRespuestaBase> ActualizarDatosDeJugador(PutJugadorSolicitud datosSolicitud, int idJugador, IApiRestRespuestaFactory apiRespuestaFactory)
        {
            ApiRespuestaBase respuestaBase = new ApiRespuestaBase();
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using (var clienteHttp = new HttpClient(handler))
            {
                string tokenUsuario = SesionToken.LeerToken();
                try
                {
                    var mensajeHttp = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Put,
                        RequestUri = new Uri(string.Concat(_ApiURLJugador, $"/{idJugador}")),
                        Content = new StringContent(JsonConvert.SerializeObject(datosSolicitud), Encoding.UTF8, "application/json")
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    respuestaBase = await apiRespuestaFactory.CrearRespuestaHTTP<ApiRespuestaBase>(mensajeObtenido);
                }
                catch (Exception excepcion)
                {
                    respuestaBase = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiRespuestaBase>(excepcion);
                }
            }
            return respuestaBase;
        }

    }
}
