using GameLogEscritorio.Servicios.GameLogAPIGRPC.Respuesta;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Juegos;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Jugador;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Social;
using GameLogEscritorio.Ventanas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static GameLogEscritorio.Ventanas.VentanaMiReseña;

namespace GameLogEscritorio.Utilidades
{
    public static class Estaticas
    {
        public static ObservableCollection<JuegoCompleto> juegosPendientes = new ObservableCollection<JuegoCompleto>();
        public static ObservableCollection<ReseñaJugador> reseñasJugador = new ObservableCollection<ReseñaJugador>();
        public static ObservableCollection<NotificacionCompleta> notificaciones = new ObservableCollection<NotificacionCompleta>();
        public static List<Juego> juegosFavoritos = new List<Juego>();
        public static List<int> idJugadoresSeguido = new List<int>();
        public static double ultimoTopVentana = 0;
        public static double ultimoLeftVentana = 0;
        public static double ultimoWidthVentana = 0;
        public static double ultimoHeightVentana = 0;

        public static void GuardarMedidasUltimaVentana(Window ventana)
        {
            ultimoWidthVentana = ventana.Width;
            ultimoHeightVentana = ventana.Height;
            ultimoLeftVentana = ventana.Left;
            ultimoTopVentana = ventana.Top;
        }
    }
}
