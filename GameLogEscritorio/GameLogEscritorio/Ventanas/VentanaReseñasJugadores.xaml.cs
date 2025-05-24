using GameLogEscritorio.Servicios.APIRawg.Modelo;
using GameLogEscritorio.Servicios.GameLogAPIGRPC.Respuesta;
using GameLogEscritorio.Servicios.GameLogAPIGRPC.Servicio;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Likes;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Reseñas;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Utilidades;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace GameLogEscritorio.Ventanas
{
    /// <summary>
    /// Lógica de interacción para VentanaReseñasJugadores.xaml
    /// </summary>
    public partial class VentanaReseñasJugadores : Window
    {

        private JuegoModelo _modeloJuego = new JuegoModelo();
        public ObservableCollection<ReseñaCompleta> Reseñas { get; set; } = new ObservableCollection<ReseñaCompleta>();
        public bool EsAdministrador => UsuarioSingleton.Instancia.tipoDeAcceso == "Administrador";

        public VentanaReseñasJugadores(JuegoModelo modeloJuego)
        {
            this._modeloJuego = modeloJuego;
            InitializeComponent();
            this.DataContext = this;
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            VentanaDescripcionJuego ventanaDescripcionJuego = new VentanaDescripcionJuego(_modeloJuego);
            ventanaDescripcionJuego.Show();
            this.Close();
        }

        private async void MostrarTodos_Click(object sender, RoutedEventArgs e)
        {
            Reseñas.Clear();
            ApiReseñaJugadoresRespuesta reseñasJugadores = await ServicioReseña.ObtenerReseñasDeUnJuego(_modeloJuego.id, UsuarioSingleton.Instancia.idJugador);
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
            ApiReseñaJugadoresRespuesta reseñasJugadores = await ServicioReseña.ObtenerReseñasDeJugadoresSeguidosEnUnJuego(_modeloJuego.id, UsuarioSingleton.Instancia.idJugador);
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
                    existeLikeReseña = reseña.existeLike,
                    totalDeLikes = reseña.totalDeLikes,
                    nombreDeUsuario = reseña.nombreDeUsuario,
                    nombre = reseña.nombre,
                    foto = reseña.foto,
                    totalDeLikesReseña = reseña.totalDeLikes,
                    existeLike = reseña.existeLike,
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
                ApiRespuestaBase apiRespuestaBase = await ServicioReseña.EliminarReseña(reseña.idResenia);
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
            ApiRespuestaBase respuestaBase = await ServicioLike.EliminarLikeAReseña(reseña.idResenia,UsuarioSingleton.Instancia.idJugador);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasConDatosODiferentesAlCodigoDeExito(respuestaBase);
            if (!esRespuestaCritica)
            {
                reseña.existeLikeReseña = false;
                reseña.totalDeLikesReseña--;
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                this.Close();
            }
        }

        private async void DarLike_Click(ReseñaCompleta reseña)
        {
            PostLikeSolicitud datosSolicitud = new PostLikeSolicitud()
            {
                idJugador = UsuarioSingleton.Instancia.idJugador,
                idResena = reseña.idResenia
            };
            ApiRespuestaBase respuestaBase = await ServicioLike.RegistrarLikeAReseña(datosSolicitud);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasConDatosODiferentesAlCodigoDeExito(respuestaBase);
            if (!esRespuestaCritica)
            {
                reseña.existeLikeReseña = true;
                reseña.totalDeLikesReseña++;
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
            ventanaDescripcionJuego.Show();
            this.Close();
        }
    }

    public class ReseñaCompleta : ReseñaJugadores, INotifyPropertyChanged
    {
        public byte[]? fotoJugador { get; set; }
        private int _totalDeLikes;
        public int totalDeLikesReseña
        {
            get => _totalDeLikes;
            set
            {
                if (_totalDeLikes != value)
                {
                    _totalDeLikes = value;
                    OnPropertyChanged(nameof(totalDeLikesReseña));
                }
            }
        }

        private bool _existeLike;
        public bool existeLikeReseña
        {
            get => _existeLike;
            set
            {
                if (_existeLike != value)
                {
                    _existeLike = value;
                    OnPropertyChanged(nameof(existeLikeReseña));
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
