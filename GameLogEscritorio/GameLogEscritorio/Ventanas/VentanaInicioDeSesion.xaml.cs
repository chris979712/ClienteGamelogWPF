using GameLogEscritorio.Servicios.APIRawg.Modelo;
using GameLogEscritorio.Servicios.APIRawg.Servicio;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Juegos;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Login;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class VentanaInicioDeSesion : Window
    {
        public VentanaInicioDeSesion()
        {
            InitializeComponent();
        }

        private async void IniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            if(ValidarDatos())
            {
                CambiarColorCampos();
                string correo = txtb_Correo.Text;
                string contraseniaHasheada = Encriptador.hasheoA256(pb_Contrasenia.Password);
                var datosSolicitud = new PostLoginSolicitud
                {
                    correo = correo,
                    contrasenia = contraseniaHasheada,
                    tipoDeUsuario = Properties.Resources.tipoDeUsuarioPorDefecto.ToString()
                };
                var respuesta = await ServicioLogin.IniciarSesion(datosSolicitud);
                if (respuesta.estado == 200)
                {
                    UsuarioSingleton.Instancia.IniciarSesion(respuesta.cuenta!.FirstOrDefault()!);
                    Constantes.juegosPendientes = await ServicioBuscarJuego.ObtenerJuegosPendientesJugador();
                    ApiJuegosRespuesta juegosFavoritosObtenidos = await ServicioJuego.ObtenerJuegosFavoritos(UsuarioSingleton.Instancia.idJugador);
                    if ((Constantes.juegosPendientes.Count == 1 && Constantes.juegosPendientes[0].idJuego == Constantes.CodigoErrorSolicitud) ||
                        (juegosFavoritosObtenidos.estado == Constantes.CodigoErrorSolicitud || juegosFavoritosObtenidos.estado == Constantes.CodigoErrorServidor))
                    {
                        VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoAdvertencia, Constantes.juegosPendientes[0].descripcion!, Constantes.juegosPendientes[0].idJuego);
                    }
                    else
                    {
                        CargarListaDeJuegos(juegosFavoritosObtenidos);
                        MenuPrincipal menuPrincipal = new MenuPrincipal();
                        menuPrincipal.Show();
                        this.Close();
                    }   
                }
                else
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuesta.mensaje!, respuesta.estado);
                }
            }
            else
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, Constantes.ContenidoDatosInvalidos, Constantes.CodigoErrorSolicitud);
            }
        }

        private void CargarListaDeJuegos(ApiJuegosRespuesta juegosFavoritosObtenidos)
        {
            if(Constantes.juegosFavoritos != null)
            {
                Constantes.juegosFavoritos = new List<Juego>();
            }
            else
            {
                Constantes.juegosFavoritos = juegosFavoritosObtenidos.juegos!;
            }
        }


        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove(); 
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); 
        }

        private void IrVentanaRecuperarContraseña(object sender, MouseButtonEventArgs e)
        {
            VentanaRecuperarContrasenia ventanaRecuperarContrasenia = new VentanaRecuperarContrasenia();
            ventanaRecuperarContrasenia.ShowDialog();
        }

        private void CrearCuenta_Click(object sender, RoutedEventArgs e)
        {
            VentanaRegistroDeCuenta ventanaRegistroDeCuenta = new VentanaRegistroDeCuenta();
            ventanaRegistroDeCuenta.Show();
            this.Close();
        }

        private bool ValidarDatos()
        {
            bool correoValidacion = Validador.ValidarCorreo(txtb_Correo.Text);
            bool contraseniaValidacion = Validador.ValidarContrasenia(pb_Contrasenia.Password);

            if(!correoValidacion)
            {
                txtb_Correo.BorderBrush = Brushes.Red;
            }

            if(!contraseniaValidacion)
            {
                pb_Contrasenia.BorderBrush = Brushes.Red;
            }

            return correoValidacion && contraseniaValidacion;
        }

        private void CambiarColorCampos()
        {
            txtb_Correo.BorderBrush = Brushes.White;
            pb_Contrasenia.BorderBrush = Brushes.White;
        }

        private void CambiarVisibilidadContrasenia(object sender, RoutedEventArgs e)
        {
            if (tbtn_VisibilidadContrasenia.IsChecked == true)
            {
                txtb_Contrasenia.Text = pb_Contrasenia.Password;
                pb_Contrasenia.Visibility = Visibility.Collapsed;
                txtb_Contrasenia.Visibility = Visibility.Visible;

                img_Visibilidad.Source = new BitmapImage(new Uri("pack://application:,,,/Imagenes/Iconos/IconoVisibleOscuro.png"));
            }
            else
            {
                pb_Contrasenia.Password = txtb_Contrasenia.Text;
                pb_Contrasenia.Visibility = Visibility.Visible;
                txtb_Contrasenia.Visibility = Visibility.Collapsed;

                img_Visibilidad.Source = new BitmapImage(new Uri("pack://application:,,,/Imagenes/Iconos/IconoNoVisibleOscuro.png"));
            }
        }

    }
}
