using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GameLogEscritorio.Servicios.GameLogAPIGRPC.Respuesta;
using GameLogEscritorio.Utilidades;
using Gamelog.Proto;
using Grpc.Core;

namespace GameLogEscritorio.Servicios.GameLogAPIGRPC.Servicio
{

    public static class ServicioFotoDePerfil
    {

        public static async Task<RespuestaGRPC> SubirFotoDePerfilUsuario(byte[] fotoDePerfil, string nombreDeUsuario)
        {
            RespuestaGRPC resultadoFotoDePerfil = new RespuestaGRPC();
            AppContext.SetSwitch(Properties.Resources.ConfiguracionCanalGRPC, true);
            var canal = new Channel(Properties.Resources.ApiGrpcUrl, ChannelCredentials.Insecure);
            var cliente = new FotosDePerfil.FotosDePerfilClient(canal);
            try
            {
                FotoDePerfilDatos datosFotoDePerfil = new FotoDePerfilDatos()
                {
                    NombreDeUsuario = nombreDeUsuario,
                    Datos = Google.Protobuf.ByteString.CopyFrom(fotoDePerfil)
                };
                FotoDePerfilRuta respuesta = await cliente.SubirFotoDeUsuarioAsync(datosFotoDePerfil);
                resultadoFotoDePerfil.codigo = Constantes.CodigoExito;
                resultadoFotoDePerfil.detalles = respuesta.RutaArchivo.ToString();
                await canal.ShutdownAsync();
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
            return resultadoFotoDePerfil;
        }

        public static async Task<RespuestaGRPC> ObtenerFotoJugador(string rutaFoto)
        {
            RespuestaGRPC resultadoFotoDePerfil = new RespuestaGRPC();
            AppContext.SetSwitch(Properties.Resources.ConfiguracionCanalGRPC, true);
            var canal = new Channel(Properties.Resources.ApiGrpcUrl, ChannelCredentials.Insecure);
            var cliente = new FotosDePerfil.FotosDePerfilClient(canal);
            try
            {
                FotoDePerfilRuta rutaFotoDePerfil = new FotoDePerfilRuta()
                {
                    RutaArchivo = rutaFoto
                };
                FotoDePerfilDatos datosObtenidos = await cliente.ObtenerFotoDePerfilUsuarioAsync(rutaFotoDePerfil);
                resultadoFotoDePerfil.codigo = Constantes.CodigoExito;
                resultadoFotoDePerfil.detalles = datosObtenidos.Datos.ToString();
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
            return resultadoFotoDePerfil;
        }

        public static async Task<RespuestaGRPC> ActualizarFotoDePerfilJugador(string rutaAntigua, string nombreDeUsuario, byte[] fotoDePerfil)
        {
            RespuestaGRPC resultadoFotoDePerfil = new RespuestaGRPC();
            AppContext.SetSwitch(Properties.Resources.ConfiguracionCanalGRPC, true);
            var canal = new Channel(Properties.Resources.ApiGrpcUrl, ChannelCredentials.Insecure);
            var cliente = new FotosDePerfil.FotosDePerfilClient(canal);
            try
            {
                ActualizacionFotoDePerfil datosActualizacionDeFoto = new ActualizacionFotoDePerfil()
                {
                    NombreDeUsuario = nombreDeUsuario,
                    RutaImagenAntigua = rutaAntigua,
                    Datos = Google.Protobuf.ByteString.CopyFrom(fotoDePerfil)
                };
                FotoDePerfilRuta datosObtenidos = await cliente.ActualizarFotoDePerfilAsync(datosActualizacionDeFoto);
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
            return resultadoFotoDePerfil;
        }

    }
}

