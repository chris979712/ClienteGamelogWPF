using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Juegos;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using System.Net.Http.Headers;
using GameLogEscritorio.Utilidades;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Servicio
{
    public static class ServicioJuego
    {
        private static readonly string _JuegoURL = Properties.Resources.ApiUrlJuego;


        public static async Task<ApiRespuestaBase> RegistrarJuego(PostJuegoSolicitud datosSolicitud)
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
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(_JuegoURL),
                        Content = new StringContent(JsonConvert.SerializeObject(datosSolicitud), Encoding.UTF8, "application/json")
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    respuestaBase = JsonConvert.DeserializeObject<ApiRespuestaBase>(contenidoJson)!;
                }
                catch (Exception excepcion)
                {
                    respuestaBase = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiRespuestaBase>(excepcion);
                }
            }
            return respuestaBase;
        }

        public static async Task<ApiRespuestaBase> RegistrarJuegoFavorito(PostJuegoFavorito datosSolicitud)
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
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(string.Concat(_JuegoURL,"/favorito")),
                        Content = new StringContent(JsonConvert.SerializeObject(datosSolicitud), Encoding.UTF8, "application/json")
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    respuestaBase = JsonConvert.DeserializeObject<ApiRespuestaBase>(contenidoJson)!;
                }
                catch (Exception excepcion)
                {
                    respuestaBase = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiRespuestaBase>(excepcion);
                }
            }
            return respuestaBase;
        }

        public static async Task<ApiRespuestaBase> RegistrarJuegoPendiente(PostJuegoPendienteSolicitud datosSolicitud)
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
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(string.Concat(_JuegoURL, "/pendiente")),
                        Content = new StringContent(JsonConvert.SerializeObject(datosSolicitud), Encoding.UTF8, "application/json")
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    respuestaBase = JsonConvert.DeserializeObject<ApiRespuestaBase>(contenidoJson)!;
                }
                catch (Exception excepcion)
                {
                    respuestaBase = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiRespuestaBase>(excepcion);
                }
            }
            return respuestaBase;
        }

        public static async Task<ApiJuegoRespuesta> BuscarJuegoPorID(int idJuego)
        {
            ApiJuegoRespuesta respuestaJuego = new ApiJuegoRespuesta();
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
                        RequestUri = new Uri(string.Concat(_JuegoURL, $"/{idJuego}")),
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    respuestaJuego = JsonConvert.DeserializeObject<ApiJuegoRespuesta>(contenidoJson)!;
                }
                catch (Exception excepcion)
                {
                    respuestaJuego = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiJuegoRespuesta>(excepcion);
                }
            }
            return respuestaJuego;
        }

        public static async Task<ApiJuegoRespuesta> BuscarJuegoPorNombre(string nombre)
        {
            ApiJuegoRespuesta respuestaJuego = new ApiJuegoRespuesta();
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
                        RequestUri = new Uri(string.Concat(_JuegoURL, $"?nombre={nombre}")),
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    respuestaJuego = JsonConvert.DeserializeObject<ApiJuegoRespuesta>(contenidoJson)!;
                }
                catch (Exception excepcion)
                {
                    respuestaJuego = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiJuegoRespuesta>(excepcion);
                }
            }
            return respuestaJuego;
        }

        public static async Task<ApiJuegosRespuesta> ObtenerJuegosPendientes(int idJugador)
        {
            ApiJuegosRespuesta respuestaJuegos = new ApiJuegosRespuesta();
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
                        RequestUri = new Uri(string.Concat(_JuegoURL, $"/pendiente/{idJugador}")),
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    respuestaJuegos = JsonConvert.DeserializeObject<ApiJuegosRespuesta>(contenidoJson)!;
                }
                catch (Exception excepcion)
                {
                    respuestaJuegos = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiJuegosRespuesta>(excepcion);
                }
            }
            return respuestaJuegos;
        }

        public static async Task<ApiJuegosRespuesta> ObtenerJuegosFavoritos(int idJugador)
        {
            ApiJuegosRespuesta respuestaJuegos = new ApiJuegosRespuesta();
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
                        RequestUri = new Uri(string.Concat(_JuegoURL, $"/favorito/{idJugador}")),
                    };
                    mensajeHttp.Headers.Add("access_token", $"Bearer {tokenUsuario}");
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    respuestaJuegos = JsonConvert.DeserializeObject<ApiJuegosRespuesta>(contenidoJson)!;
                }
                catch (Exception excepcion)
                {
                    respuestaJuegos = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiJuegosRespuesta>(excepcion);
                }
            }
            return respuestaJuegos;
        }

        public static async Task<ApiRespuestaBase> EliminarJuegoPendiente(int idJuego, int idJugador)
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
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(string.Concat(_JuegoURL, $"/pendiente/{idJuego}/{idJugador}")),
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    respuestaBase = JsonConvert.DeserializeObject<ApiRespuestaBase>(contenidoJson)!;
                }
                catch (Exception excepcion)
                {
                    respuestaBase = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiRespuestaBase>(excepcion);
                }
            }
            return respuestaBase;
        }

        public static async Task<ApiRespuestaBase> EliminarJuegoFavorito(int idJuego, int idJugador)
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
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(string.Concat(_JuegoURL, $"/favorito/{idJuego}/{idJugador}")),
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    respuestaBase = JsonConvert.DeserializeObject<ApiRespuestaBase>(contenidoJson)!;
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
