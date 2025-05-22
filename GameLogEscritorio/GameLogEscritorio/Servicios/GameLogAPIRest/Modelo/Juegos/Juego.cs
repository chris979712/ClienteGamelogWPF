using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Juegos
{
    public class Juego
    {

        [JsonProperty("idJuego")]
        public int idJuego {  get; set; }

        [JsonProperty("nombre")]
        public string? nombre { get; set; }

        [JsonProperty("fechaDeLanzamiento")]
        public string? fechaDeLanzamiento { get; set; }
    }
}
