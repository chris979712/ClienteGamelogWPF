using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Seguidor
{
    public class Seguidores
    {

        [JsonProperty("nombre")]
        public string? nombre { get; set; }

        [JsonProperty("primerApellido")]
        public string? primerApellido { get; set; }

        [JsonProperty("segundoApellido")]
        public string? segundoApellido { get; set; }

        [JsonProperty("nombreDeUsuario")]
        public string? nombreDeUsuario { get; set; }

        [JsonProperty("descripcion")]
        public string? descripcion { get; set; }

        [JsonProperty("foro")]
        public string? foro { get; set; }

        [JsonProperty("idJugador")]
        public int idJugador { get; set; }

    }

    public class Seguido
    {

        [JsonProperty("nombre")]
        public string? nombre { get; set; }

        [JsonProperty("primerApellido")]
        public string? primerApellido { get; set; }

        [JsonProperty("segundoApellido")]
        public string? segundoApellido { get; set; }

        [JsonProperty("nombreDeUsuario")]
        public string? nombreDeUsuario { get; set; }

        [JsonProperty("descripcion")]
        public string? descripcion { get; set; }

        [JsonProperty("foro")]
        public string? foro { get; set; }

        [JsonProperty("idJugador")]
        public int idJugador { get; set; }

    }
}
