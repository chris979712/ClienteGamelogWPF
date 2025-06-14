using Gamelog.Proto;
using GameLogEscritorio.Log4net;
using GameLogEscritorio.Servicios.GameLogAPIGRPC.Respuesta;
using GameLogEscritorio.Utilidades;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Net;
using System.Net.Http;

namespace GameLogEscritorio.Servicios.GameLogAPIGRPC.Servicio
{

    public static class ServicioFotoDePerfil
    {

        public static async Task<RespuestaGRPC> SubirFotoDePerfilUsuario(byte[] fotoDePerfil, int idJugador)
        {
            RespuestaGRPC resultadoFotoDePerfil = new RespuestaGRPC();
            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    return true;
                }
            };
            var canal = GrpcChannel.ForAddress(Properties.Resources.ApiGrpcUrl, new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            });
            var cliente = new FotosDePerfil.FotosDePerfilClient(canal);
            try
            {
                FotoDePerfilDatos datosFotoDePerfil = new FotoDePerfilDatos()
                {
                    IdJugador = idJugador.ToString(),
                    Datos = Google.Protobuf.ByteString.CopyFrom(fotoDePerfil)
                };
                FotoDePerfilRuta respuesta = cliente.SubirFotoDeUsuario(datosFotoDePerfil);
                resultadoFotoDePerfil.codigo = Constantes.CodigoExito;
                resultadoFotoDePerfil.detalles = respuesta.RutaArchivo.ToString();
            }
            catch (HttpRequestException excepcion)
            {
                LoggerManejador.Error(excepcion.Message);
                resultadoFotoDePerfil.detalles = Properties.Resources.HttpExcepcion;
                resultadoFotoDePerfil.codigo = Constantes.CodigoErrorServidor;
            }
            catch (RpcException excepcionRpc)
            {
                LoggerManejador.Error(excepcionRpc.Message);
                resultadoFotoDePerfil.detalles = Properties.Resources.GRPCExcepcion;
                resultadoFotoDePerfil.codigo = (int)excepcionRpc.Status.StatusCode;
            }
            finally
            {
                await canal.ShutdownAsync();
            }
            return resultadoFotoDePerfil;
        }

        public static async Task<RespuestaGRPC> ObtenerFotoJugador(string rutaFoto)
        {
            RespuestaGRPC resultadoFotoDePerfil = new RespuestaGRPC();
            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    return true;
                }
            };
            var canal = GrpcChannel.ForAddress(Properties.Resources.ApiGrpcUrl, new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            });
            var cliente = new FotosDePerfil.FotosDePerfilClient(canal);
            try
            {
                FotoDePerfilRuta rutaFotoDePerfil = new FotoDePerfilRuta()
                {
                    RutaArchivo = rutaFoto
                };
                FotoDePerfilDatos datosObtenidos = cliente.ObtenerFotoDePerfilUsuario(rutaFotoDePerfil);
                resultadoFotoDePerfil.codigo = Constantes.CodigoExito;
                resultadoFotoDePerfil.datosBinario = datosObtenidos.Datos.ToByteArray();
            }
            catch (HttpRequestException excepcion)
            {
                LoggerManejador.Error(excepcion.Message);
                resultadoFotoDePerfil.detalles = Properties.Resources.HttpExcepcion;
                resultadoFotoDePerfil.codigo = Constantes.CodigoErrorServidor;
            }
            catch (RpcException excepcionRpc)
            {
                LoggerManejador.Error(excepcionRpc.Message);
                resultadoFotoDePerfil.detalles = Properties.Resources.GRPCExcepcion;
                resultadoFotoDePerfil.codigo = (int)excepcionRpc.Status.StatusCode;
            }
            finally
            {
                await canal.ShutdownAsync();
            }
            return resultadoFotoDePerfil;
        }

        public static async Task<RespuestaGRPC> ActualizarFotoDePerfilJugador(string rutaAntigua, int idJugador, byte[] fotoDePerfil)
        {
            RespuestaGRPC resultadoFotoDePerfil = new RespuestaGRPC();
            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    return true;
                }
            };
            var canal = GrpcChannel.ForAddress(Properties.Resources.ApiGrpcUrl, new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            });
            var cliente = new FotosDePerfil.FotosDePerfilClient(canal);
            try
            {
                ActualizacionFotoDePerfil datosActualizacionDeFoto = new ActualizacionFotoDePerfil()
                {
                    IdJugador = idJugador.ToString(),
                    RutaImagenAntigua = rutaAntigua,
                    Datos = Google.Protobuf.ByteString.CopyFrom(fotoDePerfil)
                };
                FotoDePerfilRuta datosObtenidos = cliente.ActualizarFotoDePerfil(datosActualizacionDeFoto);
                resultadoFotoDePerfil.codigo = Constantes.CodigoExito;
                resultadoFotoDePerfil.detalles = datosObtenidos.RutaArchivo;
            }
            catch (HttpRequestException excepcion)
            {
                LoggerManejador.Error(excepcion.Message);
                resultadoFotoDePerfil.detalles = Properties.Resources.HttpExcepcion;
                resultadoFotoDePerfil.codigo = Constantes.CodigoErrorServidor;
            }
            catch (RpcException excepcionRpc)
            {
                LoggerManejador.Error(excepcionRpc.Message);
                resultadoFotoDePerfil.detalles = Properties.Resources.GRPCExcepcion;
                resultadoFotoDePerfil.codigo = (int)excepcionRpc.Status.StatusCode;
            }
            finally
            {
                await canal.ShutdownAsync();
            }
            return resultadoFotoDePerfil;
        }

    }
}

