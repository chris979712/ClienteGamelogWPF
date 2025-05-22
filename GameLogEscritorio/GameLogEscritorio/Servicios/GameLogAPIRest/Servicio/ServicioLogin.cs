using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Login;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GameLogEscritorio.Utilidades;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Servicio
{
    public static class ServicioLogin
    {

        private static readonly string _apiURL = Properties.Resources.ApiUrlLogin;

        public static async Task<ApiLoginRespuesta> IniciarSesion(PostLoginSolicitud datosSolicitud)
        {
            ApiLoginRespuesta respuesta = new ApiLoginRespuesta();
            using(var clienteHttp = new HttpClient())
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
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    respuesta = JsonConvert.DeserializeObject<ApiLoginRespuesta>(contenidoJson)!;
                }
                catch(Exception excepcion)
                {
                    respuesta = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiLoginRespuesta>(excepcion);
                }
            }
            return respuesta;
        }

        public static async Task<APIRecuperacionDeCuentaRespuesta> RecuperacionDeCuenta(PostRecuperacionDeCuentaSolicitud datosSolicitud)
        {
            APIRecuperacionDeCuentaRespuesta respuesta = new APIRecuperacionDeCuentaRespuesta();
            using(var clienteHttp = new HttpClient())
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
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    respuesta = JsonConvert.DeserializeObject<APIRecuperacionDeCuentaRespuesta>(contenidoJson)!;
                }
                catch(Exception excepcion)
                {
                    respuesta = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<APIRecuperacionDeCuentaRespuesta>(excepcion);
                }
            }
            return respuesta;
        }

        public static async Task<ApiRespuestaBase> RecuperacionDeCuentaValidacion(PostRecuperacionDeCuentaValidacion datosSolicitud)
        {
            ApiRespuestaBase respuesta = new ApiLoginRespuesta();
            using (var clienteHttp = new HttpClient())
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
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    respuesta = JsonConvert.DeserializeObject<ApiLoginRespuesta>(contenidoJson)!;
                }
                catch (Exception excepcion)
                {
                    respuesta = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiRespuestaBase>(excepcion);
                }
            }
            return respuesta;
        }

        public static async Task<ApiRespuestaBase> CerrarSesion(string correo)
        {
            ApiRespuestaBase respuesta = new ApiLoginRespuesta();
            using (var clienteHttp = new HttpClient())
            {
                try
                {
                    var mensajeHttp = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(string.Concat(_apiURL, $"/logout/{correo}")),
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
            return respuesta;
        }
    }
}
