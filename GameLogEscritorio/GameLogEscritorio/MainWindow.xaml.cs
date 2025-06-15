using GameLogEscritorio.Log4net;
using GameLogEscritorio.Utilidades;
using System.IO;
using System.Windows;


namespace GameLogEscritorio
{
    
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            this.Closing += CerrarSesionForzadaDeUsuario;
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            log4net.Config.XmlConfigurator.Configure();
            LoggerManejador.Informacion(Properties.Resources.AplicacionIniciada);
        }

        private async void CerrarSesionForzadaDeUsuario(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            await ManejadorSesion.CerrarSesionForzadaDeUsuario();
        }

    }
}
