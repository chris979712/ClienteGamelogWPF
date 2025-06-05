using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Acceso;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Utilidades;
using System.Windows;
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
                PostAccesoSolicitud datosSolicitud = CrearSolicitudAcceso();
                ApiRespuestaBase respuestaBase = await ServicioAcceso.CrearCuenta(datosSolicitud,apiRestCreadorRespuesta);
                if(respuestaBase.estado == Constantes.CodigoExito)
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoExito, respuestaBase.mensaje!, respuestaBase.estado);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaEmergente);
                    VentanaInicioDeSesion ventanaInicioDeSesion = new VentanaInicioDeSesion();
                    AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaInicioDeSesion);
                    this.Close();
                }
                else
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuestaBase.mensaje!, respuestaBase.estado);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaEmergente);
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
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaInicioDeSesion);
            this.Close();
        }

    }
}
