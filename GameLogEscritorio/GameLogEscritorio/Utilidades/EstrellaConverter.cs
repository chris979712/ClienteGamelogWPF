using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GameLogEscritorio.Utilidades
{
    public class EstrellaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int puntaje = System.Convert.ToInt32(value);
            int estrella = int.Parse((string)parameter);

            string img = (puntaje >= estrella) ? "estrella_llena.png" : "estrella_vacia.png";
            return new BitmapImage(new Uri($"pack://application:,,,/Imagenes/Iconos/{img}"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
