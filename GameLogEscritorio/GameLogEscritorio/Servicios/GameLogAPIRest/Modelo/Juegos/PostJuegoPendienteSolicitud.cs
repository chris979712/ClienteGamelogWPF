using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Juegos
{
    public class PostJuegoPendienteSolicitud
    {
        [JsonProperty("idJugador")]
        public int idJugador { get; set; }

        [JsonProperty("idJuego")]
        public int idJuego { get; set; }
    }
}
