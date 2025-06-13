using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Likes;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.MeGusta;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Utilidades;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Servicio
{
    public static class ServicioMeGusta
    {

        private static readonly string _ApiUrlMeGusta = Properties.Resources.ApiUrlMeGusta;

        public static async Task<ApiRespuestaBase> RegistrarMeGustaAReseña(PostMeGustaSolicitud datosSolicitud, IApiRestRespuestaFactory apiRespuestaFactory)
        {
            ApiRespuestaBase respuestaBase = new ApiRespuestaBase();
            HttpClientHandler handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                UseCookies = false,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            using (var clienteHttp = new HttpClient(handler))
            {
                string tokenUsuario = SesionToken.LeerToken();
                try
                {
                    var mensajeHttp = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(_ApiUrlMeGusta),
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

        public static async Task<ApiRespuestaBase> EliminarMeGustaAReseña(int idJuego,DeleteMeGustaSolicitud solicitud ,IApiRestRespuestaFactory apiRespuestaFactory)
        {
            ApiRespuestaBase respuestaBase = new ApiRespuestaBase();
            HttpClientHandler handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                UseCookies = false,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            using (var clienteHttp = new HttpClient(handler))
            {
                string tokenUsuario = SesionToken.LeerToken();
                try
                {
                    var mensajeHttp = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(string.Concat(_ApiUrlMeGusta,$"/{idJuego}")),
                        Content = new StringContent(JsonConvert.SerializeObject(solicitud),Encoding.UTF8,"application/json")
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
