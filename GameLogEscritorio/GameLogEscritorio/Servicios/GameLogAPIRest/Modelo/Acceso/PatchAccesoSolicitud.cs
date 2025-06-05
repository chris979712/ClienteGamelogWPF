using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Acceso
{
    public class PatchAccesoSolicitud
    {
        [JsonProperty("estadoAcceso")]
        public string? estadoAcceso {  get; set; }
    }
}
