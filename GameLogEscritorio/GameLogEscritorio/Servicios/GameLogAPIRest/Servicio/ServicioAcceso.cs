using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Acceso;
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
    public static class ServicioAcceso
    {

        private static readonly string _apiURL = Properties.Resources.ApiUrlAcceso;

        public static async Task<ApiRespuestaBase> CrearCuenta(PostAccesoSolicitud datosSolicitud, IApiRestRespuestaFactory apiRespuestaFactory)
        {
            ApiRespuestaBase respuesta = new ApiRespuestaBase();
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using (var clienteHttp = new HttpClient(handler))
            {
                try
                {
                    var mensajeHttp = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(_apiURL),
                        Content = new StringContent(JsonConvert.SerializeObject(datosSolicitud), Encoding.UTF8, "application/json")
                    };
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    respuesta = await apiRespuestaFactory.CrearRespuestaHTTP<ApiRespuestaBase>(mensajeObtenido);
                }
                catch (Exception excepcion)
                {
                    respuesta = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiAccesoRespuesta<Perfil>>(excepcion);
                }
            }
            return respuesta!;
        }

        public static async Task<ApiRespuestaBase> EditarCredencialesDeAcceso(PutAccesoSolicitud datosSolicitud, int idAcceso, IApiRestRespuestaFactory apiRespuestaFactory)
        {
            ApiRespuestaBase respuesta = new ApiRespuestaBase();
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using (var clienteHttp = new HttpClient(handler))
            {
                try
                {
                    var mensajeHttp = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Put,
                        RequestUri = new Uri(string.Concat(Properties.Resources.ApiUrlAcceso,$"/{idAcceso}")),
                        Content = new StringContent(JsonConvert.SerializeObject(datosSolicitud), Encoding.UTF8, "application/json")
                    };
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    respuesta = await apiRespuestaFactory.CrearRespuestaHTTP<ApiRespuestaBase>(mensajeObtenido);
                }
                catch (Exception excepcion)
                {
                    respuesta = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiRespuestaBase>(excepcion);
                }
            }
            return respuesta!;
        }

        public static async Task<ApiRespuestaBase> CambiarEstadoDeAcceso(PatchAccesoSolicitud datosSolicitud, int idAcceso, IApiRestRespuestaFactory apiRespuestaFactory)
        {
            ApiRespuestaBase respuesta = new ApiRespuestaBase();
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using (var clienteHttp = new HttpClient(handler))
            {
                try
                {
                    string tokenUsuario = SesionToken.LeerToken();
                    var patchMethod = new HttpMethod("PATCH");
                    var mensajeHttp = new HttpRequestMessage()
                    {
                        Method = patchMethod,
                        RequestUri = new Uri(string.Concat(Properties.Resources.ApiUrlAcceso, $"/{idAcceso}")),
                        Content = new StringContent(JsonConvert.SerializeObject(datosSolicitud), Encoding.UTF8, "application/json")
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    respuesta = await apiRespuestaFactory.CrearRespuestaHTTP<ApiRespuestaBase>(mensajeObtenido);
                }
                catch (Exception excepcion)
                {
                    respuesta = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiRespuestaBase>(excepcion);
                }
            }
            return respuesta;
        }
    }
}
