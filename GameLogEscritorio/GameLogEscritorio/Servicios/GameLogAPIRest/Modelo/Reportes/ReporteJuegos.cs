using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Reportes
{
    public class ReporteJuegos
    {

        [JsonProperty("idJuego")]
        public int idJuego {  get; set; }

        [JsonProperty("nombre")]
        public string? nombre { get; set; }

        [JsonProperty("totalReseñas")]
        public int totalReseñas { get; set; }

    }
}
