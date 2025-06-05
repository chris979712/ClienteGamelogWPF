using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace GameLogEscritorio.Utilidades
{
    public static class AnimacionesVentana
    {

        public static void Rebotar(Control control)
        {
            TranslateTransform trans = new TranslateTransform();
            control.RenderTransform = trans;
            var animacion = new DoubleAnimationUsingKeyFrames();
            animacion.Duration = TimeSpan.FromMilliseconds(300);
            animacion.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromPercent(0.0)));
            animacion.KeyFrames.Add(new EasingDoubleKeyFrame(-5, KeyTime.FromPercent(0.2)));
            animacion.KeyFrames.Add(new EasingDoubleKeyFrame(5, KeyTime.FromPercent(0.4)));
            animacion.KeyFrames.Add(new EasingDoubleKeyFrame(-5, KeyTime.FromPercent(0.6)));
            animacion.KeyFrames.Add(new EasingDoubleKeyFrame(5, KeyTime.FromPercent(0.8)));
            animacion.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromPercent(1.0)));
            trans.BeginAnimation(TranslateTransform.XProperty, animacion);
        }

        public static void RebotarImagen(Image imagen)
        {
            imagen.RenderTransformOrigin = new Point(0.5, 1);
            TranslateTransform trans = new TranslateTransform();
            imagen.RenderTransform = trans;
            var animacion = new DoubleAnimationUsingKeyFrames
            {
                Duration = TimeSpan.FromMilliseconds(500)
            };
            animacion.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromPercent(0.0)));
            animacion.KeyFrames.Add(new EasingDoubleKeyFrame(-20, KeyTime.FromPercent(0.2)));
            animacion.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromPercent(0.4)));
            animacion.KeyFrames.Add(new EasingDoubleKeyFrame(-10, KeyTime.FromPercent(0.6)));
            animacion.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromPercent(0.8)));
            animacion.KeyFrames.Add(new EasingDoubleKeyFrame(-4, KeyTime.FromPercent(0.9)));
            animacion.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromPercent(1.0)));
            trans.BeginAnimation(TranslateTransform.YProperty, animacion);
        }

        public static void IniciarVentanaPosicionActualDeVentana(double top, double left,double width, double height,Window ventana)
        {
            ventana.WindowStartupLocation = WindowStartupLocation.Manual;
            ventana.Left = left;
            ventana.Top = top;
            ventana.Width = width;
            ventana.Height = height;
            ventana.Show();
        }

        public static void MostarVentanaEnCentroDePosicionActualDeVentana(double top, double left,double width, double height, Window ventana)
        {
            ventana.WindowStartupLocation = WindowStartupLocation.Manual;
            ventana.Left = left + (width - ventana.Width) / 2;
            ventana.Top = top + (height - ventana.Height) / 2;
            ventana.ShowDialog();
        }

    }
}
