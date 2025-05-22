using GameLogEscritorio.Servicios.APIRawg.Modelo;
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
    public partial class MenuPrincipal : Window
    {
        public MenuPrincipal()
        {
            InitializeComponent();
            txt_Jugador.Text = "Bienvenido " + UsuarioSingleton.Instancia.nombreDeUsuario!.ToString();
            if (Constantes.juegosPendientes.Count >= 1)
            {
                ic_JuegosPendientes.ItemsSource = Constantes.juegosPendientes;
                ic_JuegosPendientes.Visibility = Visibility.Visible;
                txt_SinJuegos.Visibility = Visibility.Collapsed;
            }
            else
            {
                ic_JuegosPendientes.Visibility = Visibility.Collapsed;
                txt_SinJuegos.Visibility = Visibility.Visible;
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
     
        }

        public void IrVentanaBuscarJuego_Click(object sender, RoutedEventArgs e)
        {
            VentanaBuscarJuego ventanaBuscarJuego = new VentanaBuscarJuego();
            ventanaBuscarJuego.Show();
            this.Close();
        }

        public void IrVentanaBuscarUsuario_Click(object sender, RoutedEventArgs e)
        {

        }

        public void IrVentanaMisReseñas_Click(object sender, RoutedEventArgs e)
        {
            
        }

        public void IrVentanaTendencias_Click(object sender, RoutedEventArgs e)
        {

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
