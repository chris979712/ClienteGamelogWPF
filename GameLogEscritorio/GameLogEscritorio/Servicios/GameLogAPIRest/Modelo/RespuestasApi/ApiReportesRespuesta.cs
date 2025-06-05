using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Reportes;
using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi
{
    public class ApiReportesRespuesta : ApiRespuestaBase
    {
        [JsonProperty("juegos")]
        public List<ReporteJuegos>? juegos {  get; set; }
    }
}
