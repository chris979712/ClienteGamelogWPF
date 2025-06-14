using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Login;
using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi
{
    public class ApiAccesoRespuesta<T> : ApiRespuestaBase
    {
        [JsonProperty("id")]
        public Cuenta? datos {  get; set; }
    }
}
