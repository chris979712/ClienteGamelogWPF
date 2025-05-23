using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Reseñas;
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
    public static class ServicioReseña
    {
        private static readonly string _ApiURL = Properties.Resources.ApiUrlResena;

        public static async Task<ApiRespuestaBase> RegistrarReseña(PostReseñaSolicitud datosSolicitud)
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
                        RequestUri = new Uri(_ApiURL),
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

        public static async Task<ApiReseñaPersonalRespuesta> ObtenerReseñasDeUnJugador(int idJugador, int idJugadoBuscador)
        {
            ApiReseñaPersonalRespuesta respuestaReseñas = new ApiReseñaPersonalRespuesta();
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
                        RequestUri = new Uri(string.Concat(_ApiURL,$"/jugador/{idJugador}?idJugadorBuscador={idJugadoBuscador}"))
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    respuestaReseñas = JsonConvert.DeserializeObject<ApiReseñaPersonalRespuesta>(contenidoJson)!;
                }
                catch (Exception excepcion)
                {
                    respuestaReseñas = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiReseñaPersonalRespuesta>(excepcion);
                }
            }
            return respuestaReseñas;
        }

        public static async Task<ApiReseñaJugadoresRespuesta> ObtenerReseñasDeUnJuego(int idJuego, int idJugadoBuscador)
        {
            ApiReseñaJugadoresRespuesta respuestaReseñasJugadores = new ApiReseñaJugadoresRespuesta();
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
                        RequestUri = new Uri(string.Concat(_ApiURL, $"/juego/{idJuego}?idJugadorBuscador={idJugadoBuscador}"))
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    respuestaReseñasJugadores = JsonConvert.DeserializeObject<ApiReseñaJugadoresRespuesta>(contenidoJson)!;
                }
                catch (Exception excepcion)
                {
                    respuestaReseñasJugadores = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiReseñaJugadoresRespuesta>(excepcion);
                }
            }
            return respuestaReseñasJugadores;
        }

        public static async Task<ApiReseñaJugadoresRespuesta> ObtenerReseñasDeJugadoresSeguidosEnUnJuego(int idJuego, int idJugadoBuscador)
        {
            ApiReseñaJugadoresRespuesta respuestaReseñasJugadores = new ApiReseñaJugadoresRespuesta();
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
                        RequestUri = new Uri(string.Concat(_ApiURL, $"/juego/{idJuego}/seguidos?idJugador={idJugadoBuscador}"))
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    respuestaReseñasJugadores = JsonConvert.DeserializeObject<ApiReseñaJugadoresRespuesta>(contenidoJson)!;
                }
                catch (Exception excepcion)
                {
                    respuestaReseñasJugadores = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiReseñaJugadoresRespuesta>(excepcion);
                }
            }
            return respuestaReseñasJugadores;
        }

        public static async Task<ApiRespuestaBase> EliminarReseña(int idReseña)
        {
            ApiRespuestaBase respuestaReseñasJugadores = new ApiRespuestaBase();
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
                        RequestUri = new Uri(string.Concat(_ApiURL, $"/{idReseña}"))
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    respuestaReseñasJugadores = JsonConvert.DeserializeObject<ApiRespuestaBase>(contenidoJson)!;
                }
                catch (Exception excepcion)
                {
                    respuestaReseñasJugadores = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiRespuestaBase>(excepcion);
                }
            }
            return respuestaReseñasJugadores;
        }


    }
}
