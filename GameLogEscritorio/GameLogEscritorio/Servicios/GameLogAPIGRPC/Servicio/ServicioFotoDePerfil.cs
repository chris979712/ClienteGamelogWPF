using Gamelog.Proto;
using GameLogEscritorio.Servicios.GameLogAPIGRPC.Respuesta;
using GameLogEscritorio.Utilidades;
using Grpc.Core;
using Grpc.Net.Client;
using System.Net.Http;

namespace GameLogEscritorio.Servicios.GameLogAPIGRPC.Servicio
{

    public static class ServicioFotoDePerfil
    {

        public static async Task<RespuestaGRPC> SubirFotoDePerfilUsuario(byte[] fotoDePerfil, string nombreDeUsuario)
        {
            RespuestaGRPC resultadoFotoDePerfil = new RespuestaGRPC();
            var canal = GrpcChannel.ForAddress(Properties.Resources.ApiGrpcUrl, new GrpcChannelOptions
            {
                HttpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                }
            });
            var cliente = new FotosDePerfil.FotosDePerfilClient(canal);
            try
            {
                FotoDePerfilDatos datosFotoDePerfil = new FotoDePerfilDatos()
                {
                    NombreDeUsuario = nombreDeUsuario,
                    Datos = Google.Protobuf.ByteString.CopyFrom(fotoDePerfil)
                };
                FotoDePerfilRuta respuesta = cliente.SubirFotoDeUsuario(datosFotoDePerfil);
                resultadoFotoDePerfil.codigo = Constantes.CodigoExito;
                resultadoFotoDePerfil.detalles = respuesta.RutaArchivo.ToString();
            }
            catch (HttpRequestException)
            {
                resultadoFotoDePerfil.detalles = Properties.Resources.HttpExcepcion;
                resultadoFotoDePerfil.codigo = Constantes.CodigoErrorServidor;
            }
            catch (RpcException excepcionRpc)
            {
                resultadoFotoDePerfil.detalles = excepcionRpc.Status.Detail;
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
            var canal = GrpcChannel.ForAddress(Properties.Resources.ApiGrpcUrl, new GrpcChannelOptions
            {
                HttpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                }
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
            catch (HttpRequestException)
            {
                resultadoFotoDePerfil.detalles = Properties.Resources.HttpExcepcion;
                resultadoFotoDePerfil.codigo = Constantes.CodigoErrorServidor;
            }
            catch (RpcException excepcionRpc)
            {
                resultadoFotoDePerfil.detalles = excepcionRpc.Status.Detail;
                resultadoFotoDePerfil.codigo = (int)excepcionRpc.Status.StatusCode;
            }
            finally
            {
                await canal.ShutdownAsync();
            }
            return resultadoFotoDePerfil;
        }

        public static async Task<RespuestaGRPC> ActualizarFotoDePerfilJugador(string rutaAntigua, string nombreDeUsuario, byte[] fotoDePerfil)
        {
            RespuestaGRPC resultadoFotoDePerfil = new RespuestaGRPC();
            var canal = GrpcChannel.ForAddress(Properties.Resources.ApiGrpcUrl, new GrpcChannelOptions
            {
                HttpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                }
            });
            var cliente = new FotosDePerfil.FotosDePerfilClient(canal);
            try
            {
                ActualizacionFotoDePerfil datosActualizacionDeFoto = new ActualizacionFotoDePerfil()
                {
                    NombreDeUsuario = nombreDeUsuario,
                    RutaImagenAntigua = rutaAntigua,
                    Datos = Google.Protobuf.ByteString.CopyFrom(fotoDePerfil)
                };
                FotoDePerfilRuta datosObtenidos = cliente.ActualizarFotoDePerfil(datosActualizacionDeFoto);
                resultadoFotoDePerfil.codigo = Constantes.CodigoExito;
                resultadoFotoDePerfil.detalles = datosObtenidos.RutaArchivo;
            }
            catch (HttpRequestException)
            {
                resultadoFotoDePerfil.detalles = Properties.Resources.HttpExcepcion;
                resultadoFotoDePerfil.codigo = Constantes.CodigoErrorServidor;
            }
            catch (RpcException excepcionRpc)
            {
                resultadoFotoDePerfil.detalles = excepcionRpc.Status.Detail;
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

