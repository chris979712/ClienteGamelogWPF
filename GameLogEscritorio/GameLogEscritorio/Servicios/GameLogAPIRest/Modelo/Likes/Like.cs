using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Likes
{
    public class Like
    {

        [JsonProperty("idResenia")]
        public int idResenia {  get; set; }

        [JsonProperty("idJugador")]
        public int idJugador { get; set; }

    }
}
