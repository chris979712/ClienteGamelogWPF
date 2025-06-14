using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Acceso;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Utilidades;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GameLogEscritorio.Ventanas
{
    
    public partial class VentanaRegistroDeCuenta : Window
    {

        private static readonly IApiRestRespuestaFactory apiRestCreadorRespuesta = new FactoryRespuestasAPI();

        public VentanaRegistroDeCuenta()
        {
            InitializeComponent();
            Estaticas.GuardarMedidasUltimaVentana(this);
        }

        private async void Registrar_Click(object sender, RoutedEventArgs e)
        {
            CambiarColorCampos();
            if(ValidarCampos())
            {
                grd_OverlayCarga.Visibility = Visibility.Visible; 
                PostAccesoSolicitud datosSolicitud = CrearSolicitudAcceso();
                ApiRespuestaBase respuestaBase = await ServicioAcceso.CrearCuenta(datosSolicitud,apiRestCreadorRespuesta);
                if(respuestaBase.estado == Constantes.CodigoExito)
                {
                    grd_OverlayCarga.Visibility = Visibility.Collapsed;
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoExito, respuestaBase.mensaje!, respuestaBase.estado);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                    VentanaInicioDeSesion ventanaInicioDeSesion = new VentanaInicioDeSesion();
                    AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaInicioDeSesion);
                    this.Close();
                }
                else
                {
                    grd_OverlayCarga.Visibility = Visibility.Collapsed;
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuestaBase.mensaje!, respuestaBase.estado);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                }
            }
        }

        private PostAccesoSolicitud CrearSolicitudAcceso()
        {
            return new PostAccesoSolicitud()
            {
                nombre = txb_Nombre.Text,
                primerApellido = txb_PrimerApellido.Text,
                segundoApellido = txb_SegundoApellido.Text,
                nombreDeUsuario = txb_NombreUsuario.Text,
                correo = txb_Correo.Text,
                descripcion = txb_Descripcion.Text,
                contrasenia = Encriptador.hasheoA256(pb_Contrasenia.Password),
                estado = Constantes.TipoDeEstadoPorDefecto,
                tipoDeUsuario = Constantes.tipoJugadorPorDefecto,
                foto = Properties.Resources.RutaFotoPorDefecto
            };
        }

        private bool ValidarCampos()
        {
            bool nombreValidado = Validador.ValidarSoloNombres(txb_Nombre.Text);
            bool primerApellidoValidado = Validador.ValidarSoloNombres(txb_PrimerApellido.Text);
            bool segundoApellidoValidado = Validador.ValidarSegundoApellido(txb_SegundoApellido.Text);
            bool nombreDeUsuarioValidado = Validador.ValidarNombreDeUsuario(txb_NombreUsuario.Text);
            bool correoValidado = Validador.ValidarCorreo(txb_Correo.Text);
            bool contraseniaValidada = Validador.ValidarContrasenia(pb_Contrasenia.Password);
            bool descripcionValidada = Validador.ValidarDescripcion(txb_Descripcion.Text);

            if(!nombreValidado)
            {
                txb_Nombre.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txb_Nombre);
            }
            if (!primerApellidoValidado)
            {
                txb_PrimerApellido.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txb_PrimerApellido);
            }
            if (!segundoApellidoValidado)
            {
                txb_SegundoApellido.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txb_SegundoApellido);
            }
            if (!nombreDeUsuarioValidado)
            {
                txb_NombreUsuario.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txb_NombreUsuario);
            }
            if (!correoValidado)
            {
                txb_Correo.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txb_Correo);
            }
            if (!contraseniaValidada)
            {
                pb_Contrasenia.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(pb_Contrasenia);
            }
            if (!descripcionValidada)
            {
                txb_Descripcion.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txb_Descripcion);
            }
            return nombreValidado && nombreDeUsuarioValidado && primerApellidoValidado && segundoApellidoValidado
                && correoValidado && contraseniaValidada && descripcionValidada;
        }

        private void CambiarColorCampos()
        {
            txb_Nombre.BorderBrush = Brushes.White;
            txb_PrimerApellido.BorderBrush = Brushes.White;
            txb_SegundoApellido.BorderBrush = Brushes.White;
            txb_NombreUsuario.BorderBrush = Brushes.White;
            txb_Correo.BorderBrush = Brushes.White;
            pb_Contrasenia.BorderBrush = Brushes.White;
            txb_Descripcion.BorderBrush = Brushes.White;
        }

        public void TextoSugeridoGtFocus(object sender, RoutedEventArgs e)
        {
            txbl_Sugerencia.Visibility = string.IsNullOrEmpty(pb_Contrasenia.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        public void TextoSugeridoLosFocus(object sender, RoutedEventArgs e)
        {
            txbl_Sugerencia.Visibility = string.IsNullOrEmpty(pb_Contrasenia.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void PasswordVisible(object sender, RoutedEventArgs e)
        {
            txbl_Sugerencia.Visibility = string.IsNullOrEmpty(pb_Contrasenia.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TextoVisibleChanged(object sender, TextChangedEventArgs e)
        {
            txbl_Sugerencia.Visibility = string.IsNullOrEmpty(txb_Contrasenia.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void CambiarVisibilidadContrasenia(object sender, RoutedEventArgs e)
        {
            if (tbtn_VisibilidadContrasenia.IsChecked == true)
            {
                txb_Contrasenia.Text = pb_Contrasenia.Password;
                pb_Contrasenia.Visibility = Visibility.Collapsed;
                txb_Contrasenia.Visibility = Visibility.Visible;
                txbl_Sugerencia.Visibility = string.IsNullOrEmpty(txb_Contrasenia.Text) ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                pb_Contrasenia.Password = txb_Contrasenia.Text;
                txb_Contrasenia.Visibility = Visibility.Collapsed;
                pb_Contrasenia.Visibility = Visibility.Visible;
                txbl_Sugerencia.Visibility = string.IsNullOrEmpty(pb_Contrasenia.Password) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            VentanaInicioDeSesion ventanaInicioDeSesion = new VentanaInicioDeSesion();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaInicioDeSesion);
            this.Close();
        }

    }
}
