using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Acceso
{
    public class PostAccesoSolicitud
    {
        [JsonProperty("correo")]
        public string? correo { get; set; }

        [JsonProperty("contrasenia")]
        public string? contrasenia { get; set; }

        [JsonProperty("estado")]
        public string? estado { get; set; }

        [JsonProperty("nombre")]
        public string? nombre { get; set; }

        [JsonProperty("primerApellido")]
        public string? primerApellido { get; set; }

        [JsonProperty("segundoApellido")]
        public string? segundoApellido { get; set; }

        [JsonProperty("nombreDeUsuario")]
        public string? nombreDeUsuario { get; set; }

        [JsonProperty("descripcion")]
        public string? descripcion { get; set; }

        [JsonProperty("foto")]
        public string? foto { get; set; }

        [JsonProperty("tipoDeUsuario")]
        public string? tipoDeUsuario { get; set; }
    }

}
