using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Likes;
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
    public static class ServicioLike
    {

        private static readonly string _ApiURLLike = Properties.Resources.ApiUrlLike;

        public static async Task<ApiRespuestaBase> RegistrarLikeAReseña(PostLikeSolicitud datosSolicitud)
        {
            ApiRespuestaBase respuestaBase = new ApiRespuestaBase();
            using (var clienteHttp = new HttpClient())
            {
                string tokenUsuario = SesionToken.LeerToken();
                try
                {
                    var mensajeHttp = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(_ApiURLLike),
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

        public static async Task<ApiRespuestaBase> EliminarLikeAReseña(int idResena,int idJugador)
        {
            ApiRespuestaBase respuestaBase = new ApiRespuestaBase();
            using (var clienteHttp = new HttpClient())
            {
                string tokenUsuario = SesionToken.LeerToken();
                try
                {
                    var mensajeHttp = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(string.Concat(_ApiURLLike,$"/{idResena}/{idJugador}"))
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
