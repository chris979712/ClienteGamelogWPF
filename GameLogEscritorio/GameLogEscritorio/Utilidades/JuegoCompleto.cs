using GameLogEscritorio.Servicios.APIRawg.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Utilidades
{
    public class JuegoCompleto
    {

        public int idJuego { get; set; }

        public string? nombre { get; set; }

        public string? fechaLanzamiento { get; set; }

        public string? descripcion { get; set; }

        public float rating { get; set; }

        public string? imagenFondo { get; set; }

        public List<PlataformaEnmascarada>? platforms { get; set; }

    }
}
