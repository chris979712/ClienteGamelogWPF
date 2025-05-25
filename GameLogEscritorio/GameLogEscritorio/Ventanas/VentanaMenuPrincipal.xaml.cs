using GameLogEscritorio.Servicios.APIRawg.Modelo;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Utilidades;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class MenuPrincipal : Window
    {
        public MenuPrincipal()
        {
            InitializeComponent();
            txt_Jugador.Text = "Bienvenido " + UsuarioSingleton.Instancia.nombreDeUsuario!.ToString();
            if (Estaticas.juegosPendientes.Count >= 1)
            {
                ic_JuegosPendientes.ItemsSource = Estaticas.juegosPendientes;
                ic_JuegosPendientes.Visibility = Visibility.Visible;
                txt_SinJuegos.Visibility = Visibility.Collapsed;
            }
            else
            {
                ic_JuegosPendientes.Visibility = Visibility.Collapsed;
                txt_SinJuegos.Visibility = Visibility.Visible;
            }
            img_FotoDePerfil.Source = ConvertirBytesAImagen(UsuarioSingleton.Instancia.fotoDePerfil!);
            DecorarNombre();
        }

        public void DecorarNombre()
        {
            if (!UsuarioSingleton.Instancia.tipoDeAcceso!.Equals(Constantes.tipoJugadorPorDefecto))
            {
                LinearGradientBrush arcoiris = new LinearGradientBrush();
                arcoiris.StartPoint = new Point(0, 0);
                arcoiris.EndPoint = new Point(1, 0);
                arcoiris.GradientStops.Add(new GradientStop(Colors.Red, 0.0));
                arcoiris.GradientStops.Add(new GradientStop(Colors.Orange, 0.2));
                arcoiris.GradientStops.Add(new GradientStop(Colors.Yellow, 0.4));
                arcoiris.GradientStops.Add(new GradientStop(Colors.Green, 0.6));
                arcoiris.GradientStops.Add(new GradientStop(Colors.Blue, 0.8));
                arcoiris.GradientStops.Add(new GradientStop(Colors.Purple, 1.0));
                txt_Jugador.Foreground = arcoiris;
            }
            else
            {
                txt_Jugador.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC000"));
            }
        }

        public static ImageSource ConvertirBytesAImagen(byte[] imagenBytes)
        {
            using (var ms = new MemoryStream(imagenBytes))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                bitmap.Freeze(); 
                return bitmap;
            }
        }

        public async void Salir_Click(object sender, RoutedEventArgs e)
        {
            ApiRespuestaBase respuesta = await ServicioLogin.CerrarSesion(UsuarioSingleton.Instancia.correo!);
            UsuarioSingleton.Instancia.CerrarSesion();
            SesionToken.CerrarSesion();
            VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, respuesta.mensaje!, respuesta.estado);
            VentanaInicioDeSesion ventanaInicioDeSesion = new VentanaInicioDeSesion();
            ventanaInicioDeSesion.Show();
            this.Close();
        }

        public void IrVentanaEditarPerfil_Click(object sender, RoutedEventArgs e)
        {
            VentanaEditarPerfil ventanaEditarPerfil = new VentanaEditarPerfil();
            ventanaEditarPerfil.Show();
            this.Close();
        }

        public void IrVentanaBuscarJuego_Click(object sender, RoutedEventArgs e)
        {
            VentanaBuscarJuego ventanaBuscarJuego = new VentanaBuscarJuego();
            ventanaBuscarJuego.Show();
            this.Close();
        }

        public void IrVentanaBuscarUsuario_Click(object sender, RoutedEventArgs e)
        {
            VentanaBuscarJugador ventanaBuscarJugador = new VentanaBuscarJugador();
            ventanaBuscarJugador.Show();
            this.Close();
        }

        public void IrVentanaMisReseñas_Click(object sender, RoutedEventArgs e)
        {
            
        }

        public void IrVentanaTendencias_Click(object sender, RoutedEventArgs e)
        {
            VentanaReporteTendencias ventanaReporteTendencias = new VentanaReporteTendencias();
            ventanaReporteTendencias.Show();
            this.Close();
        }

        public void IrVentanaSeguidores_Click(object sender,RoutedEventArgs e)
        {

        }

        public void IrAVentanaeseñarJuego_Click(object sender, RoutedEventArgs e)
        {
            Image? imagen = sender as Image;
            if (imagen != null)
            {
                var juegoSeleccionado = imagen.DataContext as JuegoCompleto;
                if (juegoSeleccionado != null)
                {
                    JuegoModelo modeloJuego = new JuegoModelo()
                    {
                        id = juegoSeleccionado.idJuego,
                        description = juegoSeleccionado.descripcion,
                        backgroundImage = juegoSeleccionado.imagenFondo,
                        name = juegoSeleccionado.nombre
                    };
                    VentanaReseñarJuego reseñarJuego = new VentanaReseñarJuego(modeloJuego, "Menu");
                    reseñarJuego.Show();
                    this.Close();
                }
            }
        }

        public async void CerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            await ManejadorSesion.RegresarInicioDeSesionUsuario();
            this.Close();
        }
    }
}
