using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.GameLogAPIGRPC.Respuesta
{
    public class RespuestaGRPC
    {
        public int codigo {  get; set; }

        public string? detalles { get; set; }

        public byte[]? datosBinario {  get; set; }   
    }
}
