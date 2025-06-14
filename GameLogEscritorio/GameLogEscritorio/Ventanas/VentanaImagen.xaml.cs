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
    /// <summary>
    /// Lógica de interacción para VentanaImagen.xaml
    /// </summary>
    public partial class VentanaImagen : Window
    {
        public VentanaImagen(byte[] imagen)
        {
            InitializeComponent();
            img_Vista.Source = BytesAImagen(imagen);
        }

        public static BitmapImage BytesAImagen(byte[] imageDatos)
        {
            using (var ms = new MemoryStream(imageDatos))
            {
                var imagen = new BitmapImage();
                imagen.BeginInit();
                imagen.CacheOption = BitmapCacheOption.OnLoad;
                imagen.StreamSource = ms;
                imagen.EndInit();
                imagen.Freeze();
                return imagen;
            }
        }

        private void CerrarVentana(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
