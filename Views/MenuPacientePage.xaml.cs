using FisioTurno.Data;
using FisioTurno.Models;

namespace FisioTurno.Views;

public partial class MenuPacientePage : ContentPage
{
    private readonly AppDatabase _db;
    private readonly Usuario _usuario;

    public MenuPacientePage(AppDatabase db, Usuario user)
    {
        InitializeComponent();
        _db = db;
        _usuario = user;

        lblUsuario.Text = $"Paciente: {user.Username}";
    }

    private async void AgendarCita_Tapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AgendarCitaPage(_db));
    }

    private async void MisCitas_Tapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DashboardCitasPage(_db, _usuario));
    }

    private async void CerrarSesion_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}

