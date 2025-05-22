using System.Windows;
using System.Windows.Controls;

namespace GameLogEscritorio.Extensiones
{
    public static class TextBoxExtensiones
    {
        public static readonly DependencyProperty TextoSugeridoProperty =
            DependencyProperty.RegisterAttached(
                "TextoSugerido",
                typeof(string),
                typeof(TextBoxExtensiones),
                new PropertyMetadata(string.Empty));

        public static string GetTextoSugerido(DependencyObject obj)
        {
            return (string)obj.GetValue(TextoSugeridoProperty);
        }

        public static void SetTextoSugerido(DependencyObject obj, string value)
        {
            obj.SetValue(TextoSugeridoProperty, value);
        }
    }
}
