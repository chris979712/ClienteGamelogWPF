using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Jugador;

namespace GameLogEscritorio.Utilidades
{
    public class UsuarioSingleton
    {
        private static UsuarioSingleton? _instancia;
        private static readonly object _lock = new object();    

        public int idCuenta {  get; private set; }
        public string? correo {  get; private set; }
        public string? estado { get; private set; }
        public string? tipoDeAcceso { get; private set; }
        public int idJugador { get; private set; }
        public string? nombre { get; set; }
        public string? primerApellido { get; set; }
        public string? segundoApellido { get; set; }
        public string? nombreDeUsuario {  get; set; }
        public string? descripcion {  get; set; }
        public string? foto { get; set; }
        public byte[]? fotoDePerfil { get; set; }

        private UsuarioSingleton() { }

        public static UsuarioSingleton Instancia
        {
            get
            {
                lock(_lock )
                {
                    if(_instancia == null)
                    {
                        _instancia = new UsuarioSingleton()!;
                    }
                    return _instancia;
                }
            }
        }

        public void IniciarSesion(Perfil cuenta)
        {
            idCuenta = cuenta.idCuenta;
            idJugador = cuenta.idJugador;
            estado = cuenta.estado;
            tipoDeAcceso = cuenta.tipoDeAcceso;
            nombre = cuenta.nombre;
            primerApellido = cuenta.primerApellido;
            segundoApellido = cuenta.segundoApellido;
            nombreDeUsuario = cuenta.nombreDeUsuario;
            correo = cuenta.correo;
            descripcion = cuenta.descripcion;
            foto = cuenta.foto;
        }

        public void CerrarSesion()
        {
            _instancia = null;
        }
    }
}
