using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.MeGusta
{
    public class DeleteMeGustaSolicitud
    {

        [JsonProperty("idResena")]
        public int idResena { get; set; }

        [JsonProperty("idJugadorAutor")]
        public int idJugadorAutor {  get; set; }

        [JsonProperty("idJugador")]
        public int idJugador {  get; set; }

        [JsonProperty("nombreJuego")]
        public string? nombreJuego { get; set; }

    }
}
