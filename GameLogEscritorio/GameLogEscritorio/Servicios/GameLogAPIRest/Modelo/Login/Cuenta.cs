using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Login
{
    public class Cuenta
    {

        [JsonProperty("idCuenta")]
        public int idCuenta { get; set; }

        [JsonProperty("correo")]
        public string? correo { get; set; }

        [JsonProperty("estado")]
        public string? estado { get; set; }

        [JsonProperty("tipoDeAcceso")]
        public string? tipoDeAcceso { get; set; }

    }
}
