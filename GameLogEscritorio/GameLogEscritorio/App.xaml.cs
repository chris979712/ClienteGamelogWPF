using GameLogEscritorio.Log4net;
using GameLogEscritorio.Utilidades;
using System.Windows;

namespace GameLogEscritorio
{
    
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            LoggerManejador.Informacion("Aplicación iniciada");
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            await ManejadorSesion.CerrarSesionForzadaDeUsuario();
        }

    }

}
