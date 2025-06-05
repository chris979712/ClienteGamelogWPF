using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Reseñas
{
    public class Reseña
    {

        [JsonProperty("idJuego")]
        public int idJuego { get; set; }

        [JsonProperty("idJugador")]
        public int idJugador { get; set; }

        [JsonProperty("calificacion")]
        public decimal calificacion { get; set; }

        [JsonProperty("opinion")]
        public string? opinion { get; set; }

    }

    public class ReseñaPersonal
    {

        [JsonProperty("idResenia")]
        public int idResenia { get; set; }

        [JsonProperty("idJugador")]
        public int idJugador { get; set; }

        [JsonProperty("idJuego")]
        public int idJuego { get; set; }

        [JsonProperty("nombre")]
        public string? nombre { get; set; }

        [JsonProperty("fecha")]
        public string? fecha { get; set; }

        [JsonProperty("opinion")]
        public string? opinion { get; set; }

        [JsonProperty("calificacion")]
        public decimal calificacion { get; set; }

        [JsonProperty("totalDeMeGusta")]
        public int totalDeMeGusta { get; set; }

        [JsonProperty("existeMeGusta")]
        public bool existeMeGusta { get; set; }

    }

    public class ReseñaJugadores
    {

        [JsonProperty("idResenia")]
        public int idResenia { get; set; }

        [JsonProperty("idJugador")]
        public int idJugador { get; set; }

        [JsonProperty("nombreDeUsuario")]
        public string? nombreDeUsuario { get; set; }

        [JsonProperty("foto")]
        public string? foto { get; set; }

        [JsonProperty("idJuego")]
        public int idJuego { get; set; }

        [JsonProperty("nombre")]
        public string? nombre { get; set; }

        [JsonProperty("fecha")]
        public string? fecha { get; set; }

        [JsonProperty("opinion")]
        public string? opinion { get; set; }

        [JsonProperty("calificacion")]
        public decimal calificacion { get; set; }

        [JsonProperty("totalDeMeGusta")]
        public int totalDeMeGusta { get; set; }

        [JsonProperty("existeMeGusta")]
        public bool existeMeGusta { get; set; }

    }
}
