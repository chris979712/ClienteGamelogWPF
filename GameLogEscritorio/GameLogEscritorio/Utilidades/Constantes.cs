namespace GameLogEscritorio.Utilidades
{
    public static class Constantes
    {
        public const string TipoInformacion = "Informacion";
        public const string TipoError = "Error";
        public const string TipoAdvertencia = "Advertencia";
        public const string TipoExito = "Exito";
        public const string TipoConfirmarAccion = "Confirmacion";
        public const int ErrorEnLaOperacion = -1;
        public const int OperacionExitosa = 1;
        public const int ValorPorDefecto = 0;
        public const string TituloExcepcionServidor = "Error de servidor";
        public const string ContenidoDatosInvalidos = "Los datos ingresados son inválidos, por favor verifique los campos marcados.";
        public const string ContenidoExcepcionServidor = "El servidor esta inactivo. Por favor inténtelo más tarde.";
        public const string TituloExcepcionBD = "Error de base de datos";
        public const string ContenidoExcepcionBD = "No hay conexión con la base de datos. Por favor, inténtelo más tarde.";
        public const int CodigoExito = 200;
        public const int CodigoErrorSolicitud = 400;
        public const int CodigoErrorServidor = 500;
        public const int CodigoErrorAcceso = 401;
        public const int CodigoSinResultadosEncontrados = 404;
        public const int CodigoBadGetaway = 502;
        public const int CodigoEstadoOKGRPC = 0;
        public const int CodigoArgumentosInvalidosGRPC = 3;
        public const int CodigoPermisosDenegadosGRPC = 7;
        public const int CodigoElementoNoEncontradoGRPC = 5;
        public const int CodigoServidorNoEncontradoGRPC = 12;
        public const int CodigoErrorInternoGRPC = 13;
        public const int CodigoServidorNoDisponibleGRPC = 14;
        public const string tipoJugadorPorDefecto = "Jugador";
        public const string TipoDeEstadoPorDefecto = "Desbaneado";
        public const string JuegoNoEncontradoRawg = "Not found.";
        public const string AccionSocialDarMeGusta = "Interactuar_resena";
        public const string AccionSocialAgregarSeguidor = "Agregar_seguidor";
        public const string AccionSocialEliminarSeguidor = "Eliminar_seguidor";
        public const string AccionSocialBanearUsuario = "Banear_usuario";
        public const string AccionResenaDarMeGusta = "Dar_MeGusta";
        public const string AccionResenaQuitarMeGusta = "Quitar_MeGusta";
        public const string AccionEliminarResena = "Eliminar_resena";
        public const string AccionInsertarResena = "Registar_resena";
    }
}
