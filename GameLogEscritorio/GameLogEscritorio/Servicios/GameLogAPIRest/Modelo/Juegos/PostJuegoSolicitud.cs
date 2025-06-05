using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Juegos
{
    public class PostJuegoSolicitud
    {
        [JsonProperty("idJuego")]
        public int idJuego {  get; set; }

        [JsonProperty("nombre")]
        public string? nombre { get; set; }

        [JsonProperty("fechaDeLanzamiento")]
        public string? fechaDeLanzamiento { get; set; }
    }
}
