using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Acceso
{
    public class PatchAccesoSolicitud
    {
        [JsonProperty("estadoAcceso")]
        public string? estadoAcceso {  get; set; }
    }
}
