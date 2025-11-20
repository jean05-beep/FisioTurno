using FisioTurno.Data;
using FisioTurno.Models;

namespace FisioTurno.Views;

public partial class RegisterPage : ContentPage
{
    private readonly AppDatabase _db;

    public RegisterPage(AppDatabase db)
    {
        InitializeComponent();
        _db = db;
    }

    private async void Registrar_Clicked(object sender, EventArgs e)
    {
        var username = txtUsername.Text?.Trim();
        var pass = txtPassword.Text?.Trim();
        var confirm = txtConfirm.Text?.Trim();

        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(pass) ||
            string.IsNullOrWhiteSpace(confirm))
        {
            await DisplayAlert("Error", "Todos los campos son obligatorios", "OK");
            return;
        }

        if (pass != confirm)
        {
            await DisplayAlert("Error", "Las contraseñas no coinciden", "OK");
            return;
        }

        if (await _db.ExisteUsuarioAsync(username))
        {
            await DisplayAlert("Error", "El usuario ya existe", "OK");
            return;
        }

        // ✔ Asignar siempre rol PACIENTE
        await _db.RegistrarUsuarioAsync(new Usuario
        {
            Username = username,
            Password = pass,
            Rol = "Paciente"
        });

        await DisplayAlert("Éxito", "Paciente registrado correctamente", "OK");
        await Navigation.PopAsync();
    }

    private async void Volver_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}


