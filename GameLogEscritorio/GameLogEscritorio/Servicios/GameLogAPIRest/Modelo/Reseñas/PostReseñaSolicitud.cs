using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Reseñas
{
    public class PostReseñaSolicitud
    {

        [JsonProperty("idJuego")]
        public int idJuego { get; set; }

        [JsonProperty("idJugador")]
        public int idJugador { get; set; }

        [JsonProperty("calificacion")]
        public Decimal calificacion { get; set; }

        [JsonProperty("opinion")]
        public string? opinion { get; set; }

    }
}
