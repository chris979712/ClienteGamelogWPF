using GameLogEscritorio.Ventanas;
using System.IO;


namespace GameLogEscritorio.Utilidades
{
    public static class FotoPorDefecto
    {

        public static byte[] ObtenerFotoDePerfilPorDefecto()
        {
            byte[] imagenPorDefectoBytes = new byte[0];
            try
            {
                string rutaBase = AppDomain.CurrentDomain.BaseDirectory;
                string rutaImagen = Path.Combine(rutaBase, "../../../Imagenes/imagendeperfildefaultgamelog.png");
                imagenPorDefectoBytes  = File.ReadAllBytes(rutaImagen);
            }
            catch(FileNotFoundException)
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, "No se ha podido obtener la foto de perfil por defecto", Constantes.CodigoErrorSolicitud);
            }
            return imagenPorDefectoBytes;
        }

    }
}
