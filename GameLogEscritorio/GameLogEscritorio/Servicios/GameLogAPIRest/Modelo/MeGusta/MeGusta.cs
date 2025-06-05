using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Likes
{
    public class MeGusta
    {

        [JsonProperty("idResenia")]
        public int idResenia {  get; set; }

        [JsonProperty("idJugador")]
        public int idJugador { get; set; }

    }
}
