using GameLogEscritorio.Utilidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GameLogEscritorio
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            await ManejadorSesion.CerrarSesionForzadaDeUsuario();
        }

    }
}
