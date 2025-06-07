using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.APIRawg.Modelo
{
    public class JuegoModelo
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("detail")]
        public string? detail { get; set; }

        [JsonProperty("name")]
        public string? name { get; set; }

        [JsonProperty("released")]
        public string? released { get; set; }

        [JsonProperty("description")]
        public string? description { get; set; }

        [JsonProperty("rating")]
        public float rating { get; set; }

        [JsonProperty("background_image")]
        public string? backgroundImage { get; set; }

        [JsonProperty("platforms")]
        public List<PlataformaEnmascarada>? platforms { get; set; }

        [JsonProperty("redirect")]
        public bool redirect { get; set; }

        [JsonProperty("slug")]
        public string? slug { get; set; }
    }

    public class PlataformaEnmascarada
    {
        [JsonProperty("platform")]
        public Plataforma? platform { get; set; }
    }

    public class Plataforma
    {
        [JsonProperty("name")]
        public string? name { get; set; }
    }
}
