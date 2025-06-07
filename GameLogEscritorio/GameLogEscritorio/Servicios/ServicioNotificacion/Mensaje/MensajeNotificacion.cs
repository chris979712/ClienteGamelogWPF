using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.ServicioNotificacion.Mensaje
{
    public class MensajeNotificacion
    {
        [JsonProperty("idResena")]
        public int idResena {  get; set; }
        [JsonProperty("mensaje")]
        public string? mensaje {  get; set; }
        [JsonProperty("accion")]
        public string? accion { get; set; }
        [JsonProperty("fecha")]
        public DateTime fecha {  get; set; }
    }
}
