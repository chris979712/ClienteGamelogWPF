using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Juegos
{
    public class PostJuegoPendienteSolicitud
    {
        [JsonProperty("idJugador")]
        public int idJugador { get; set; }

        [JsonProperty("idJuego")]
        public int idJuego { get; set; }
    }
}
