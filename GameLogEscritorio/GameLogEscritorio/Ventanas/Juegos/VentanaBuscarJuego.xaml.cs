using GameLogEscritorio.Servicios.APIRawg.Modelo;
using GameLogEscritorio.Servicios.APIRawg.Servicio;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Juegos;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Utilidades;
using HtmlAgilityPack;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameLogEscritorio.Ventanas
{
    
    public partial class VentanaBuscarJuego : Window
    {

        private static readonly IApiRestRespuestaFactory apiRestCreadorRespuesta = new FactoryRespuestasApi();

        private JuegoModelo _modeloJuegoEncontrado = new JuegoModelo();

        public VentanaBuscarJuego()
        {
            InitializeComponent();
            Estaticas.GuardarMedidasUltimaVentana(this);
        }

        public bool ValidarDatos()
        {
            bool nombreValidado = Validador.ValidarDescripcion(txb_Busqueda.Text);
            if (!nombreValidado)
            {
                txb_Busqueda.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txb_Busqueda);
            }
            return nombreValidado;
        }

        private async void Buscar_Click(object sender, RoutedEventArgs e)
        {
            txb_Busqueda.BorderBrush = Brushes.White;
            if (ValidarDatos())
            {
                string nombreNormal = txb_Busqueda.Text;
                string soloLetrasYNumeros = Regex.Replace(nombreNormal, @"[^a-zA-Z0-9\s]", "");
                string nombreSlugJuego = Regex.Replace(soloLetrasYNumeros.Trim(), @"\s+", "-").ToLower();
                grd_OverlayCarga.Visibility = Visibility.Visible;
                grd_resultado.Visibility = Visibility.Collapsed;
                _modeloJuegoEncontrado =  await ServicioBuscarJuego.BuscarJuegoPorSlug(nombreSlugJuego);
                if (!string.IsNullOrEmpty(_modeloJuegoEncontrado.name))
                {
                    CargarDatosVideojuego();
                }
                else if (!string.IsNullOrEmpty(_modeloJuegoEncontrado.slug))
                {
                    string nombreLimpio = _modeloJuegoEncontrado.slug.Replace("-", " ");
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoAdvertencia, string.Concat(string.Concat(Properties.Resources.RedireccionamientoSlug," "), nombreLimpio), Constantes.CodigoErrorSolicitud);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                }
                else if(!string.IsNullOrEmpty(_modeloJuegoEncontrado.detail))
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoAdvertencia, _modeloJuegoEncontrado.detail, Constantes.CodigoErrorSolicitud);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                }
                else
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoAdvertencia, Properties.Resources.juegoIngresadoNoEncontrado, Constantes.CodigoErrorSolicitud);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                }
                grd_OverlayCarga.Visibility = Visibility.Collapsed;
            }
        }

        public void CargarDatosVideojuego()
        {
            grd_resultado.Visibility = Visibility.Visible;
            txbl_NombreJuego.Text = _modeloJuegoEncontrado.name;
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(_modeloJuegoEncontrado.backgroundImage!);
            bitmap.EndInit();
            img_juego.Source = bitmap;
        }


        private async void Detalles_Click(object sender, RoutedEventArgs e)
        {
            PostJuegoSolicitud solicitud = new PostJuegoSolicitud()
            {
                idJuego = _modeloJuegoEncontrado.id,
                nombre = _modeloJuegoEncontrado.name,
                fechaDeLanzamiento = _modeloJuegoEncontrado.released
            };
            grd_OverlayCarga.Visibility = Visibility.Visible;
            ApiRespuestaBase respuesta = await ServicioJuego.RegistrarJuego(solicitud, apiRestCreadorRespuesta);
            if (respuesta.estado == Constantes.CodigoErrorServidor)
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuesta.mensaje!, Constantes.CodigoErrorServidor);
                AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
            }
            else if(respuesta.estado == Constantes.CodigoErrorAcceso) 
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                grd_OverlayCarga.Visibility = Visibility.Collapsed;
                this.Close();
            }
            else
            {
                if (!string.IsNullOrEmpty(_modeloJuegoEncontrado.description))
                {
                    HtmlDocument documento = new HtmlDocument();
                    documento.LoadHtml(_modeloJuegoEncontrado.description);
                    _modeloJuegoEncontrado.description = documento.DocumentNode.InnerText;
                }
                VentanaDescripcionJuego ventanaDescripcionJuego = new VentanaDescripcionJuego(_modeloJuegoEncontrado);
                AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaDescripcionJuego);
                grd_OverlayCarga.Visibility = Visibility.Collapsed;
                this.Close();
            }
            grd_OverlayCarga.Visibility = Visibility.Collapsed;
        }

        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            grd_OverlayCarga.Visibility = Visibility.Visible;
            MenuPrincipal menuPrincipal = new MenuPrincipal();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, menuPrincipal);
            grd_OverlayCarga.Visibility = Visibility.Collapsed;
            this.Close();
        }
    }

    public class Videojuego
    {
        public string? nombreVideojuego { get; set; }
        public string? imagenVideojuego { get; set; }

    }
}
