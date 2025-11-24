using FisioTurno.Data;
using FisioTurno.Models;

namespace FisioTurno.Views;

public partial class EditarFisioPage : ContentPage
{
    private readonly AppDatabase _db;
    private readonly Usuario _fisio;

    public EditarFisioPage(AppDatabase db, Usuario fisio)
    {
        InitializeComponent();

        _db = db;
        _fisio = fisio ?? throw new ArgumentNullException(nameof(fisio));

        // Cargar datos
        txtNombre.Text = _fisio.NombreCompleto;
        txtUsuario.Text = _fisio.Username;
        txtPass.Text = _fisio.Password;
    }

    private async void Guardar_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
            string.IsNullOrWhiteSpace(txtUsuario.Text) ||
            string.IsNullOrWhiteSpace(txtPass.Text))
        {
            await DisplayAlert("Error", "Todos los campos son obligatorios", "OK");
            return;
        }

        // Guardar cambios
        _fisio.NombreCompleto = txtNombre.Text.Trim();
        _fisio.Username = txtUsuario.Text.Trim();
        _fisio.Password = txtPass.Text.Trim();

        await _db.ActualizarUsuarioAsync(_fisio);

        await DisplayAlert("Éxito", "Fisioterapeuta actualizado correctamente", "OK");

        await Navigation.PopAsync();
    }
}
