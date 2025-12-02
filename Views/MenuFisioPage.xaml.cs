using FisioTurno.Data;
using FisioTurno.Models;
using Microsoft.Maui.Media;

namespace FisioTurno.Views
{
    public partial class MenuFisioPage : ContentPage
    {
        private readonly AppDatabase _db;
        private readonly Usuario _usuario;

        public MenuFisioPage(AppDatabase db, Usuario usuario)
        {
            InitializeComponent();
            _db = db;
            _usuario = usuario;

            lblUsuario.Text = $"Bienvenido, {_usuario.NombreCompleto}";
        }

        // Ver citas del día
        private async void VerCitas_Tapped(object sender, EventArgs e )
        {
            await Navigation.PushAsync(
                new DashboardCitasFisioPage(_db, _usuario )
            );
        }

        // Cerrar sesión
        private void CerrarSesion_Tapped(object sender, EventArgs e)
        {
            App.UsuarioActual = null;
            Application.Current.Windows[0].Page =
                new NavigationPage(new LoginPage(_db));
        }

        
    }
}
