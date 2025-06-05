using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Juegos;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using System.Net.Http.Headers;
using GameLogEscritorio.Utilidades;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Servicio
{
    public static class ServicioJuego
    {
        private static readonly string _JuegoURL = Properties.Resources.ApiUrlJuego;


        public static async Task<ApiRespuestaBase> RegistrarJuego(PostJuegoSolicitud datosSolicitud, IApiRestRespuestaFactory apiRespuestaFactory)
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
                    respuestaBase = await apiRespuestaFactory.CrearRespuestaHTTP<ApiRespuestaBase>(mensajeObtenido);
                }
                catch (Exception excepcion)
                {
                    respuestaBase = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiRespuestaBase>(excepcion);
                }
            }
            return respuestaBase;
        }

        public static async Task<ApiRespuestaBase> RegistrarJuegoFavorito(PostJuegoFavorito datosSolicitud, IApiRestRespuestaFactory apiRespuestaFactory)
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
                    respuestaBase = await apiRespuestaFactory.CrearRespuestaHTTP<ApiRespuestaBase>(mensajeObtenido);
                }
                catch (Exception excepcion)
                {
                    respuestaBase = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiRespuestaBase>(excepcion);
                }
            }
            return respuestaBase;
        }

        public static async Task<ApiRespuestaBase> RegistrarJuegoPendiente(PostJuegoPendienteSolicitud datosSolicitud, IApiRestRespuestaFactory apiRespuestaFactory)
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
                    respuestaBase = await apiRespuestaFactory.CrearRespuestaHTTP<ApiRespuestaBase>(mensajeObtenido);
                }
                catch (Exception excepcion)
                {
                    respuestaBase = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiRespuestaBase>(excepcion);
                }
            }
            return respuestaBase;
        }

        public static async Task<ApiJuegoRespuesta> BuscarJuegoPorID(int idJuego, IApiRestRespuestaFactory apiRespuestaFactory)
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
                    respuestaJuego = await apiRespuestaFactory.CrearRespuestaHTTP<ApiJuegoRespuesta>(mensajeObtenido);
                }
                catch (Exception excepcion)
                {
                    respuestaJuego = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiJuegoRespuesta>(excepcion);
                }
            }
            return respuestaJuego;
        }

        public static async Task<ApiJuegoRespuesta> BuscarJuegoPorNombre(string nombre, IApiRestRespuestaFactory apiRespuestaFactory)
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
                    respuestaJuego = await apiRespuestaFactory.CrearRespuestaHTTP<ApiJuegoRespuesta>(mensajeObtenido);
                }
                catch (Exception excepcion)
                {
                    respuestaJuego = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiJuegoRespuesta>(excepcion);
                }
            }
            return respuestaJuego;
        }

        public static async Task<ApiJuegosRespuesta> ObtenerJuegosPendientes(int idJugador, IApiRestRespuestaFactory apiRespuestaFactory)
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
                    respuestaJuegos = await apiRespuestaFactory.CrearRespuestaHTTP<ApiJuegosRespuesta>(mensajeObtenido);
                }
                catch (Exception excepcion)
                {
                    respuestaJuegos = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiJuegosRespuesta>(excepcion);
                }
            }
            return respuestaJuegos;
        }

        public static async Task<ApiJuegosRespuesta> ObtenerJuegosFavoritos(int idJugador, IApiRestRespuestaFactory apiRespuestaFactory)
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
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    respuestaJuegos = await apiRespuestaFactory.CrearRespuestaHTTP<ApiJuegosRespuesta>(mensajeObtenido);
                }
                catch (Exception excepcion)
                {
                    respuestaJuegos = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiJuegosRespuesta>(excepcion);
                }
            }
            return respuestaJuegos;
        }

        public static async Task<ApiRespuestaBase> EliminarJuegoPendiente(int idJuego, int idJugador, IApiRestRespuestaFactory apiRespuestaFactory)
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
                    respuestaBase = await apiRespuestaFactory.CrearRespuestaHTTP<ApiRespuestaBase>(mensajeObtenido);
                }
                catch (Exception excepcion)
                {
                    respuestaBase = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiRespuestaBase>(excepcion);
                }
            }
            return respuestaBase;
        }

        public static async Task<ApiRespuestaBase> EliminarJuegoFavorito(int idJuego, int idJugador, IApiRestRespuestaFactory apiRespuestaFactory)
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
