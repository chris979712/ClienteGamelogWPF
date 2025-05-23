using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Utilidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Servicio
{
    public static class ServicioReporte
    {

        private static readonly string _ApiURLReporte = Properties.Resources.ApiUrlReporte;

        public static async Task<ApiReportesRespuesta> ObtenerReporteTendencias(string fechaInicioBusqueda, string fechaFinBusqueda)
        {
            ApiReportesRespuesta respuestaReportes = new ApiReportesRespuesta();
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
                        RequestUri = new Uri(string.Concat(_ApiURLReporte, $"/tendencias/{fechaInicioBusqueda}/{fechaFinBusqueda}")),
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    respuestaReportes = JsonConvert.DeserializeObject<ApiReportesRespuesta>(contenidoJson)!;
                }
                catch (Exception excepcion)
                {
                    respuestaReportes = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiReportesRespuesta>(excepcion);
                }
            }
            return respuestaReportes;
        }

        public static async Task<ApiReportesRespuesta> ObtenerReporteTendenciasRevivalRetro(string fechaInicioBusqueda, string fechaFinBusqueda)
        {
            ApiReportesRespuesta respuestaReportes = new ApiReportesRespuesta();
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
                        RequestUri = new Uri(string.Concat(_ApiURLReporte, $"/revivalretro/{fechaInicioBusqueda}/{fechaFinBusqueda}")),
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    respuestaReportes = JsonConvert.DeserializeObject<ApiReportesRespuesta>(contenidoJson)!;
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
