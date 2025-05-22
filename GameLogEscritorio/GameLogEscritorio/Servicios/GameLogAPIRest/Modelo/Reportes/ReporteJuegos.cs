using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Reportes
{
    public class ReporteJuegos
    {

        [JsonProperty("idCuenta")]
        public int idJuego {  get; set; }

        [JsonProperty("idCuenta")]
        public string? nombre { get; set; }

        [JsonProperty("totalReseñas")]
        public int totalReseñas { get; set; }

    }
}
