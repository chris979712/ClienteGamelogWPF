using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Juegos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi
{
    public class ApiJuegoRespuesta : ApiRespuestaBase
    {
        [JsonProperty("juego")]
        public List<Juego>? juego { get; set; }
        
    }

    public class ApiJuegosRespuesta : ApiRespuestaBase
    {
        [JsonProperty("juegos")]
        public List<Juego>? juegos { get; set; }
    }
}
