using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Juegos;
using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi
{
    public class ApiJuegoRespuesta : ApiRespuestaBase
    {
        [JsonProperty("juego")]
        public List<Juego>? juego { get; set; }
        
    }

    public class ApiJuegosRespuesta : ApiRespuestaBase
    {
        [JsonProperty("juegos")]
        public List<Juego>? juegos { get; set; }
    }
}
