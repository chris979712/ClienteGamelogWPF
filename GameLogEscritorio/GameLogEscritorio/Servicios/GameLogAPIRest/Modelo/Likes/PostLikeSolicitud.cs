using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Likes
{
    public class PostLikeSolicitud
    {

        [JsonProperty("idResena")]
        public int idResena { get; set; }

        [JsonProperty("idJugador")]
        public int idJugador { get; set; }

    }
}
