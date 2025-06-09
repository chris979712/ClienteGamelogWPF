using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Social;
using GameLogEscritorio.Utilidades;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Servicio
{
    public static class ServicioSeguidor
    {

        private static readonly string _ApiURLSeguidor = Properties.Resources.ApiUrlSeguidor;

        public static async Task<ApiRespuestaBase> RegistrarNuevoSeguidor(PostSeguidorSolicitud datosSolicitud, IApiRestRespuestaFactory apiRespuestaFactory)
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
                        RequestUri = new Uri(_ApiURLSeguidor),
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

        public static async Task<ApiSeguidoresRespuesta> ObtenerJugadoresSeguidores(int idJugadorSeguido, IApiRestRespuestaFactory apiRespuestaFactory)
        {
            ApiSeguidoresRespuesta respuestaSeguidores = new ApiSeguidoresRespuesta();
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
                        RequestUri = new Uri(string.Concat(_ApiURLSeguidor, $"/seguidores/{idJugadorSeguido}")),
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    respuestaSeguidores = await apiRespuestaFactory.CrearRespuestaHTTP<ApiSeguidoresRespuesta>(mensajeObtenido);
                }
                catch (Exception excepcion)
                {
                    respuestaSeguidores = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiSeguidoresRespuesta>(excepcion);
                }
            }
            return respuestaSeguidores;
        }

        public static async Task<ApiSeguidosRespuesta> ObtenerJugadoresSeguidos(int idJugadorSeguidor, IApiRestRespuestaFactory apiRespuestaFactory)
        {
            ApiSeguidosRespuesta respuestaSeguidos = new ApiSeguidosRespuesta();
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
                        RequestUri = new Uri(string.Concat(_ApiURLSeguidor, $"/seguidos/{idJugadorSeguidor}")),
                    };
                    mensajeHttp.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenUsuario);
                    HttpResponseMessage mensajeObtenido = await clienteHttp.SendAsync(mensajeHttp);
                    respuestaSeguidos = await apiRespuestaFactory.CrearRespuestaHTTP<ApiSeguidosRespuesta>(mensajeObtenido);
                }
                catch (Exception excepcion)
                {
                    respuestaSeguidos = ClasificadorExcepcion.DeterminarTipoExcepcionAPIRest<ApiSeguidosRespuesta>(excepcion);
                }
            }
            return respuestaSeguidos;
        }



        public static async Task<ApiRespuestaBase> EliminarJugadorSeguido(int idJugadorSeguidor, int idJugadorSeguido, IApiRestRespuestaFactory apiRespuestaFactory)
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
                        RequestUri = new Uri(string.Concat(_ApiURLSeguidor,$"/{idJugadorSeguidor}/{idJugadorSeguido}")),
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
