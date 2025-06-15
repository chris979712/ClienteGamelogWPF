using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Login;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Utilidades;
using System.Windows;
using System.Windows.Media;

namespace GameLogEscritorio.Ventanas
{

    public partial class VentanaRecuperarContrasenia : Window
    {

        private readonly IApiRestRespuestaFactory apiRestCreadorRespuesta = new FactoryRespuestasApi();
        public VentanaRecuperarContrasenia()
        {
            InitializeComponent();
        }

        private async void Recuperar_Click(object sender, RoutedEventArgs e)
        {
            CambiarColorCampos();
            if (ValidarDatos())
            {
                grd_OverlayCarga.Visibility = Visibility.Visible;
                PostRecuperacionDeCuentaSolicitud solicitud = new PostRecuperacionDeCuentaSolicitud()
                {
                    correo = txtb_Correo.Text,
                    tipoDeUsuario = Constantes.tipoJugadorPorDefecto
                };
                ApiRecuperacionDeCuentaRespuesta resultado = await ServicioLogin.RecuperacionDeCuenta(solicitud,apiRestCreadorRespuesta);
                grd_OverlayCarga.Visibility = Visibility.Collapsed;
                if (resultado.estado == 200)
                {
                    this.Close();
                    VentanaCodigoDeRecuperacion ventanaCodigoDeRecuperacion = new VentanaCodigoDeRecuperacion(resultado.idAcceso,txtb_Correo.Text);
                    ventanaCodigoDeRecuperacion.ShowDialog();
                }
                else
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError,resultado.mensaje!,resultado.estado);
                    ventanaEmergente.ShowDialog();
                }
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool ValidarDatos()
        {
            bool correoValidado = Validador.ValidarCorreo(txtb_Correo.Text);

            if (!correoValidado)
            {
                txtb_Correo.BorderBrush = Brushes.Red;
                txtb_CorrecionDeDatos.Visibility = Visibility.Visible;
                AnimacionesVentana.Rebotar(txtb_Correo);
            }

            return correoValidado;
        }

        private void CambiarColorCampos()
        {
            txtb_Correo.BorderBrush = Brushes.White;
            txtb_CorrecionDeDatos.Visibility = Visibility.Hidden;
        }

    }
}
