using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Reportes;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Utilidades;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.WPF;
using SkiaSharp;
using System;
using System.Collections.Generic;
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

namespace GameLogEscritorio.Ventanas
{
    /// <summary>
    /// Lógica de interacción para VentanaReporteTendencias.xaml
    /// </summary>
    public partial class VentanaReporteTendencias : Window
    {
        public ISeries[]? Series { get; set; }
        public Axis[]? XAxes { get; set; }
        public Axis[]? YAxes { get; set; }

        public VentanaReporteTendencias()
        {
            InitializeComponent();
            dp_Inicio.SelectedDate = DateTime.Now;
            dp_Fin.SelectedDate = DateTime.Now;
        }

        private async void BtnRevival_Click(object sender, RoutedEventArgs e)
        {
            LimpiarGrafica();
            DateTime? fechaInicioBusqueda = dp_Inicio.SelectedDate;
            DateTime? fechaFinBusqueda = dp_Fin.SelectedDate;
            string? fechaInicioBusquedaFormateada = fechaInicioBusqueda?.ToString("yyyy-MM-dd");
            string? fechaFinBusquedaFormateada = fechaFinBusqueda?.ToString("yyyy-MM-dd");
            if (ValidarDatos(fechaInicioBusquedaFormateada!, fechaFinBusquedaFormateada!))
            {
                ApiReportesRespuesta respuestaReportesRevivalRetro = await ServicioReporte.ObtenerReporteTendenciasRevivalRetro(fechaInicioBusquedaFormateada!, fechaFinBusquedaFormateada!);
                bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasConDatosODiferentesAlCodigoDeExito(respuestaReportesRevivalRetro);
                if (!esRespuestaCritica)
                {
                    if (respuestaReportesRevivalRetro.estado == Constantes.CodigoExito)
                    {
                        List<ReporteJuegos> reportesObtenidos = respuestaReportesRevivalRetro.juegos!;
                        ActualizarDatosGraficaReseñas(reportesObtenidos, Properties.Resources.DescripcionRevivalRetro,"Reseña revival retro");
                    }
                }
                else
                {
                    await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                    this.Close();
                }
            }
            else
            {
                new VentanaEmergente(Constantes.TipoAdvertencia, Properties.Resources.FechasInvalidas, Constantes.CodigoErrorServidor);
            }
        }

        private async void BtnMasResenados_Click(object sender, RoutedEventArgs e)
        {
            LimpiarGrafica();
            DateTime? fechaInicioBusqueda = dp_Inicio.SelectedDate;
            DateTime? fechaFinBusqueda = dp_Fin.SelectedDate;
            string? fechaInicioBusquedaFormateada = fechaInicioBusqueda?.ToString("yyyy-MM-dd");
            string? fechaFinBusquedaFormateada = fechaFinBusqueda?.ToString("yyyy-MM-dd");
            if (ValidarDatos(fechaInicioBusquedaFormateada!, fechaFinBusquedaFormateada!))
            {
                ApiReportesRespuesta respuestaReportesTendencias = await ServicioReporte.ObtenerReporteTendencias(fechaInicioBusquedaFormateada!, fechaFinBusquedaFormateada!);
                bool esRespuestaCritica = ManejadorRespuestas.ManejarRespuestasConDatosODiferentesAlCodigoDeExito(respuestaReportesTendencias);
                if (!esRespuestaCritica)
                {
                    if(respuestaReportesTendencias.estado == Constantes.CodigoExito)
                    {
                        List<ReporteJuegos> reportesObtenidosTendencias = respuestaReportesTendencias.juegos!;
                        ActualizarDatosGraficaReseñas(reportesObtenidosTendencias, Properties.Resources.DescripcionTendencias,"Reseña tendencias");
                    }
                }
                else
                {
                    await ManejadorSesion.RegresarInicioDeSesionSinAcceso();
                    this.Close();
                }
            }
            else
            {
                new VentanaEmergente(Constantes.TipoAdvertencia, Properties.Resources.FechasInvalidas, Constantes.CodigoErrorServidor);
            }
        }

        private void LimpiarGrafica()
        {
            GraficaReporte.Series = null!;
            GraficaReporte.XAxes = null!;
            GraficaReporte.YAxes = null!;
        }

        private void ActualizarDatosGraficaReseñas(List<ReporteJuegos> reporteReseñas, string descripcionReporte, string tipoDeReporte)
        {
            txtDescripcion.Text = descripcionReporte;
            IList<double> numeroDeReseñas = new List<double>();
            IList<string>? nombreJuegos = new List<string>();
            foreach (var reporte in reporteReseñas)
            {
                numeroDeReseñas.Add(reporte.totalReseñas!);
                nombreJuegos?.Add(reporte.nombre!);
            }
            Series = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Name = tipoDeReporte,
                    Values = numeroDeReseñas.ToList()
                }
            };

            XAxes = new Axis[]
            {
                new Axis {
                    Labels = nombreJuegos,
                    LabelsPaint = new SolidColorPaint
                    {
                        Color = SKColors.LightGoldenrodYellow,
                    },
                    TextSize = 8
                }
            };

            YAxes = new Axis[]
            {
                new Axis { Labeler = value => value.ToString("N0") }
            };
            GraficaReporte.Series = Series;
            GraficaReporte.XAxes = XAxes;
            GraficaReporte.YAxes = YAxes;
        }

        private bool ValidarDatos(string fechaInicioBusquedaFormateada, string fechaFinBusquedaFormateada)
        {
            DateTime? fechaInicioBusqueda = dp_Inicio.SelectedDate;
            DateTime? fechaFinBusqueda = dp_Fin.SelectedDate;
            bool fechaInicioValida = Validador.ValidarFecha(fechaInicioBusquedaFormateada!);
            bool fechaFinValida = Validador.ValidarFecha(fechaFinBusquedaFormateada!);
            if (!fechaInicioValida)
            {
                dp_Inicio.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(dp_Inicio);
            }
            if(!fechaFinValida)
            {
                dp_Fin.BorderBrush = Brushes.Red;
                AnimacionesVentana.Rebotar(dp_Fin);
            }

            return fechaInicioValida && fechaFinValida && fechaInicioBusqueda <= fechaFinBusqueda;
        }


        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnRegresar_Click(object sender, RoutedEventArgs e)
        {
            MenuPrincipal menuPrincipal = new MenuPrincipal();
            menuPrincipal.Show();
            this.Close();
        }

    }
}
