using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi
{
    public class ApiAccesoRespuesta<T> : ApiRespuestaBase
    {
        public T? datos {  get; set; }
    }
}
