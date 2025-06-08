using GameLogEscritorio.Servicios.ServicioNotificacion.Mensaje;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

    public partial class VentanaEmergenteNotificacion : Window
    {      

        public VentanaEmergenteNotificacion(string mensaje)
        {
            InitializeComponent();
            txtb_Notificacion.Text = mensaje;
            Loaded += (s, e) =>
            {
                var ventanaPrincipal = Application.Current.MainWindow;
                double centroMitadDerechaX = ventanaPrincipal.Left + (ventanaPrincipal.ActualWidth * 0.85);
                Left = centroMitadDerechaX - (this.ActualWidth / 2);
                Top = ventanaPrincipal.Top + 20;
                Top = ventanaPrincipal.Top + 20; 
                this.Topmost = true;
                this.Activate();
                Task.Delay(3000).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(Close);
                });
            };
        }

    }

}
