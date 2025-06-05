using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Login;
using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Jugador
{
    public class Perfil : Cuenta
    {
        [JsonProperty("idJugador")]
        public int idJugador { get; set; }

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

        [JsonProperty("foto")]
        public string? foto { get; set; }
    }
}
