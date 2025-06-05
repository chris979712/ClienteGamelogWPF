using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Login
{
    public class PostLoginSolicitud
    {

        [JsonProperty("correo")]
        public string? correo { get; set; }

        [JsonProperty("contrasenia")]
        public string? contrasenia { get; set; }

        [JsonProperty("tipoDeUsuario")]
        public string? tipoDeUsuario { get; set; }

    }
}
