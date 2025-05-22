using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Login
{
    public class PostRecuperacionDeCuentaValidacion
    {

        [JsonProperty("correo")]
        public string? correo { get; set; }

        [JsonProperty("codigo")]
        public int codigo { get; set; }

        [JsonProperty("tipoDeUsuario")]
        public string? tipoDeUsuario { get; set; }
    }
}
