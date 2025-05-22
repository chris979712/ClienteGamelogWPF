using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GameLogEscritorio.Utilidades
{
    public class CorazonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool leDioLike = value is bool b && b;
            return leDioLike
                ? "/Imagenes/Iconos/megusta.png"
                : "/Imagenes/Iconos/megusta_vacio.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            Binding.DoNothing;
    }

}
