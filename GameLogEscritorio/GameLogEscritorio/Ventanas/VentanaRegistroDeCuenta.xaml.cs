using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Acceso;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GameLogEscritorio.Ventanas
{
    /// <summary>
    /// Lógica de interacción para VentanaRegistroDeCuenta.xaml
    /// </summary>
    public partial class VentanaRegistroDeCuenta : Window
    {
        public VentanaRegistroDeCuenta()
        {
            InitializeComponent();
        }

        private async void Registrar_Click(object sender, RoutedEventArgs e)
        {
            CambiarColorCampos();
            if(ValidarCampos())
            {
                PostAccesoSolicitud datosSolicitud = CrearSolicitudAcceso();
                ApiRespuestaBase respuestaBase = await ServicioAcceso.CrearCuenta(datosSolicitud);
                if(respuestaBase.estado == Constantes.CodigoExito)
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoExito, respuestaBase.mensaje!, respuestaBase.estado);
                    VentanaInicioDeSesion ventanaInicioDeSesion = new VentanaInicioDeSesion();
                    ventanaInicioDeSesion.Show();
                    this.Close();
                }
                else
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuestaBase.mensaje!, respuestaBase.estado);
                }
            }
        }

        private PostAccesoSolicitud CrearSolicitudAcceso()
        {
            return new PostAccesoSolicitud()
            {
                nombre = txtb_Nombre.Text,
                primerApellido = txtb_PrimerApellido.Text,
                segundoApellido = txtb_SegundoApellido.Text,
                nombreDeUsuario = txtb_NombreUsuario.Text,
                correo = txtb_Correo.Text,
                descripcion = txtb_Descripcion.Text,
                contrasenia = Encriptador.hasheoA256(pb_Contrasenia.Password),
                estado = Constantes.TipoDeEstadoPorDefecto,
                tipoDeUsuario = Constantes.tipoJugadorPorDefecto,
                foto = Properties.Resources.RutaFotoPorDefecto
            };
        }

        private bool ValidarCampos()
        {
            bool nombreValidado = Validador.ValidarSoloNombres(txtb_Nombre.Text);
            bool primerApellidoValidado = Validador.ValidarSoloNombres(txtb_PrimerApellido.Text);
            bool segundoApellidoValidado = Validador.ValidarSegundoApellido(txtb_SegundoApellido.Text);
            bool nombreDeUsuarioValidado = Validador.ValidarNombreDeUsuario(txtb_NombreUsuario.Text);
            bool correoValidado = Validador.ValidarCorreo(txtb_Correo.Text);
            bool contraseniaValidada = Validador.ValidarContrasenia(pb_Contrasenia.Password);
            bool descripcionValidada = Validador.ValidarDescripcion(txtb_Descripcion.Text);

            if(!nombreValidado)
            {
                txtb_Nombre.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txtb_Nombre);
            }
            if (!primerApellidoValidado)
            {
                txtb_PrimerApellido.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txtb_PrimerApellido);
            }
            if (!segundoApellidoValidado)
            {
                txtb_SegundoApellido.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txtb_SegundoApellido);
            }
            if (!nombreDeUsuarioValidado)
            {
                txtb_NombreUsuario.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txtb_NombreUsuario);
            }
            if (!correoValidado)
            {
                txtb_Correo.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txtb_Correo);
            }
            if (!contraseniaValidada)
            {
                pb_Contrasenia.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(pb_Contrasenia);
            }
            if (!descripcionValidada)
            {
                txtb_Descripcion.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txtb_Descripcion);
            }
            return nombreValidado && nombreDeUsuarioValidado && primerApellidoValidado && segundoApellidoValidado
                && correoValidado && contraseniaValidada && descripcionValidada;
        }

        private void CambiarColorCampos()
        {
            txtb_Nombre.BorderBrush = Brushes.White;
            txtb_PrimerApellido.BorderBrush = Brushes.White;
            txtb_SegundoApellido.BorderBrush = Brushes.White;
            txtb_NombreUsuario.BorderBrush = Brushes.White;
            txtb_Correo.BorderBrush = Brushes.White;
            pb_Contrasenia.BorderBrush = Brushes.White;
            txtb_Descripcion.BorderBrush = Brushes.White;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            VentanaInicioDeSesion ventanaInicioDeSesion = new VentanaInicioDeSesion();
            ventanaInicioDeSesion.Show();
            this.Close();
        }

    }
}
