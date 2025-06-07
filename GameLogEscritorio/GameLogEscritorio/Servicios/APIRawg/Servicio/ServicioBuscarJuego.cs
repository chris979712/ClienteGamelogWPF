using GameLogEscritorio.Servicios.APIRawg.Modelo;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Juegos;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Utilidades;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace GameLogEscritorio.Servicios.APIRawg.Servicio
{
    public static class ServicioBuscarJuego
    {

        private static readonly IApiRestRespuestaFactory apiRestCreadorRespuesta = new FactoryRespuestasAPI();
        private static readonly string _RawgAPIURL = Properties.Resources.RawAPIGames;
        private static readonly HttpClient clienteHttp = new HttpClient();

        public static async Task<JuegoModelo> BuscarJuegoPorSlug(string nombre)
        {
            JuegoModelo juegoModelo = new JuegoModelo();
            try
            {
                HttpResponseMessage mensajeObtenido = await clienteHttp.GetAsync($"{_RawgAPIURL}{nombre}?key={Properties.Resources.RawgKey}");
                if (!mensajeObtenido.IsSuccessStatusCode)
                {
                    juegoModelo.detail = Properties.Resources.ErrorAlBuscarJuego;
                }
                else if(mensajeObtenido.IsSuccessStatusCode)
                {
                    string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                    juegoModelo = JsonConvert.DeserializeObject<JuegoModelo>(contenidoJson)!;
                }
            }
            catch(HttpRequestException)
            {
                juegoModelo.id = Constantes.ErrorEnLaOperacion;
                juegoModelo.detail = Properties.Resources.HttpExcepcion;
            }
            catch(JsonException)
            {
                juegoModelo.id = Constantes.ErrorEnLaOperacion;
                juegoModelo.detail = Properties.Resources.JsonExcepcion;
            }
            catch(Exception)
            {
                juegoModelo.id = Constantes.ErrorEnLaOperacion;
                juegoModelo.detail = Properties.Resources.Excepcion;
            }
            return juegoModelo;
        }

        public static async Task<JuegoModelo> BuscarJuegoPorID(int idJuego)
        {
            JuegoModelo juegoModelo = new JuegoModelo();
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using (var clienteHttp = new HttpClient(handler))
            {
                try
                {
                    HttpResponseMessage mensajeObtenido = await clienteHttp.GetAsync($"{_RawgAPIURL}{idJuego}?key={Properties.Resources.RawgKey}");
                    if (!mensajeObtenido.IsSuccessStatusCode)
                    {
                        juegoModelo.detail = Properties.Resources.ErrorAlBuscarJuego;
                    }
                    else
                    {
                        string contenidoJson = await mensajeObtenido.Content.ReadAsStringAsync();
                        juegoModelo = JsonConvert.DeserializeObject<JuegoModelo>(contenidoJson)!;
                    }
                }
                catch (HttpRequestException)
                {
                    juegoModelo.id = Constantes.ErrorEnLaOperacion;
                    juegoModelo.detail = Properties.Resources.HttpExcepcion;
                }
                catch (JsonException)
                {
                    juegoModelo.id = Constantes.ErrorEnLaOperacion;
                    juegoModelo.detail = Properties.Resources.JsonExcepcion;
                }
                catch (Exception)
                {
                    juegoModelo.id = Constantes.ErrorEnLaOperacion;
                    juegoModelo.detail = Properties.Resources.Excepcion;
                }
            }
            return juegoModelo;
        }

        public static async Task<ObservableCollection<JuegoCompleto>> ObtenerJuegosPendientesJugador(int idJugador)
        {
            ObservableCollection<JuegoCompleto> juegosPendientes = new ObservableCollection<JuegoCompleto>();
            var respuesta = await ServicioJuego.ObtenerJuegosPendientes(idJugador,apiRestCreadorRespuesta);
            if (respuesta.estado == Constantes.CodigoExito)
            {
                List<Juego> juegosPendietesObtenidos = respuesta.juegos!;
                foreach (var juego in juegosPendietesObtenidos)
                {
                    JuegoModelo juegoObtenidoRawg = await ServicioBuscarJuego.BuscarJuegoPorID(juego.idJuego);
                    if(juegoObtenidoRawg.id == Constantes.ErrorEnLaOperacion)
                    {
                        juegosPendientes.Insert(0, new JuegoCompleto()
                        {
                            idJuego = juegoObtenidoRawg.id,
                            descripcion = juegoObtenidoRawg.detail
                        });
                        break;
                    }
                    else
                    {
                        juegosPendientes.Add(new JuegoCompleto()
                        {
                            idJuego = juego.idJuego,
                            nombre = juego.nombre,
                            fechaLanzamiento = juegoObtenidoRawg.released,
                            descripcion = juegoObtenidoRawg.description,
                            rating = juegoObtenidoRawg.rating,
                            imagenFondo = juegoObtenidoRawg.backgroundImage,
                            platforms = juegoObtenidoRawg.platforms
                        });
                    }
                }
            }
            else if (respuesta.estado == Constantes.CodigoErrorSolicitud || respuesta.estado == Constantes.CodigoErrorServidor || respuesta.estado == Constantes.CodigoErrorAcceso)
            {
                juegosPendientes.Add(new JuegoCompleto()
                {
                    idJuego = respuesta.estado,
                    descripcion = respuesta.mensaje!
                });
            }
            return juegosPendientes;
        }

        public static async Task<ObservableCollection<JuegoCompleto>> ObtenerJuegosFavoritosJugador(int idJugador)
        {
            ObservableCollection<JuegoCompleto> juegosFavoritos = new ObservableCollection<JuegoCompleto>();
            var respuesta = await ServicioJuego.ObtenerJuegosFavoritos(idJugador,apiRestCreadorRespuesta);
            if (respuesta.estado == Constantes.CodigoExito)
            {
                List<Juego> juegosFavoritosObtenidos = respuesta.juegos!;
                foreach (var juego in juegosFavoritosObtenidos)
                {
                    JuegoModelo juegoObtenidoRawg = await ServicioBuscarJuego.BuscarJuegoPorID(juego.idJuego);
                    if (juegoObtenidoRawg.id == Constantes.ErrorEnLaOperacion)
                    {
                        juegosFavoritos.Insert(0, new JuegoCompleto()
                        {
                            idJuego = juegoObtenidoRawg.id,
                            descripcion = juegoObtenidoRawg.detail
                        });
                        break;
                    }
                    else
                    {
                        juegosFavoritos.Add(new JuegoCompleto()
                        {
                            idJuego = juego.idJuego,
                            nombre = juego.nombre,
                            fechaLanzamiento = juegoObtenidoRawg.released,
                            descripcion = juegoObtenidoRawg.description,
                            rating = juegoObtenidoRawg.rating,
                            imagenFondo = juegoObtenidoRawg.backgroundImage,
                            platforms = juegoObtenidoRawg.platforms
                        });
                    }
                }
            }
            else if (respuesta.estado == Constantes.CodigoErrorSolicitud || respuesta.estado == Constantes.CodigoErrorServidor || respuesta.estado == Constantes.CodigoErrorAcceso)
            {
                juegosFavoritos.Add(new JuegoCompleto()
                {
                    idJuego = respuesta.estado,
                    descripcion = respuesta.mensaje!
                });
            }
            return juegosFavoritos;
        }

    }
}
