using FisioTurno.Data;
using FisioTurno.Models;

namespace FisioTurno.Views;

public partial class MenuPacientePage : ContentPage
{
    private readonly AppDatabase _db;
    private readonly Usuario _usuario;

    // 👉 Constructor vacío (si vienes desde App.UsuarioActual)
    public MenuPacientePage()
    {
        InitializeComponent();

        _db = App.Database;
        _usuario = App.UsuarioActual;

        if (_usuario != null)
            lblUsuario.Text = $"Paciente: {_usuario.Username}";
        else
            lblUsuario.Text = "Paciente";
    }

    // 👉 Constructor con parámetros (si vienes directo desde Login)
    public MenuPacientePage(AppDatabase db, Usuario user)
    {
        InitializeComponent();

        _db = db;
        _usuario = user;

        // Guardar usuario global
        App.UsuarioActual = user;

        lblUsuario.Text = $"Paciente: {user.Username}";
    }

    // 👉 Botón Agendar Cita
    private async void AgendarCita_Tapped(object sender, EventArgs e)
    {
        // Pasar db y usuario correctamente
        await Navigation.PushAsync(new AgendarCitaPage(_db, _usuario));
    }

    // 👉 Botón Mis Citas
    private async void MisCitas_Tapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DashboardCitasPage(_db, _usuario));
    }

    // 👉 Botón Cerrar Sesión
    private async void CerrarSesion_Tapped(object sender, EventArgs e)
    {
        bool cerrar = await DisplayAlert("Confirmar", "¿Desea cerrar sesión?", "Sí", "No");
        if (!cerrar) return;

        App.UsuarioActual = null;

        // Volver al login correctamente
        Application.Current.MainPage = new NavigationPage(new LoginPage(App.Database));
    }
}
