using GameLogEscritorio.Servicios.APIRawg.Modelo;
using GameLogEscritorio.Servicios.APIRawg.Servicio;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Utilidades;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Juegos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HtmlAgilityPack;

namespace GameLogEscritorio.Ventanas
{
    /// <summary>
    /// Lógica de interacción para VentanaBuscarJuego.xaml
    /// </summary>
    public partial class VentanaBuscarJuego : Window
    {

        private JuegoModelo _modeloJuegoEncontrado = new JuegoModelo();
        private ObservableCollection<Videojuego>? _juegosEncontrados { get; set; }

        public VentanaBuscarJuego()
        {
            InitializeComponent();
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            MenuPrincipal menuPrincipal = new MenuPrincipal();
            menuPrincipal.Show();
            this.Close();
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
                btn_buscar.Background = Brushes.Gray;
                btn_buscar.IsEnabled = false;
                _modeloJuegoEncontrado =  await ServicioBuscarJuego.BuscarJuegoPorSlug(nombreSlugJuego);
                btn_buscar.Background = Brushes.Green;
                btn_buscar.IsEnabled = true;
                if (!string.IsNullOrEmpty(_modeloJuegoEncontrado.name))
                {
                    CargarDatosVideojuego();
                }
                else if (!string.IsNullOrEmpty(_modeloJuegoEncontrado.slug))
                {
                    string nombreLimpio = _modeloJuegoEncontrado.slug.Replace("-", " ");
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoAdvertencia, string.Concat(string.Concat(Properties.Resources.RedireccionamientoSlug," "), nombreLimpio), Constantes.CodigoErrorSolicitud);
                }
                else if(!string.IsNullOrEmpty(_modeloJuegoEncontrado.detail))
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoAdvertencia, _modeloJuegoEncontrado.detail, Constantes.CodigoErrorSolicitud);
                }
                else
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoAdvertencia, Properties.Resources.juegoIngresadoNoEncontrado, Constantes.CodigoErrorSolicitud);
                }
            }
        }

        public void CargarDatosVideojuego()
        {
            grd_resultado.Visibility = Visibility.Visible;

            txt_nombreJuego.Text = _modeloJuegoEncontrado.name;

            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_modeloJuegoEncontrado.backgroundImage!);
                bitmap.EndInit();
                img_juego.Source = bitmap;
            }
            catch
            {
                img_juego.Source = null;
            }
        }


        private async void Detalles_Click(object sender, RoutedEventArgs e)
        {
            ApiRespuestaBase respuesta = await ServicioJuego.RegistrarJuego(new PostJuegoSolicitud()
            {
                idJuego = _modeloJuegoEncontrado.id,
                nombre = _modeloJuegoEncontrado.name,
                fechaDeLanzamiento = _modeloJuegoEncontrado.released
            });
            if (respuesta.estado == Constantes.CodigoErrorServidor)
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuesta.mensaje!, Constantes.CodigoErrorServidor);
            }
            else if(respuesta.estado == Constantes.CodigoErrorAcceso) 
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso(respuesta.mensaje!);
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
                ventanaDescripcionJuego.Show();
                this.Close();
            }
        }

        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            MenuPrincipal menuPrincipal = new MenuPrincipal();
            menuPrincipal.Show();
            this.Close();
        }
    }

    public class Videojuego
    {
        public string? nombreVideojuego { get; set; }
        public string? imagenVideojuego { get; set; }

    }
}
