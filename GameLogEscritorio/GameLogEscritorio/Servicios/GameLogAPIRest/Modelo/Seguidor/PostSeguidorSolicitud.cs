using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Social
{
    public class PostSeguidorSolicitud
    {

        [JsonProperty("idJugadorSeguidor")]
        public int idJugadorSeguidor {  get; set; }

        [JsonProperty("idJugadorSeguido")]
        public int idJugadorSeguido { get; set; }

    }
}
