using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi
{
    public class APIRecuperacionDeCuentaRespuesta : ApiRespuestaBase
    {
        [JsonProperty("idAcceso")]
        public int idAcceso { get; set; }
    }
}
