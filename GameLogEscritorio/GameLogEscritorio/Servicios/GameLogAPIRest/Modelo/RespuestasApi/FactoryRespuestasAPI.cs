using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Utilidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi
{
    public interface IApiRestRespuestaFactory
    {
        Task<TRespuesta> CrearRespuestaHTTP<TRespuesta>(HttpResponseMessage mensaje) where TRespuesta : ApiRespuestaBase, new();
    }

    public class FactoryRespuestasAPI : IApiRestRespuestaFactory
    {
        public async Task<TRespuesta> CrearRespuestaHTTP<TRespuesta>(HttpResponseMessage mensaje)
            where TRespuesta : ApiRespuestaBase, new()
        {
            TRespuesta respuestaFabrica;
            if ((int)mensaje.StatusCode == Constantes.CodigoBadGetaway)
            {
                respuestaFabrica = new TRespuesta()
                {
                    error = true,
                    estado = (int)mensaje.StatusCode,
                    mensaje = Properties.Resources.BadGetAwayMensaje
                };
            }
            else
            {
                string contenido = await mensaje.Content.ReadAsStringAsync();
                respuestaFabrica = JsonConvert.DeserializeObject<TRespuesta>(contenido)!;
            }
            return respuestaFabrica;
        }

    }
}
