using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.Reportes;
using GameLogEscritorio.Servicios.GameLogAPIRest.Modelo.RespuestasApi;
using GameLogEscritorio.Servicios.GameLogAPIRest.Servicio;
using GameLogEscritorio.Utilidades;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Windows;
using System.Windows.Media;

namespace GameLogEscritorio.Ventanas
{
    
    public partial class VentanaReporteTendencias : Window
    {

        private readonly IApiRestRespuestaFactory apiRestCreadorRespuesta = new FactoryRespuestasAPI();
        public ISeries[]? Series { get; set; }
        public Axis[]? XAxes { get; set; }
        public Axis[]? YAxes { get; set; }

        public VentanaReporteTendencias()
        {
            InitializeComponent();
            dp_Inicio.SelectedDate = DateTime.Now;
            dp_Fin.SelectedDate = DateTime.Now;
            Estaticas.GuardarMedidasUltimaVentana(this);
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
                ApiReportesRespuesta respuestaReportesRevivalRetro = await ServicioReporte.ObtenerReporteTendenciasRevivalRetro(fechaInicioBusquedaFormateada!, fechaFinBusquedaFormateada!,apiRestCreadorRespuesta);
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
                VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoAdvertencia, Properties.Resources.FechasInvalidas, Constantes.CodigoErrorServidor);
                AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaEmergente);
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
                ApiReportesRespuesta respuestaReportesTendencias = await ServicioReporte.ObtenerReporteTendencias(fechaInicioBusquedaFormateada!, fechaFinBusquedaFormateada!,apiRestCreadorRespuesta);
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
                VentanaEmergente ventanaEmergente = new VentanaEmergente(Constantes.TipoAdvertencia, Properties.Resources.FechasInvalidas, Constantes.CodigoErrorServidor);
                AnimacionesVentana.MostarVentanaEnCentroDePosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, ventanaEmergente);
            }
        }

        private void LimpiarGrafica()
        {
            crts_GraficaReporte.Series = null!;
            crts_GraficaReporte.XAxes = null!;
            crts_GraficaReporte.YAxes = null!;
        }

        private void ActualizarDatosGraficaReseñas(List<ReporteJuegos> reporteReseñas, string descripcionReporte, string tipoDeReporte)
        {
            txbl_Descripcion.Text = descripcionReporte;
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
            crts_GraficaReporte.Series = Series;
            crts_GraficaReporte.XAxes = XAxes;
            crts_GraficaReporte.YAxes = YAxes;
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

        private void BtnRegresar_Click(object sender, RoutedEventArgs e)
        {
            MenuPrincipal menuPrincipal = new MenuPrincipal();
            AnimacionesVentana.IniciarVentanaPosicionActualDeVentana(this.Top, this.Left, this.Width, this.Height, menuPrincipal);
            this.Close();
        }

    }

}
