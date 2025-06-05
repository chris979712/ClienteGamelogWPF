using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Acceso;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Login;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Utilidades;
using System.Windows;
using System.Windows.Media;


namespace GameLogEscritorio.Ventanas
{
   
    public partial class VentanaCodigoDeRecuperacion : Window
    {

        private readonly IApiRestRespuestaFactory apiRestCreadorRespuesta = new FactoryRespuestasAPI();
        private int _idAcceso {  get; set; }
        private string _correo { get; set; }

        public VentanaCodigoDeRecuperacion(int idAcceso,string correo)
        {
            InitializeComponent();
            this._idAcceso = idAcceso;
            this._correo = correo;
        }

        private async void Verificar_Click(object sender, RoutedEventArgs e)
        {
            CambiarColorCampos();
            if (ValidarCodigoIngresado())
            {
                PostRecuperacionDeCuentaValidacion datosSolicitud = new PostRecuperacionDeCuentaValidacion()
                {
                    codigo = int.Parse(txtb_Codigo.Text),
                    correo = _correo,
                    tipoDeUsuario = Constantes.tipoJugadorPorDefecto
                };
                ApiRespuestaBase resultado = await ServicioLogin.RecuperacionDeCuentaValidacion(datosSolicitud,apiRestCreadorRespuesta);
                if (resultado.estado == Constantes.CodigoExito)
                {
                    grd_Codigo.Visibility = Visibility.Collapsed;
                    grd_NuevaContrasenia.Visibility = Visibility.Visible;
                }
                else
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, resultado.mensaje!, resultado.estado);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaEmergente);
                }
            }
        }

        private bool ValidarCodigoIngresado()
        {
            bool codigoValidado = Validador.ValidarCodigo(txtb_Codigo.Text);
            if (!codigoValidado)
            {
                txtb_Codigo.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txtb_Codigo);
                txt_ErrorCodigo.Visibility = Visibility.Visible;
            }
            return codigoValidado;
        }

        private bool ValidarContraseniaIngresada()
        {
            bool contraseniaValidada = Validador.ValidarContrasenia(pb_NuevaContrasenia.Password);
            if (!contraseniaValidada)
            {
                pb_NuevaContrasenia.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(pb_NuevaContrasenia);
                txt_ErrorContraseña.Visibility = Visibility.Visible;
            }
            return contraseniaValidada;
        }

        private async void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            CambiarColorCampos();
            if (ValidarContraseniaIngresada())
            {
                string contraseniaNuevaHasheada = Encriptador.hasheoA256(pb_NuevaContrasenia.Password);
                PutAccesoSolicitud datosSolicitud = new PutAccesoSolicitud()
                {
                    contrasenia = contraseniaNuevaHasheada,
                    correo = _correo,
                    tipoDeUsuario = Constantes.tipoJugadorPorDefecto
                };
                ApiRespuestaBase respuesta = await ServicioAcceso.EditarCredencialesDeAcceso(datosSolicitud, _idAcceso,apiRestCreadorRespuesta);
                if (respuesta.estado == Constantes.CodigoExito)
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoExito, respuesta.mensaje!, respuesta.estado);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaEmergente);
                    this.Close();
                }
                else
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuesta.mensaje!, respuesta.estado);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaEmergente);
                }
            }
        }

        private void CambiarColorCampos()
        {
            txtb_Codigo.BorderBrush = Brushes.White;
            pb_NuevaContrasenia.BorderBrush = Brushes.White;
            txt_ErrorCodigo.Visibility = Visibility.Collapsed;
            txt_ErrorContraseña.Visibility = Visibility.Collapsed;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
