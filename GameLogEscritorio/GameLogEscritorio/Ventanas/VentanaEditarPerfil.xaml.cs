using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.Win32;
using System.IO;
using GameLogEscritorio.Utilidades;
using GameLogEscritorio.Servicios.GameLogAPIGRPC.Respuesta;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Jugador;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.ApiResponse;


namespace GameLogEscritorio.Ventanas
{
    /// <summary>
    /// Lógica de interacción para VentanaEditarPerfil.xaml
    /// </summary>
    public partial class VentanaEditarPerfil : Window
    {

        private byte[] imagenASubir = new byte[0];
        private string _rutaNuevaFoto = "";

        public VentanaEditarPerfil()
        {
            InitializeComponent();
            CargarDatosUsuario();
            img_Perfil.Source = ConvertirBytesAImagen(UsuarioSingleton.Instancia.fotoDePerfil!);
            imagenASubir = UsuarioSingleton.Instancia.fotoDePerfil!;
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
            txtb_Descripcion.Text = UsuarioSingleton.Instancia.descripcion ?? "";
            txtb_Nombre.Text = UsuarioSingleton.Instancia.nombre!;
            txtb_PrimerApellido.Text = UsuarioSingleton.Instancia.primerApellido!;
            txtb_SegundoApellido.Text = UsuarioSingleton.Instancia.segundoApellido ?? "";
            txtb_NombreUsuario.Text = UsuarioSingleton.Instancia.nombreDeUsuario!;
        }

        private async void Actualizar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCambiosEnCampos())
            {
                if (ValidarDatos())
                {

                    if (await RealizarActualizacionFotoDePerfil())
                    {
                        await RealizarActualizacionPerfil();
                    }
                }
                else
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoError, Constantes.ContenidoDatosInvalidos, Constantes.CodigoErrorSolicitud);
                }
            }
            else
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoAdvertencia, Properties.Resources.EdicionSinCambios, Constantes.CodigoErrorSolicitud);
            }
        }

        private async Task<bool> RealizarActualizacionFotoDePerfil()
        {
            bool actualizacionExitosa = false;
            RespuestaGRPC respuestaGRPC = await Servicios.GameLogAPIGRPC.Servicio.ServicioFotoDePerfil.ActualizarFotoDePerfilJugador(UsuarioSingleton.Instancia.foto!, txtb_NombreUsuario.Text, imagenASubir);
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

        private async Task<bool> RealizarActualizacionPerfil()
        {
            PutJugadorSolicitud datosSolicitud = new PutJugadorSolicitud()
            {
                descripcion = txtb_Descripcion.Text,
                nombre = txtb_Nombre.Text,
                primerApellido = txtb_PrimerApellido.Text,
                segundoApellido = txtb_SegundoApellido.Text,
                nombreDeUsuario = txtb_NombreUsuario.Text,
                foto = _rutaNuevaFoto
            };
            ApiRespuestaBase respuestaBase = await Servicios.GameLogAPIRest.Servicio.ServicioJugador.ActualizarDatosDeJugador(datosSolicitud, UsuarioSingleton.Instancia.idJugador);
            bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasBase(respuestaBase);
            if(!esRespuestaCritica)
            {
                if(respuestaBase.estado == Constantes.CodigoExito)
                {
                    ActualizarSingleton();
                    MenuPrincipal menuPrincipal = new MenuPrincipal();
                    menuPrincipal.Show();
                    this.Close();
                }
                else
                {
                    await RevertirFotoDePerfil();
                }
            }
            else
            {
                await ManejadorSesion.RegresarInicioDeSesionSinAcceso(respuestaBase.mensaje!);
                this.Close();
            }
            return true;
        }

        private async Task<bool> RevertirFotoDePerfil()
        {
            RespuestaGRPC respuestaGRPC = await Servicios.GameLogAPIGRPC.Servicio.ServicioFotoDePerfil.ActualizarFotoDePerfilJugador(UsuarioSingleton.Instancia.foto!, UsuarioSingleton.Instancia.nombreDeUsuario!, FotoPorDefecto.ObtenerFotoDePerfilPorDefecto());
            return true;
        }

        private void ActualizarSingleton()
        {
            UsuarioSingleton.Instancia.nombre = txtb_Nombre.Text;
            UsuarioSingleton.Instancia.primerApellido = txtb_PrimerApellido.Text;
            UsuarioSingleton.Instancia.segundoApellido = txtb_SegundoApellido.Text;
            UsuarioSingleton.Instancia.nombreDeUsuario = txtb_NombreUsuario.Text;
            UsuarioSingleton.Instancia.descripcion = txtb_Descripcion.Text;
            UsuarioSingleton.Instancia.foto = _rutaNuevaFoto;
            UsuarioSingleton.Instancia.fotoDePerfil = imagenASubir;
        }

        private bool ValidarDatos()
        {
            bool esDescripcionValida = Validador.ValidarDescripcion(txtb_Descripcion.Text);
            bool esNombreValido = Validador.ValidarSoloNombres(txtb_Nombre.Text);
            bool esPrimerApellidoValido = Validador.ValidarSoloNombres(txtb_PrimerApellido.Text);
            bool esSegundoApellidoValido = Validador.ValidarSegundoApellido(txtb_SegundoApellido.Text);
            bool esNombreDeUsuarioValido = Validador.ValidarNombreDeUsuario(txtb_NombreUsuario.Text);

            if (!esDescripcionValida)
            {
                txtb_Descripcion.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txtb_Descripcion);
            }

            if(!esNombreDeUsuarioValido)
            {
                txtb_NombreUsuario.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txtb_NombreUsuario);
            }

            if (!esNombreValido)
            {
                txtb_Nombre.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txtb_Nombre);
            }

            if (!esPrimerApellidoValido)
            {
                txtb_PrimerApellido.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txtb_PrimerApellido);
            }

            if (!esSegundoApellidoValido)
            {
                txtb_SegundoApellido.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(txtb_SegundoApellido);
            }

            return esSegundoApellidoValido && esNombreValido && esPrimerApellidoValido && esNombreDeUsuarioValido && esDescripcionValida;
        }

        private bool ValidarCambiosEnCampos()
        {
            return txtb_Descripcion.Text != UsuarioSingleton.Instancia.descripcion! || txtb_Nombre.Text != UsuarioSingleton.Instancia.nombre! || txtb_PrimerApellido.Text != UsuarioSingleton.Instancia.primerApellido!
                || txtb_SegundoApellido.Text != UsuarioSingleton.Instancia.segundoApellido || txtb_NombreUsuario.Text != UsuarioSingleton.Instancia.nombreDeUsuario! || !imagenASubir.SequenceEqual(UsuarioSingleton.Instancia.fotoDePerfil!);
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MenuPrincipal menuPrincipal = new MenuPrincipal();
            menuPrincipal.Show();
            this.Close();
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            MenuPrincipal menuPrincipal = new MenuPrincipal();
            menuPrincipal.Show();
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
                        }
                    }
                    else
                    {
                        VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoAdvertencia, Properties.Resources.TamañoImagenMayor, Constantes.CodigoErrorSolicitud);
                    }
                }
                catch (Exception)
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoAdvertencia, Properties.Resources.ErrorAlLeerImagen, Constantes.CodigoErrorSolicitud);
                }
            }
        }

        private bool EsImagenValida(string rutaArchivo)
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
