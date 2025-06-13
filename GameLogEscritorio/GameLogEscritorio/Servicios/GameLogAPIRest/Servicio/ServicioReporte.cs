using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Utilidades;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Servicio
{
    public static class ServicioReporte
    {

        private static readonly string _ApiURLReporte = Properties.Resources.ApiUrlReporte;

        public static async Task<ApiReportesRespuesta> ObtenerReporteTendencias(string fechaInicioBusqueda, string fechaFinBusqueda, IApiRestRespuestaFactory apiRespuestaFactory)
        {
            ApiReportesRespuesta respuestaReportes = new ApiReportesRespuesta();
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
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Concat(_ApiURLReporte, $"/tendencias/{fechaInicioBusqueda}/{fechaFinBusqueda}")),
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    respuestaReportes = await apiRespuestaFactory.CrearRespuestaHTTP<ApiReportesRespuesta>(mensajeObtenido);
                }
                catch (Exception excepcion)
                {
                    respuestaReportes = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiReportesRespuesta>(excepcion);
                }
            }
            return respuestaReportes;
        }

        public static async Task<ApiReportesRespuesta> ObtenerReporteTendenciasRevivalRetro(string fechaInicioBusqueda, string fechaFinBusqueda, IApiRestRespuestaFactory apiRespuestaFactory)
        {
            ApiReportesRespuesta respuestaReportes = new ApiReportesRespuesta();
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
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Concat(_ApiURLReporte, $"/revivalretro/{fechaInicioBusqueda}/{fechaFinBusqueda}")),
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    respuestaReportes = await apiRespuestaFactory.CrearRespuestaHTTP<ApiReportesRespuesta>(mensajeObtenido);
                }
                catch (Exception excepcion)
                {
                    respuestaReportes = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiReportesRespuesta>(excepcion);
                }
            }
            return respuestaReportes;
        }
    }
}
