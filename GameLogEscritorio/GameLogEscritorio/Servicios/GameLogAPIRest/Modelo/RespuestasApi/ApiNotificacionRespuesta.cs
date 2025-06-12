using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Notificacion;
using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi
{
    public class ApiNotificacionRespuesta : ApiRespuestaBase
    {

        [JsonProperty("notificaciones")]
        public List<Notificaciones>? notificaciones {  get; set; }

    }
}
