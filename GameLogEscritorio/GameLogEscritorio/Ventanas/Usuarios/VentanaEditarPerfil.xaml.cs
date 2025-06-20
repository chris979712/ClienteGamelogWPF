﻿using GameLogEscritorio.Servicios.GameLogAPIGRPC.Respuesta;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Jugador;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Utilidades;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace GameLogEscritorio.Ventanas
{
    
    public partial class VentanaEditarPerfil : Window
    {

        private static readonly IApiRestRespuestaFactory apiRestCreadorRespuesta = new FactoryRespuestasApi();
        private byte[] imagenASubir = new byte[0];
        private string _rutaNuevaFoto = "";

        public VentanaEditarPerfil()
        {
            InitializeComponent();
            CargarDatosUsuario();
            img_Perfil.Source = ConvertirBytesAImagen(UsuarioSingleton.Instancia.fotoDePerfil!);
            imagenASubir = UsuarioSingleton.Instancia.fotoDePerfil!;
            Estaticas.GuardarMedidasUltimaVentana(this);
        }

        public static ImageSource ConvertirBytesAImagen(byte[] imagenBytes)
        {
            using (var ms = new MemoryStream(imagenBytes))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
        }

        private void CargarDatosUsuario()
        {
            txb_Descripcion.Text = UsuarioSingleton.Instancia.descripcion ?? "";
            txb_Nombre.Text = UsuarioSingleton.Instancia.nombre!;
            txb_PrimerApellido.Text = UsuarioSingleton.Instancia.primerApellido!;
            txb_SegundoApellido.Text = UsuarioSingleton.Instancia.segundoApellido ?? "";
            txb_NombreUsuario.Text = UsuarioSingleton.Instancia.nombreDeUsuario!;
        }

        private async void Actualizar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCambiosEnCampos())
            {
                if (ValidarDatos())
                {
                    grd_OverlayCarga.Visibility = Visibility.Visible;
                    if (imagenASubir.Length!=UsuarioSingleton.Instancia.fotoDePerfil!.Length)
                    {
                        bool resultadoActualizacionPerfil = await RealizarActualizacionFotoDePerfil();
                        if (resultadoActualizacionPerfil)
                        {
                            await RealizarActualizacionPerfil();
                        } 
                    }
                    else
                    {
                        _rutaNuevaFoto = UsuarioSingleton.Instancia.foto!;
                        await RealizarActualizacionPerfil();
                    }
                    grd_OverlayCarga.Visibility = Visibility.Collapsed;
                }
                else
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, Properties.Resources.ContenidoDatosInvalidos, Constantes.CodigoErrorSolicitud);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                }
            }
            else
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoAdvertencia, Properties.Resources.EdicionSinCambios, Constantes.CodigoErrorSolicitud);
                AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
            }
        }

        private async Task<bool> RealizarActualizacionFotoDePerfil()
        {
            bool actualizacionExitosa = false;
            RespuestaGRPC respuestaGRPC = await Servicios.GameLogAPIGRPC.Servicio.ServicioFotoDePerfil.ActualizarFotoDePerfilJugador(UsuarioSingleton.Instancia.foto!, UsuarioSingleton.Instancia.idJugador, imagenASubir);
            if (Validador.SoloRutas.IsMatch(respuestaGRPC.detalles!))
            {
                _rutaNuevaFoto = respuestaGRPC.detalles!;
                actualizacionExitosa = true;
            }
            else
            {
                ManejadorRespuestas.ManejadorRespuestasGRPC(respuestaGRPC.codigo);
                actualizacionExitosa = false;
            }
            return actualizacionExitosa;
        }

        private async Task RealizarActualizacionPerfil()
        {
            PutJugadorSolicitud datosSolicitud = new PutJugadorSolicitud()
            {
                descripcion = txb_Descripcion.Text,
                nombre = txb_Nombre.Text,
                primerApellido = txb_PrimerApellido.Text,
                segundoApellido = txb_SegundoApellido.Text,
                nombreDeUsuario = txb_NombreUsuario.Text,
                foto = _rutaNuevaFoto
            };
            ApiRespuestaBase respuestaBase = await Servicios.GameLogAPIRest.Servicio.ServicioJugador.ActualizarDatosDeJugador(datosSolicitud, UsuarioSingleton.Instancia.idJugador,apiRestCreadorRespuesta);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasBase(respuestaBase);
            if(!esRespuestaCritica)
            {
                if(respuestaBase.estado == Constantes.CodigoExito)
                {
                    ActualizarSingleton();
                    MenuPrincipal menuPrincipal = new MenuPrincipal();
                    AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, menuPrincipal);
                    this.Close();
                }
                else
                {
                    await RevertirFotoDePerfil();
                }
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                this.Close();
            }
        }

        private static async Task RevertirFotoDePerfil()
        {
            RespuestaGRPC respuestaGRPC = await Servicios.GameLogAPIGRPC.Servicio.ServicioFotoDePerfil.ActualizarFotoDePerfilJugador(UsuarioSingleton.Instancia.foto!, UsuarioSingleton.Instancia.idJugador!, FotoPorDefecto.ObtenerFotoDePerfilPorDefecto());
            if (respuestaGRPC.codigo != Constantes.CodigoExito)
            {
                ManejadorRespuestas.ManejadorRespuestasGRPC(respuestaGRPC.codigo);
            }
        }

        private void ActualizarSingleton()
        {
            UsuarioSingleton.Instancia.nombre = txb_Nombre.Text;
            UsuarioSingleton.Instancia.primerApellido = txb_PrimerApellido.Text;
            UsuarioSingleton.Instancia.segundoApellido = txb_SegundoApellido.Text;
            UsuarioSingleton.Instancia.nombreDeUsuario = txb_NombreUsuario.Text;
            UsuarioSingleton.Instancia.descripcion = txb_Descripcion.Text;
            UsuarioSingleton.Instancia.foto = _rutaNuevaFoto;
            UsuarioSingleton.Instancia.fotoDePerfil = imagenASubir;
        }

        private bool ValidarDatos()
        {
            bool esDescripcionValida = Validador.ValidarDescripcion(txb_Descripcion.Text);
            bool esNombreValido = Validador.ValidarSoloNombres(txb_Nombre.Text);
            bool esPrimerApellidoValido = Validador.ValidarSoloNombres(txb_PrimerApellido.Text);
            bool esSegundoApellidoValido = Validador.ValidarSegundoApellido(txb_SegundoApellido.Text);
            bool esNombreDeUsuarioValido = Validador.ValidarNombreDeUsuario(txb_NombreUsuario.Text);

            if (!esDescripcionValida)
            {
                txb_Descripcion.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txb_Descripcion);
            }

            if(!esNombreDeUsuarioValido)
            {
                txb_NombreUsuario.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txb_NombreUsuario);
            }

            if (!esNombreValido)
            {
                txb_Nombre.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txb_Nombre);
            }

            if (!esPrimerApellidoValido)
            {
                txb_PrimerApellido.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txb_PrimerApellido);
            }

            if (!esSegundoApellidoValido)
            {
                txb_SegundoApellido.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txb_SegundoApellido);
            }

            return esSegundoApellidoValido && esNombreValido && esPrimerApellidoValido && esNombreDeUsuarioValido && esDescripcionValida;
        }

        private bool ValidarCambiosEnCampos()
        {
            return txb_Descripcion.Text != UsuarioSingleton.Instancia.descripcion! || txb_Nombre.Text != UsuarioSingleton.Instancia.nombre! || txb_PrimerApellido.Text != UsuarioSingleton.Instancia.primerApellido!
                || txb_SegundoApellido.Text != UsuarioSingleton.Instancia.segundoApellido || txb_NombreUsuario.Text != UsuarioSingleton.Instancia.nombreDeUsuario! || !imagenASubir.SequenceEqual(UsuarioSingleton.Instancia.fotoDePerfil!);
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            grd_OverlayCarga.Visibility = Visibility.Visible;
            MenuPrincipal menuPrincipal = new MenuPrincipal();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, menuPrincipal);
            grd_OverlayCarga.Visibility = Visibility.Collapsed;
            this.Close();
        }

        private void btnSeleccionarImagen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Archivos de imagen (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                    if(fileInfo.Length < 2 * 1024 * 1024)
                    {
                        if (EsImagenValida(openFileDialog.FileName))
                        {
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(openFileDialog.FileName);
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                            imagenASubir = File.ReadAllBytes(openFileDialog.FileName);
                            img_Perfil.Source = ConvertirBytesAImagen(imagenASubir);
                        }
                        else
                        {
                            VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoAdvertencia, Properties.Resources.ArchivoNoEsImagen,Constantes.CodigoErrorSolicitud);
                            AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                        }
                    }
                    else
                    {
                        VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoAdvertencia, Properties.Resources.TamañoImagenMayor, Constantes.CodigoErrorSolicitud);
                        AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                    }
                }
                catch (Exception)
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoAdvertencia, Properties.Resources.ErrorAlLeerImagen, Constantes.CodigoErrorSolicitud);
                    AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(ventanaEmergente);
                }
            }
        }

        private static bool EsImagenValida(string rutaArchivo)
        {
            bool esImagenValida = false;
            try
            {
                byte[] headerBytes = new byte[8];
                using (FileStream fs = new FileStream(rutaArchivo, FileMode.Open, FileAccess.Read))
                {
                    fs.Read(headerBytes, 0, headerBytes.Length);
                }
                if (headerBytes.Take(2).SequenceEqual(new byte[] { 0xFF, 0xD8 }))
                {
                    esImagenValida = true;
                }
                else if (headerBytes.Take(8).SequenceEqual(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }))
                {
                    esImagenValida = true;
                }
                else if (headerBytes.Take(3).SequenceEqual(new byte[] { 0x47, 0x49, 0x46 }))
                {
                    esImagenValida = true;
                }
                if (headerBytes.Take(2).SequenceEqual(new byte[] { 0x42, 0x4D }))
                {
                    esImagenValida = true;
                }
            }
            catch
            {
                esImagenValida = false;
            }
            return esImagenValida;
        }

    }
}
