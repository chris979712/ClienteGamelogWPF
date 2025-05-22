using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Ventanas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogEscritorio.Utilidades
{
    public class ManejadorRespuestas
    {

        public static bool ManejarRespuestasBase(ApiRespuestaBase respuestaBase)
        {
            bool esEstadoCritico = false;
            switch (respuestaBase.estado)
            {
                case Constantes.CodigoExito:
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoExito,respuestaBase.mensaje!,respuestaBase.estado);
                    esEstadoCritico = false;
                    break;
                case Constantes.CodigoErrorSolicitud:
                    VentanaEmergente ventanaEmergenteErrorSolicitud = new VentanaEmergente(Constantes.TipoInformacion, respuestaBase.mensaje!, respuestaBase.estado);
                    esEstadoCritico = false;
                    break;
                case Constantes.CodigoErrorAcceso:
                    esEstadoCritico = true;
                    VentanaEmergente ventanaEmergenteSinAcceso = new VentanaEmergente(Constantes.TipoError, respuestaBase.mensaje!, respuestaBase.estado);
                    break;
                case Constantes.CodigoSinResultadosEncontrados:
                    esEstadoCritico = false;
                    VentanaEmergente ventanaEmergenteSinResultadosEncontrados = new VentanaEmergente(Constantes.TipoInformacion, respuestaBase.mensaje!, respuestaBase.estado);
                    break;
                case Constantes.CodigoErrorServidor:
                    esEstadoCritico = false;
                    VentanaEmergente ventanaEmergenteErrorServidor = new VentanaEmergente(Constantes.TipoError, respuestaBase.mensaje!, respuestaBase.estado);
                    break;
            }
            return esEstadoCritico;
        }

        public static bool ManejarRespuestasConDatosODiferentesAlCodigoDeExito(ApiRespuestaBase respuestaApi)
        {
            bool esEstadoCritico = false;
            switch (respuestaApi.estado)
            {
                case Constantes.CodigoExito:
                    esEstadoCritico = false;
                    break;
                case Constantes.CodigoErrorSolicitud:
                    VentanaEmergente ventanaEmergenteErrorSolicitud = new VentanaEmergente(Constantes.TipoInformacion, respuestaApi.mensaje!, respuestaApi.estado);
                    esEstadoCritico = false;
                    break;
                case Constantes.CodigoErrorAcceso:
                    esEstadoCritico = true;
                    VentanaEmergente ventanaEmergenteSinAcceso = new VentanaEmergente(Constantes.TipoError, respuestaApi.mensaje!, respuestaApi.estado);
                    break;
                case Constantes.CodigoSinResultadosEncontrados:
                    esEstadoCritico = false;
                    VentanaEmergente ventanaEmergenteSinResultadosEncontrados = new VentanaEmergente(Constantes.TipoInformacion, respuestaApi.mensaje!, respuestaApi.estado);
                    break;
                case Constantes.CodigoErrorServidor:
                    esEstadoCritico = false;
                    VentanaEmergente ventanaEmergenteErrorServidor = new VentanaEmergente(Constantes.TipoError, respuestaApi.mensaje!, respuestaApi.estado);
                    break;
            }
            return esEstadoCritico;
        }

    }
}
