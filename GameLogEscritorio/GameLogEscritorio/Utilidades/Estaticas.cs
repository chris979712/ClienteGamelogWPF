using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Juegos;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Jugador;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Seguidor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GameLogEscritorio.Ventanas.VentanaMiReseña;

namespace GameLogEscritorio.Utilidades
{
    public static class Estaticas
    {
        public static ObservableCollection<JuegoCompleto> juegosPendientes = new ObservableCollection<JuegoCompleto>();
        public static ObservableCollection<ReseñaJugador> reseñasJugador = new ObservableCollection<ReseñaJugador>();
        public static List<Juego> juegosFavoritos = new List<Juego>();
        public static List<int> idJugadoresSeguido = new List<int>();
    }
}
