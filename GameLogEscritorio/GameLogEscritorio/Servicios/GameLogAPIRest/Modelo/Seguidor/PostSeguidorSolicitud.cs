using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Seguidor
{
    public class PostSeguidorSolicitud
    {

        [JsonProperty("idJugadorSeguidor")]
        public int idJugadorSeguidor {  get; set; }

        [JsonProperty("idJugadorSeguido")]
        public int idJugadorSeguido { get; set; }

    }
}
