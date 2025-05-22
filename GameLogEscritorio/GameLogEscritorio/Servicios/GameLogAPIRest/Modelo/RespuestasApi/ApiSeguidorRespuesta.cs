using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Seguidor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
