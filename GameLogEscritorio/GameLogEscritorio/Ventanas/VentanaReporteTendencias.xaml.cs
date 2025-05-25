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
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WPF;
using System.Collections.Generic;

namespace GameLogEscritorio.Ventanas
{
    /// <summary>
    /// Lógica de interacción para VentanaReporteTendencias.xaml
    /// </summary>
    public partial class VentanaReporteTendencias : Window
    {
        public ISeries[] Series { get; set; }
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }

        public VentanaReporteTendencias()
        {
            InitializeComponent();
            dpInicio.SelectedDate = DateTime.Now;
            dpFin.SelectedDate = DateTime.Now;
        }

        private void BtnRevival_Click(object sender, RoutedEventArgs e)
        {
            if (!FechasValidas()) return;

            txtDescripcion.Text = "El reporte seleccionado muestra los juegos retros más reseñados por la comunidad de acuerdo a las fechas seleccionadas.";

            Series = new ISeries[]
            {
        new ColumnSeries<double>
        {
            Name = "Retro",
            Values = new double[] { 15, 20, 10, 8, 12 }
        }
            };

            XAxes = new Axis[]
            {
        new Axis { Labels = new[] { "Pac-Man", "Tetris", "Mario", "Zelda", "Sonic" } }
            };

            YAxes = new Axis[]
            {
        new Axis { Labeler = value => value.ToString("N0") }
            };

            MiGrafica.Series = Series;
            MiGrafica.XAxes = XAxes;
            MiGrafica.YAxes = YAxes;
        }

        private void BtnMasResenados_Click(object sender, RoutedEventArgs e)
        {
            if (!FechasValidas()) return;

            txtDescripcion.Text = "El reporte seleccionado muestra los juegos más reseñados por la comunidad.";

            Series = new ISeries[]
            {
        new ColumnSeries<double>
        {
            Name = "Reseñas",
            Values = new double[] { 30, 45, 38, 22, 50 }
        }
            };

            XAxes = new Axis[]
            {
        new Axis { Labels = new[] { "Minecraft", "Fortnite", "Warzone", "Valorant", "LOL" } }
            };

            YAxes = new Axis[]
            {
        new Axis { Labeler = value => value.ToString("N0") }
            };

            MiGrafica.Series = Series;
            MiGrafica.XAxes = XAxes;
            MiGrafica.YAxes = YAxes;
        }

        private bool FechasValidas()
        {
            if (dpInicio.SelectedDate == null || dpFin.SelectedDate == null)
            {
                MessageBox.Show("Selecciona ambas fechas.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (dpInicio.SelectedDate > dpFin.SelectedDate)
            {
                MessageBox.Show("La fecha de inicio no puede ser mayor que la fecha final.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
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
