using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Jugador;
using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi
{
    public class ApiJugadorRespuesta : ApiRespuestaBase
    {
        [JsonProperty("cuenta")]
        public List<Perfil>? jugador {  get; set; }
    }
}
