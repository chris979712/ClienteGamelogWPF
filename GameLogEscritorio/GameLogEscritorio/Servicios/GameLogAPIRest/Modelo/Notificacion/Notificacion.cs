using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Notificacion
{
    public class Notificaciones
    {

        [JsonProperty("idNotificacion")]
        public int idNotificacion {  get; set; }

        [JsonProperty("idJugadorNotificado")]
        public int idJugadorNotificado { get; set; }

        [JsonProperty("idJugadorNotificante")]
        public int idJugadorNotificante { get; set; }

        [JsonProperty("mensajeNotificacion")]
        public string? mensajeNotificacion { get; set; }

        [JsonProperty("fechaNotificacion")]
        public string? fechaNotificacion { get; set; }

    }
}
