using System.Windows;

namespace GameLogEscritorio.Ventanas
{
    
    public partial class VentanaDeConfirmacion : Window
    {

        public VentanaDeConfirmacion(string contenido, Window ownerWindow)
        {
            InitializeComponent();
            Txbl_Contenido.Text = contenido;
            if (ownerWindow != null)
            {
                this.Owner = ownerWindow;
                this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            else
            {
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
        }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }

}
