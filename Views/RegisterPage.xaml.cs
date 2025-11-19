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
        var user = txtUsername.Text?.Trim();
        var pass = txtPassword.Text?.Trim();
        var confirm = txtConfirm.Text?.Trim();

        if (string.IsNullOrWhiteSpace(user) ||
            string.IsNullOrWhiteSpace(pass) ||
            string.IsNullOrWhiteSpace(confirm) ||
            pickRol.SelectedItem == null)
        {
            await DisplayAlert("Error", "Todos los campos son obligatorios", "OK");
            return;
        }

        if (pass != confirm)
        {
            await DisplayAlert("Error", "Las contraseñas no coinciden", "OK");
            return;
        }

        bool existe = await _db.ExisteUsuarioAsync(user);
        if (existe)
        {
            await DisplayAlert("Error", "El usuario ya existe", "OK");
            return;
        }

        await _db.RegistrarUsuarioAsync(new Usuario
        {
            Username = user,
            Password = pass,
            Rol = pickRol.SelectedItem.ToString()
        });

        await DisplayAlert("Éxito", "Registro completado", "OK");
        await Navigation.PopAsync();
    }
}
