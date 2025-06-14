using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Acceso;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Login;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Utilidades;
using System.Windows;
using System.Windows.Controls;
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
                grd_OverlayCarga.Visibility = Visibility.Visible;
                PostRecuperacionDeCuentaValidacion datosSolicitud = new PostRecuperacionDeCuentaValidacion()
                {
                    codigo = int.Parse(txb_Codigo.Text),
                    correo = _correo,
                    tipoDeUsuario = Constantes.tipoJugadorPorDefecto
                };
                ApiRespuestaBase resultado = await ServicioLogin.RecuperacionDeCuentaValidacion(datosSolicitud,apiRestCreadorRespuesta);
                grd_OverlayCarga.Visibility = Visibility.Collapsed;
                if (resultado.estado == Constantes.CodigoExito)
                {
                    grd_Codigo.Visibility = Visibility.Collapsed;
                    grd_NuevaContrasenia.Visibility = Visibility.Visible;
                }
                else
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, resultado.mensaje!, resultado.estado);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                }
            }
        }

        private bool ValidarCodigoIngresado()
        {
            bool codigoValidado = Validador.ValidarCodigo(txb_Codigo.Text);
            if (!codigoValidado)
            {
                txb_Codigo.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txb_Codigo);
                txbl_ErrorCodigo.Visibility = Visibility.Visible;
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
                txbl_ErrorContraseña.Visibility = Visibility.Visible;
            }
            return contraseniaValidada;
        }

        private async void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            CambiarColorCampos();
            if (ValidarContraseniaIngresada())
            {
                grd_OverlayCarga.Visibility = Visibility.Visible;
                string contraseniaNuevaHasheada = Encriptador.hasheoA256(pb_NuevaContrasenia.Password);
                PutAccesoSolicitud datosSolicitud = new PutAccesoSolicitud()
                {
                    contrasenia = contraseniaNuevaHasheada,
                    correo = _correo,
                    tipoDeUsuario = Constantes.tipoJugadorPorDefecto
                };
                ApiRespuestaBase respuesta = await ServicioAcceso.EditarCredencialesDeAcceso(datosSolicitud, _idAcceso,apiRestCreadorRespuesta);
                grd_OverlayCarga.Visibility = Visibility.Collapsed;
                if (respuesta.estado == Constantes.CodigoExito)
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoExito, respuesta.mensaje!, respuesta.estado);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                    this.Close();
                }
                else
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuesta.mensaje!, respuesta.estado);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                }
            }
        }

        private void CambiarColorCampos()
        {
            txb_Codigo.BorderBrush = Brushes.White;
            pb_NuevaContrasenia.BorderBrush = Brushes.White;
            txbl_ErrorCodigo.Visibility = Visibility.Collapsed;
            txbl_ErrorContraseña.Visibility = Visibility.Collapsed;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void TextoSugeridoGtFocus(object sender, RoutedEventArgs e)
        {
            txbl_Sugerencia.Visibility = string.IsNullOrEmpty(pb_NuevaContrasenia.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        public void TextoSugeridoLosFocus(object sender, RoutedEventArgs e)
        {
            txbl_Sugerencia.Visibility = string.IsNullOrEmpty(pb_NuevaContrasenia.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void PasswordVisible(object sender, RoutedEventArgs e)
        {
            txbl_Sugerencia.Visibility = string.IsNullOrEmpty(pb_NuevaContrasenia.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TextoVisibleChanged(object sender, TextChangedEventArgs e)
        {
            txbl_Sugerencia.Visibility = string.IsNullOrEmpty(txb_NuevaContrasenia.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void CambiarVisibilidadContrasenia(object sender, RoutedEventArgs e)
        {
            if (tbtn_VisibilidadNuevaContrasenia.IsChecked == true)
            {
                txb_NuevaContrasenia.Text = pb_NuevaContrasenia.Password;
                pb_NuevaContrasenia.Visibility = Visibility.Collapsed;
                txb_NuevaContrasenia.Visibility = Visibility.Visible;
                txbl_Sugerencia.Visibility = string.IsNullOrEmpty(txb_NuevaContrasenia.Text) ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                pb_NuevaContrasenia.Password = txb_NuevaContrasenia.Text;
                txb_NuevaContrasenia.Visibility = Visibility.Collapsed;
                pb_NuevaContrasenia.Visibility = Visibility.Visible;
                txbl_Sugerencia.Visibility = string.IsNullOrEmpty(pb_NuevaContrasenia.Password) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

    }
}
