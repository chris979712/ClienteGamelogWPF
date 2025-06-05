using GameLogEscritorio.Utilidades;
using System.Windows;

namespace GameLogEscritorio.Ventanas
{
    public partial class VentanaEmergente : Window
    {
        public VentanaEmergente(string titulo, string contenido, int codigo)
        {
            InitializeComponent();
            Txbl_Titulo.Text = titulo;
            Txbl_Contenido.Text = contenido;
            string rutaIcono="/Imagenes/Iconos/Error.png";
            if(codigo == 200)
            {
                rutaIcono = "/Imagenes/Iconos/Exito.png";
            }
            else if(codigo >= 400 && codigo <= 499)
            {
                rutaIcono = "/Imagenes/Iconos/Advertencia.png";
                Txbl_Titulo.Text = Constantes.TipoInformacion;
            }
            else if(codigo == 500)
            {
                rutaIcono = "/Imagenes/Iconos/Error.png";
                Txbl_Titulo.Text = Constantes.TipoError;
            }
            Img_IconoVentanaEmergente.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(rutaIcono, UriKind.Relative));
        }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
