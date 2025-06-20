﻿using GameLogEscritorio.Servicios.APIRawg.Modelo;

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
