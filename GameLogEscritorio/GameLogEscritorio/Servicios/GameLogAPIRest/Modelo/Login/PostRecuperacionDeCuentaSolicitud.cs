using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Login
{
    public class PostRecuperacionDeCuentaSolicitud
    {

        [JsonProperty("correo")]
        public string? correo {  get; set; }

        [JsonProperty("tipoDeUsuario")]
        public string? tipoDeUsuario { get; set; }

    }
}
