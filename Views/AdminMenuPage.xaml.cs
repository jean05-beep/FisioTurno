using FisioTurno.Data;
using FisioTurno.Models;

namespace FisioTurno.Views
{
    public partial class AdminMenuPage : ContentPage
    {
        private readonly AppDatabase _db;
        private readonly Usuario _admin;

        public AdminMenuPage(AppDatabase db, Usuario admin)
        {
            InitializeComponent();

            _db = db;
            _admin = admin ?? throw new ArgumentNullException(nameof(admin));

            lblAdmin.Text = $"Administrador: {_admin.NombreCompleto}";
        }

        // 👉 Registrar nuevo fisioterapeuta
        private async void RegistrarFisio_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistrarFisioPage(_db));
        }

        // 👉 Ver lista de fisioterapeutas
        private async void ListaFisio_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaFisioterapeutasPage(_db));
        }

        // 👉 Ver todas las citas registradas
        private async void VerCitas_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DashboardCitasAdminPage(_db));
        }

        // 👉 Cerrar sesión
        private async void Cerrar_Tapped(object sender, EventArgs e)
        {
            bool confirmar = await DisplayAlert("Confirmar",
                                                "¿Desea cerrar sesión?",
                                                "Sí", "No");

            if (!confirmar)
                return;

            App.UsuarioActual = null;

            // ✔️ Nueva forma recomendada MAUI .NET 8/9
            Application.Current!.Windows[0].Page =
                new NavigationPage(new LoginPage(App.Database));
        }
    }
}
