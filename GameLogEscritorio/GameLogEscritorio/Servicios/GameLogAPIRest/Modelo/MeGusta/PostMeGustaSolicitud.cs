using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Likes
{
    public class PostMeGustaSolicitud
    {

        [JsonProperty("idResena")]
        public int idResena { get; set; }

        [JsonProperty("idJugador")]
        public int idJugador { get; set; }

        [JsonProperty("idJuego")]
        public int idJuego { get; set; }

        [JsonProperty("idJugadorAutor")]
        public int idJugadorAutor {  get; set; }

    }
}
