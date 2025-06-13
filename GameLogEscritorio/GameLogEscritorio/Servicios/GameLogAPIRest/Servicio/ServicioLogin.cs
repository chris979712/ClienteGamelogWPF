using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Login;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Utilidades;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Servicio
{
    public static class ServicioLogin
    {

        private static readonly string _apiURL = Properties.Resources.ApiUrlLogin;

        public static async Task<ApiLoginRespuesta> IniciarSesion(PostLoginSolicitud datosSolicitud, IApiRestRespuestaFactory apiRespuestaFactory)
        {
            ApiLoginRespuesta respuesta = new ApiLoginRespuesta();
            HttpClientHandler handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                UseCookies = false,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
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
                    if (mensajeObtenido.Headers.TryGetValues("access_token", out var tokens))
                    {
                        string tokenAcceso = tokens.FirstOrDefault()!;
                        SesionToken.GuardarToken(tokenAcceso);
                    }
                    respuesta = await apiRespuestaFactory.CrearRespuestaHTTP<ApiLoginRespuesta>(mensajeObtenido);
                }
                
                catch(Exception excepcion)
                {
                    respuesta = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiLoginRespuesta>(excepcion);
                }
            }
            return respuesta;
        }

        public static async Task<APIRecuperacionDeCuentaRespuesta> RecuperacionDeCuenta(PostRecuperacionDeCuentaSolicitud datosSolicitud,IApiRestRespuestaFactory apiRespuestaFactory)
        {
            APIRecuperacionDeCuentaRespuesta respuesta = new APIRecuperacionDeCuentaRespuesta();
            HttpClientHandler handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                UseCookies = false,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            using (var clienteHttp = new HttpClient(handler))
            {
                try
                {
                    var mensajeHttp = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(string.Concat(_apiURL, "/recuperacionDeCuenta")),
                        Content = new StringContent(JsonConvert.SerializeObject(datosSolicitud), Encoding.UTF8, "application/json")
                    };
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    respuesta = await apiRespuestaFactory.CrearRespuestaHTTP<APIRecuperacionDeCuentaRespuesta>(mensajeObtenido);
                }
                catch(Exception excepcion)
                {
                    respuesta = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<APIRecuperacionDeCuentaRespuesta>(excepcion);
                }
            }
            return respuesta;
        }

        public static async Task<ApiRespuestaBase> RecuperacionDeCuentaValidacion(PostRecuperacionDeCuentaValidacion datosSolicitud, IApiRestRespuestaFactory apiRespuestaFactory)
        {
            ApiRespuestaBase respuesta = new ApiLoginRespuesta();
            HttpClientHandler handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                UseCookies = false,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            using (var clienteHttp = new HttpClient(handler))
            {
                try
                {
                    var mensajeHttp = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(string.Concat(_apiURL, "/recuperacionDeCuenta/validacion")),
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
            return respuesta;
        }

        public static async Task<ApiRespuestaBase> CerrarSesion(string correo, IApiRestRespuestaFactory apiRespuestaFactory)
        {
            ApiRespuestaBase respuesta = new ApiLoginRespuesta();
            HttpClientHandler handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                UseCookies = false,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            using (var clienteHttp = new HttpClient(handler))
            {
                try
                {
                    var mensajeHttp = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(string.Concat(_apiURL, $"/logout/{correo}")),
                    };
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
