using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Acceso
{
    public class PutAccesoSolicitud
    {
        [JsonProperty("correo")]
        public string? correo {  get; set; }

        [JsonProperty("contrasenia")]
        public string? contrasenia { get; set; }

        [JsonProperty("tipoDeUsuario")]
        public string? tipoDeUsuario {  get; set; }  
    }
}
