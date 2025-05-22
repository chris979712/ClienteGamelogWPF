using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Reseñas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi
{
    public class ApiReseñaPersonalRespuesta : ApiRespuestaBase
    {
        [JsonProperty("reseñas")]
        public List<ReseñaPersonal>? reseñasPersonales {  get; set; }
    }

    public class ApiReseñaJugadoresRespuesta : ApiRespuestaBase
    {
        [JsonProperty("reseñas")]
        public List<ReseñaJugadores>? reseñaJugadores { get; set; }
    }
}
