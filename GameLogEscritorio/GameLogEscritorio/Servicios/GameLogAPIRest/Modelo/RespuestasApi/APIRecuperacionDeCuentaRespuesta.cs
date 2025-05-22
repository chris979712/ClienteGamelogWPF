using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi
{
    public class APIRecuperacionDeCuentaRespuesta : ApiRespuestaBase
    {
        [JsonProperty("idAcceso")]
        public int idAcceso { get; set; }
    }
}
