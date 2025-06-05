using System.Globalization;
using System.Windows.Data;

namespace GameLogEscritorio.Utilidades
{
    public class CursorSobreImagenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string fuenteImagen)
            {
                return fuenteImagen.Replace(".png", "Oscuro.png");
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
