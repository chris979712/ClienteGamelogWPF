using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Seguidor;
using Newtonsoft.Json;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi
{
    public class ApiSeguidoresRespuesta : ApiRespuestaBase
    {
        [JsonProperty("seguidores")]
        public List<Seguidores>? jugadoresSeguidores {  get; set; }
    }

    public class ApiSeguidosRespuesta : ApiRespuestaBase
    {
        [JsonProperty("seguidos")]
        public List<Seguido>? jugadoresSeguidos { get; set; }
    }
}
