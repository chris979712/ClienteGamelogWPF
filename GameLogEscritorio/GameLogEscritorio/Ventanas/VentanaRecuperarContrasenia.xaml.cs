using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Login;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GameLogEscritorio.Ventanas
{

    public partial class VentanaRecuperarContrasenia : Window
    {
        public VentanaRecuperarContrasenia()
        {
            InitializeComponent();
        }

        private async void Recuperar_Click(object sender, RoutedEventArgs e)
        {
            CambiarColorCampos();
            if (ValidarDatos())
            {
                PostRecuperacionDeCuentaSolicitud solicitud = new PostRecuperacionDeCuentaSolicitud()
                {
                    correo = txtb_Correo.Text,
                    tipoDeUsuario = Constantes.tipoJugadorPorDefecto
                };
                APIRecuperacionDeCuentaRespuesta resultado = await ServicioLogin.RecuperacionDeCuenta(solicitud);
                if(resultado.estado == 200)
                {
                    this.Close();
                    VentanaCodigoDeRecuperacion ventanaCodigoDeRecuperacion = new VentanaCodigoDeRecuperacion(resultado.idAcceso,txtb_Correo.Text);
                    ventanaCodigoDeRecuperacion.ShowDialog();
                }
                else
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError,resultado.mensaje!,resultado.estado);
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
