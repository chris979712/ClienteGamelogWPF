using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Ventanas;
using System.Windows;

namespace GameLogEscritorio.Utilidades
{
    public class ManejadorRespuestas
    {

        public static bool ManejarRespuestasBase(ApiRespuestaBase respuestaBase)
        {
            VentanaEmergente ventanaEmergente = null!;
            bool esEstadoCritico = false;
            switch (respuestaBase.estado)
            {
                case Constantes.CodigoExito:
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoExito,respuestaBase.mensaje!,respuestaBase.estado);
                    esEstadoCritico = false;
                    break;
                case Constantes.CodigoErrorSolicitud:
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoInformacion, respuestaBase.mensaje!, respuestaBase.estado);
                    esEstadoCritico = false;
                    break;
                case Constantes.CodigoErrorAcceso:
                    esEstadoCritico = true;
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuestaBase.mensaje!, respuestaBase.estado);
                    break;
                case Constantes.CodigoSinResultadosEncontrados:
                    esEstadoCritico = false;
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoInformacion, respuestaBase.mensaje!, respuestaBase.estado);
                    break;
                case Constantes.CodigoErrorServidor:
                    esEstadoCritico = false;
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuestaBase.mensaje!, respuestaBase.estado);
                    break;
                case Constantes.CodigoBadGetaway:
                    esEstadoCritico = false;
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuestaBase.mensaje!, respuestaBase.estado);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                    break;
            }
            AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
            return esEstadoCritico;
        }

        public static bool ManejarRespuestasConDatosODiferentesAlCodigoDeExito(ApiRespuestaBase respuestaApi)
        {
            VentanaEmergente ventanaEmergente;
            bool esEstadoCritico = false;
            switch (respuestaApi.estado)
            {
                case Constantes.CodigoExito:
                    esEstadoCritico = false;
                    break;
                case Constantes.CodigoErrorSolicitud:
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoInformacion, respuestaApi.mensaje!, respuestaApi.estado);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                    esEstadoCritico = false;
                    break;
                case Constantes.CodigoErrorAcceso:
                    esEstadoCritico = true;
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuestaApi.mensaje!, respuestaApi.estado);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                    break;
                case Constantes.CodigoSinResultadosEncontrados:
                    esEstadoCritico = false;
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoInformacion, respuestaApi.mensaje!, respuestaApi.estado);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                    break;
                case Constantes.CodigoErrorServidor:
                    esEstadoCritico = false;
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuestaApi.mensaje!, respuestaApi.estado);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                    break;
                case Constantes.CodigoBadGetaway:
                    esEstadoCritico = false;
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuestaApi.mensaje!, respuestaApi.estado);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                    break;
            }
            return esEstadoCritico;
        }

        public static bool ManejarRespuestasNotificacionDespachador(ApiRespuestaBase respuestaApi)
        {
            VentanaEmergente ventanaEmergente;
            bool esEstadoCritico = false;
            Application.Current.Dispatcher.Invoke(() =>
            {
                switch (respuestaApi.estado)
                {
                    case Constantes.CodigoExito:
                        esEstadoCritico = false;
                        break;
                    case Constantes.CodigoErrorSolicitud:
                        ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuestaApi.mensaje!, respuestaApi.estado);
                        AnimacionesVentana.MostrarVentanaEnCentroDeVentanaActualDespachador(ventanaEmergente);
                        esEstadoCritico = false;
                        break;
                    case Constantes.CodigoErrorAcceso:
                        esEstadoCritico = true;
                        ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuestaApi.mensaje!, respuestaApi.estado);
                        AnimacionesVentana.MostrarVentanaEnCentroDeVentanaActualDespachador(ventanaEmergente);
                        break;
                    case Constantes.CodigoSinResultadosEncontrados:
                        esEstadoCritico = false;
                        break;
                    case Constantes.CodigoErrorServidor:
                        esEstadoCritico = false;
                        ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuestaApi.mensaje!, respuestaApi.estado);
                        AnimacionesVentana.MostrarVentanaEnCentroDeVentanaActualDespachador(ventanaEmergente);
                        break;
                    case Constantes.CodigoBadGetaway:
                        esEstadoCritico = false;
                        ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuestaApi.mensaje!, respuestaApi.estado);
                        AnimacionesVentana.MostrarVentanaEnCentroDeVentanaActualDespachador(ventanaEmergente);
                        break;
                }
            });
            return esEstadoCritico;
        }

        public static void ManejarRespuestasGRPCDespachador(int codigoDeRespuesta)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                VentanaEmergente ventanaEmergente;
                switch (codigoDeRespuesta)
                {
                    case Constantes.CodigoArgumentosInvalidosGRPC:
                        ventanaEmergente = new VentanaEmergente(Constantes.TipoInformacion, Properties.Resources.GRPCArgumentosInvalidos, Constantes.CodigoErrorSolicitud);
                        AnimacionesVentana.MostrarVentanaEnCentroDeVentanaActualDespachador(ventanaEmergente);
                        break;
                    case Constantes.CodigoPermisosDenegadosGRPC:
                        ventanaEmergente = new VentanaEmergente(Constantes.TipoError, Properties.Resources.GRPCPermisosInvalidos, Constantes.CodigoErrorServidor);
                        AnimacionesVentana.MostrarVentanaEnCentroDeVentanaActualDespachador(ventanaEmergente);
                        break;
                    case Constantes.CodigoElementoNoEncontradoGRPC:
                        ventanaEmergente = new VentanaEmergente(Constantes.TipoInformacion, Properties.Resources.GRPElementosNoEncontrado, Constantes.CodigoErrorSolicitud);
                        AnimacionesVentana.MostrarVentanaEnCentroDeVentanaActualDespachador(ventanaEmergente);
                        break;
                    case Constantes.CodigoErrorInternoGRPC:
                        ventanaEmergente = new VentanaEmergente(Constantes.TipoError, Properties.Resources.GRPCErrorInterno, Constantes.CodigoErrorServidor);
                        AnimacionesVentana.MostrarVentanaEnCentroDeVentanaActualDespachador(ventanaEmergente);
                        break;
                    case Constantes.CodigoServidorNoDisponibleGRPC:
                        ventanaEmergente = new VentanaEmergente(Constantes.TipoInformacion, Properties.Resources.GRPCServidorNoDisponible, Constantes.CodigoErrorSolicitud);
                        AnimacionesVentana.MostrarVentanaEnCentroDeVentanaActualDespachador(ventanaEmergente);
                        break;
                    case Constantes.CodigoServidorNoEncontradoGRPC:
                        ventanaEmergente = new VentanaEmergente(Constantes.TipoInformacion, Properties.Resources.ServidorGRPCNoEncontrado, Constantes.CodigoErrorSolicitud);
                        AnimacionesVentana.MostrarVentanaEnCentroDeVentanaActualDespachador(ventanaEmergente);
                        break;
                }
            });
        }

        public static void ManejadorRespuestasGRPC(int codigoDeRespuesta)
        {
            VentanaEmergente ventanaEmergente = null!;
            switch (codigoDeRespuesta)
            {
                case Constantes.CodigoArgumentosInvalidosGRPC:
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoInformacion,Properties.Resources.GRPCArgumentosInvalidos,Constantes.CodigoErrorSolicitud);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                    break;
                case Constantes.CodigoPermisosDenegadosGRPC:
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoError,Properties.Resources.GRPCPermisosInvalidos, Constantes.CodigoErrorServidor);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                    break;
                case Constantes.CodigoElementoNoEncontradoGRPC:
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoInformacion,Properties.Resources.GRPElementosNoEncontrado, Constantes.CodigoErrorSolicitud);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                    break;
                case Constantes.CodigoErrorInternoGRPC:
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoError, Properties.Resources.GRPCErrorInterno, Constantes.CodigoErrorServidor);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                    break;
                case Constantes.CodigoServidorNoDisponibleGRPC:
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoInformacion,Properties.Resources.GRPCServidorNoDisponible, Constantes.CodigoErrorSolicitud);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                    break;
                case Constantes.CodigoServidorNoEncontradoGRPC:
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoInformacion, Properties.Resources.ServidorGRPCNoEncontrado, Constantes.CodigoErrorSolicitud);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                    break;
            }
            if(codigoDeRespuesta == Constantes.CodigoExito)
            {
                AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
            }
        }

    }
}
