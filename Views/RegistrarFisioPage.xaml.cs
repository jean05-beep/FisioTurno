using FisioTurno.Data;
using FisioTurno.Models;

namespace FisioTurno.Views;

public partial class RegistrarFisioPage : ContentPage
{
    private readonly AppDatabase _db;

    public RegistrarFisioPage(AppDatabase db)
    {
        InitializeComponent();
        _db = db;
    }

    private async void Registrar_Clicked(object sender, EventArgs e)
    {
        var nombre = txtNombre.Text?.Trim();
        var username = txtUsername.Text?.Trim();
        var pass = txtPassword.Text?.Trim();

        // Validaciones básicas
        if (string.IsNullOrWhiteSpace(nombre) ||
            string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(pass))
        {
            await DisplayAlert("Error", "Todos los campos son obligatorios.", "OK");
            return;
        }

        // Validar usuario existente
        if (await _db.ExisteUsuarioAsync(username))
        {
            await DisplayAlert("Error", "Ese usuario ya existe.", "OK");
            return;
        }

        // Crear nuevo fisioterapeuta
        var fisio = new Usuario
        {
            NombreCompleto = nombre,
            Username = username,
            Password = pass,
            Rol = "FISIOTERAPEUTA"
        };

        await _db.RegistrarUsuarioAsync(fisio);

        await DisplayAlert("Éxito", "Fisioterapeuta registrado correctamente.", "OK");

        await Navigation.PopAsync();
    }

    private async void Volver_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}

