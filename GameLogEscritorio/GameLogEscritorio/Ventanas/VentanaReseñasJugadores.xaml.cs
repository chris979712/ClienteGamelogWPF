using GameLogEscritorio.Servicios.APIRawg.Modelo;
using GameLogEscritorio.Servicios.GameLogAPIGRPC.Respuesta;
using GameLogEscritorio.Servicios.GameLogAPIGRPC.Servicio;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Likes;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.MeGusta;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Reseñas;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Utilidades;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace GameLogEscritorio.Ventanas
{
    
    public partial class VentanaReseñasJugadores : Window
    {

        private readonly IApiRestRespuestaFactory apiRestCreadorRespuesta = new FactoryRespuestasAPI();
        private JuegoModelo _modeloJuego = new JuegoModelo();
        public static ObservableCollection<ReseñaCompleta> Reseñas { get; set; } = new ObservableCollection<ReseñaCompleta>();
        public bool EsAdministrador => UsuarioSingleton.Instancia.tipoDeAcceso == "Administrador";

        public VentanaReseñasJugadores(JuegoModelo modeloJuego)
        {
            this._modeloJuego = modeloJuego;
            InitializeComponent();
            this.DataContext = this;
            Estaticas.GuardarMedidasUltimaVentana(this);
        }

        private async void MostrarTodos_Click(object sender, RoutedEventArgs e)
        {
            Reseñas.Clear();
            ApiReseñaJugadoresRespuesta reseñasJugadores = await ServicioReseña.ObtenerReseñasDeUnJuego(_modeloJuego.id, UsuarioSingleton.Instancia.idJugador,apiRestCreadorRespuesta);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasConDatosODiferentesAlCodigoDeExito(reseñasJugadores);
            if (!esRespuestaCritica)
            {
                if(reseñasJugadores.estado == Constantes.CodigoExito)
                {
                    await CargarReseñasObtenidas(reseñasJugadores.reseñaJugadores!);
                }
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                this.Close();
            }
        }

        private async void MostrarSeguidos_Click(object sender, RoutedEventArgs e)
        {
            Reseñas.Clear();
            ApiReseñaJugadoresRespuesta reseñasJugadores = await ServicioReseña.ObtenerReseñasDeJugadoresSeguidosEnUnJuego(_modeloJuego.id, UsuarioSingleton.Instancia.idJugador,apiRestCreadorRespuesta);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasConDatosODiferentesAlCodigoDeExito(reseñasJugadores);
            if (!esRespuestaCritica)
            {
                if (reseñasJugadores.estado == Constantes.CodigoExito)
                {
                    await CargarReseñasObtenidas(reseñasJugadores.reseñaJugadores!);
                }
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                this.Close();
            }
        }

        private async Task CargarReseñasObtenidas(List<ReseñaJugadores> reseñasObtenidas)
        {
            Reseñas.Clear();
            foreach (var reseña in reseñasObtenidas)
            {
                ReseñaCompleta reseñaCompleta = new ReseñaCompleta()
                {
                    idJuego = reseña.idJuego,
                    idJugador = reseña.idJugador,
                    idResenia = reseña.idResenia,
                    opinion = reseña.opinion,
                    fecha = reseña.fecha,
                    calificacion = reseña.calificacion,
                    totalDeMeGustaReseña = reseña.totalDeMeGusta,
                    existeMeGustaReseña = reseña.existeMeGusta,
                    nombreDeUsuario = reseña.nombreDeUsuario,
                    nombre = reseña.nombre,
                    foto = reseña.foto,
                    totalDeMeGusta = reseña.totalDeMeGusta,
                    existeMeGusta = reseña.existeMeGusta,
                    fotoJugador = await CargarFotoDePerfilUsuario(reseña.foto!)
                };
                Reseñas.Add(reseñaCompleta);
            }
        }

        private async Task<byte[]> CargarFotoDePerfilUsuario(string rutaFoto)
        {
            byte[] fotoEncontrada = FotoPorDefecto.ObtenerFotoDePerfilPorDefecto();
            RespuestaGRPC respuestaGRPC = await ServicioFotoDePerfil.ObtenerFotoJugador(rutaFoto);
            if (respuestaGRPC.codigo == Constantes.CodigoExito)
            {
                fotoEncontrada = respuestaGRPC.datosBinario!;
            }
            return fotoEncontrada;
        }

        private async void EliminarReseña_Click(object sender, RoutedEventArgs e)
        {
            var boton = sender as Button;
            var reseña = boton?.DataContext as ReseñaCompleta;
            if(reseña != null)
            {
                ApiRespuestaBase apiRespuestaBase = await ServicioReseña.EliminarReseña(reseña.idJuego,reseña.idResenia,apiRestCreadorRespuesta);
                bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasBase(apiRespuestaBase);
                if(!esRespuestaCritica)
                {
                    if(apiRespuestaBase.estado == Constantes.CodigoExito)
                    {
                        Reseñas.Remove(reseña);
                    }
                }
                else
                {
                    await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                    this.Close();
                }
            }
        }

        private void ToggleLike_Click(object sender, RoutedEventArgs e)
        {
            var boton = sender as ToggleButton;
            var reseña = boton?.DataContext as ReseñaCompleta;
            if(reseña != null )
            {
                if (boton?.IsChecked == true)
                {
                    DarLike_Click(reseña);
                }
                else
                {
                    QuitarLike_Click(reseña);
                }
            }
        }


        private async void QuitarLike_Click(ReseñaCompleta reseña)
        {
            DeleteMeGustaSolicitud deleteMeGustaSolicitud = new DeleteMeGustaSolicitud()
            {
                idJugador = UsuarioSingleton.Instancia.idJugador,
                idJugadorAutor = reseña.idJugador,
                idResena = reseña.idResenia
            };
            ApiRespuestaBase respuestaBase = await ServicioMeGusta.EliminarMeGustaAReseña(reseña.idJuego,deleteMeGustaSolicitud,apiRestCreadorRespuesta);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasConDatosODiferentesAlCodigoDeExito(respuestaBase);
            if (!esRespuestaCritica)
            {
                reseña.existeMeGustaReseña = false;
                reseña.totalDeMeGustaReseña--;
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                this.Close();
            }
        }

        private async void DarLike_Click(ReseñaCompleta reseña)
        {
            PostMeGustaSolicitud datosSolicitud = new PostMeGustaSolicitud()
            {
                idJugador = UsuarioSingleton.Instancia.idJugador,
                idResena = reseña.idResenia,
                idJuego = reseña.idJuego,
                idJugadorAutor = reseña.idJugador,
                nombreJuego = reseña.nombre
            };
            ApiRespuestaBase respuestaBase = await ServicioMeGusta.RegistrarMeGustaAReseña(datosSolicitud,apiRestCreadorRespuesta);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasConDatosODiferentesAlCodigoDeExito(respuestaBase);
            if (!esRespuestaCritica)
            {
                reseña.existeMeGustaReseña = true;
                reseña.totalDeMeGustaReseña++;
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                this.Close();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            VentanaDescripcionJuego ventanaDescripcionJuego = new VentanaDescripcionJuego(_modeloJuego);
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaDescripcionJuego);
            this.Close();
        }
    }

    public class ReseñaCompleta : ReseñaJugadores, INotifyPropertyChanged
    {
        public string FechaFormateada
        {
            get
            {
                if (DateTime.TryParse(fecha, out var fechaConvertida))
                {
                    fecha = fechaConvertida.ToString("dd/MM/yyyy");
                }
                return fecha!;
            }
        }
        public byte[]? fotoJugador { get; set; }
        private int _totalDeMeGusta;
        public int totalDeMeGustaReseña
        {
            get => _totalDeMeGusta;
            set
            {
                if (_totalDeMeGusta != value)
                {
                    _totalDeMeGusta = value;
                    OnPropertyChanged(nameof(totalDeMeGustaReseña));
                }
            }
        }

        private bool _existeMeGusta;
        public bool existeMeGustaReseña
        {
            get => _existeMeGusta;
            set
            {
                if (_existeMeGusta != value)
                {
                    _existeMeGusta = value;
                    OnPropertyChanged(nameof(existeMeGustaReseña));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
