using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Acceso;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Jugador;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Utilidades;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Servicio
{
    public static class ServicioAcceso
    {

        private static readonly string _apiURL = Properties.Resources.ApiUrlAcceso;

        public static async Task<ApiAccesoRespuesta<Perfil>> CrearCuenta(PostAccesoSolicitud datosSolicitud)
        {
            ApiAccesoRespuesta<Perfil> respuesta = new ApiAccesoRespuesta<Perfil>();
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
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    respuesta = JsonConvert.DeserializeObject<ApiAccesoRespuesta<Perfil>>(contenidoJson)!;
                }
                catch (Exception excepcion)
                {
                    respuesta = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiAccesoRespuesta<Perfil>>(excepcion);
                }
            }
            return respuesta!;
        }

        public static async Task<ApiAccesoRespuesta<int>> ObtenerIdAccesoPorCorreo(string correo)
        {
            ApiAccesoRespuesta<int> respuesta = new ApiAccesoRespuesta<int>();
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using (var clienteHttp = new HttpClient(handler))
            {
                try
                {
                    var mensajeHttp = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Concat(Properties.Resources.ApiUrlAcceso,$"/{correo}")),
                    };
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    respuesta = JsonConvert.DeserializeObject<ApiAccesoRespuesta<int>>(contenidoJson)!;
                }
                catch (Exception excepcion)
                {
                    respuesta = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiAccesoRespuesta<int>>(excepcion);
                }
            }
            return respuesta;
        }

        public static async Task<ApiRespuestaBase> EditarCredencialesDeAcceso(PutAccesoSolicitud datosSolicitud, int idAcceso)
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
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    respuesta = JsonConvert.DeserializeObject<ApiRespuestaBase>(contenidoJson)!;
                }
                catch (Exception excepcion)
                {
                    respuesta = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiRespuestaBase>(excepcion);
                }
            }
            return respuesta!;
        }

        public static async Task<ApiRespuestaBase> CambiarEstadoDeAcceso(PatchAccesoSolicitud datosSolicitud, int idAcceso)
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
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    respuesta = JsonConvert.DeserializeObject<ApiRespuestaBase>(contenidoJson)!;
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
