using System.Windows;


namespace GameLogEscritorio.Ventanas
{

    public partial class VentanaEmergenteNotificacion : Window
    {      

        public VentanaEmergenteNotificacion(string mensaje)
        {
            InitializeComponent();
            txbl_Notificacion.Text = mensaje;
            Loaded += (s, e) =>
            {
                var ventanaPrincipal = Application.Current.MainWindow;
                double centroMitadDerechaX = ventanaPrincipal.Left + (ventanaPrincipal.ActualWidth * 0.85);
                Left = centroMitadDerechaX - (this.ActualWidth / 2);
                Top = ventanaPrincipal.Top + 20;
                Top = ventanaPrincipal.Top + 20; 
                this.Topmost = true;
                this.Activate();
                Task.Delay(5000).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(Close);
                });
            };
        }

    }

}
