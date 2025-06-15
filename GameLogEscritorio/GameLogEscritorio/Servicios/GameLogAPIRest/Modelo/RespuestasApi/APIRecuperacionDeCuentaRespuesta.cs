using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi
{
    public class ApiRecuperacionDeCuentaRespuesta : ApiRespuestaBase
    {
        [JsonProperty("idAcceso")]
        public int idAcceso { get; set; }
    }
}
