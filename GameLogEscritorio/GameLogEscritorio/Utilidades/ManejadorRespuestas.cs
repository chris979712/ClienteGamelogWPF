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
                    esEstadoCritico = true;
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuestaBase.mensaje!, respuestaBase.estado);
                    break;
            }
            AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaEmergente);
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
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaEmergente);
                    esEstadoCritico = false;
                    break;
                case Constantes.CodigoErrorAcceso:
                    esEstadoCritico = true;
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuestaApi.mensaje!, respuestaApi.estado);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaEmergente);
                    break;
                case Constantes.CodigoSinResultadosEncontrados:
                    esEstadoCritico = false;
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoInformacion, respuestaApi.mensaje!, respuestaApi.estado);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaEmergente);
                    break;
                case Constantes.CodigoErrorServidor:
                    esEstadoCritico = true;
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuestaApi.mensaje!, respuestaApi.estado);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaEmergente);
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
                        ventanaEmergente.Show();
                        esEstadoCritico = false;
                        break;
                    case Constantes.CodigoErrorAcceso:
                        esEstadoCritico = true;
                        ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuestaApi.mensaje!, respuestaApi.estado);
                        ventanaEmergente.Show();
                        break;
                    case Constantes.CodigoSinResultadosEncontrados:
                        esEstadoCritico = false;
                        break;
                    case Constantes.CodigoErrorServidor:
                        esEstadoCritico = true;
                        ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuestaApi.mensaje!, respuestaApi.estado);
                        ventanaEmergente.Show();
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
                        ventanaEmergente.Show();
                        break;
                    case Constantes.CodigoPermisosDenegadosGRPC:
                        ventanaEmergente = new VentanaEmergente(Constantes.TipoError, Properties.Resources.GRPCPermisosInvalidos, Constantes.CodigoErrorServidor);
                        break;
                    case Constantes.CodigoElementoNoEncontradoGRPC:
                        ventanaEmergente = new VentanaEmergente(Constantes.TipoInformacion, Properties.Resources.GRPElementosNoEncontrado, Constantes.CodigoErrorSolicitud);
                        ventanaEmergente.Show();
                        break;
                    case Constantes.CodigoErrorInternoGRPC:
                        ventanaEmergente = new VentanaEmergente(Constantes.TipoError, Properties.Resources.GRPCErrorInterno, Constantes.CodigoErrorServidor);
                        ventanaEmergente.Show();
                        break;
                    case Constantes.CodigoServidorNoDisponibleGRPC:
                        ventanaEmergente = new VentanaEmergente(Constantes.TipoInformacion, Properties.Resources.GRPCServidorNoDisponible, Constantes.CodigoErrorSolicitud);
                        ventanaEmergente.Show();
                        break;
                    case Constantes.CodigoServidorNoEncontradoGRPC:
                        ventanaEmergente = new VentanaEmergente(Constantes.TipoInformacion, Properties.Resources.ServidorGRPCNoEncontrado, Constantes.CodigoErrorSolicitud);
                        ventanaEmergente.Show();
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
                    break;
                case Constantes.CodigoPermisosDenegadosGRPC:
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoError,Properties.Resources.GRPCPermisosInvalidos, Constantes.CodigoErrorServidor);
                    break;
                case Constantes.CodigoElementoNoEncontradoGRPC:
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoInformacion,Properties.Resources.GRPElementosNoEncontrado, Constantes.CodigoErrorSolicitud);
                    break;
                case Constantes.CodigoErrorInternoGRPC:
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoError,Properties.Resources.GRPCErrorInterno, Constantes.CodigoErrorServidor);
                    break;
                case Constantes.CodigoServidorNoDisponibleGRPC:
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoInformacion,Properties.Resources.GRPCServidorNoDisponible, Constantes.CodigoErrorSolicitud);
                    break;
                case Constantes.CodigoServidorNoEncontradoGRPC:
                    ventanaEmergente = new VentanaEmergente(Constantes.TipoInformacion, Properties.Resources.ServidorGRPCNoEncontrado, Constantes.CodigoErrorSolicitud);
                    break;
            }
            if(codigoDeRespuesta == Constantes.CodigoExito)
            {
                AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(Estaticas.ultimoTopVentana, Estaticas.ultimoLeftVentana, Estaticas.ultimoWidthVentana, Estaticas.ultimoHeightVentana, ventanaEmergente);
            }
        }

    }
}
