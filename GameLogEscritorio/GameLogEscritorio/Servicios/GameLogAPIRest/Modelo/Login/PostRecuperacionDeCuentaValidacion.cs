using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Login
{
    public class PostRecuperacionDeCuentaValidacion
    {

        [JsonProperty("correo")]
        public string? correo { get; set; }

        [JsonProperty("codigo")]
        public int codigo { get; set; }

        [JsonProperty("tipoDeUsuario")]
        public string? tipoDeUsuario { get; set; }
    }
}
