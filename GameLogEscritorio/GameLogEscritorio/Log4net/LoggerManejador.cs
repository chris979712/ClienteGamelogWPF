using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;

namespace GameLogEscritorio.Log4net
{
    public static class LoggerManejador
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!);

        static LoggerManejador()
        {
            log4net.Util.LogLog.InternalDebugging = true;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly()!);
            var configFile = new FileInfo("log4net.config");
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Logs"));
            XmlConfigurator.ConfigureAndWatch(logRepository, configFile);
        }

        public static void Informacion(string mensaje)
        {
            Logger.Info(mensaje);
        }

        public static void Depuracion(string mensaje)
        {
            Logger.Debug(mensaje);
        }

        public static void Advertencia(string mensaje)
        {
            Logger.Warn(mensaje);
        }

        public static void Error(string mensaje)
        {
            Logger.Error(mensaje);
        }

        public static void Fatal(string mensaje)
        {
            Logger.Fatal(mensaje);
        }
    }
}
