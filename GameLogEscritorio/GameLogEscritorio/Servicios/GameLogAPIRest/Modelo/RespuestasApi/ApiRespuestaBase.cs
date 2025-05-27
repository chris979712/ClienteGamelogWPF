using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse
{
    public class ApiRespuestaBase
    {

        [JsonProperty("error")]
        public bool error { get; set; }
        [JsonProperty("estado")]
        public int estado { get; set; }
        [JsonProperty("mensaje")]
        public string? mensaje { get; set; }
        [JsonProperty("resultado")]
        public int resultado { get; set; }

    }
}
