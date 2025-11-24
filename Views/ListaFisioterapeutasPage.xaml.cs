using FisioTurno.Data;
using FisioTurno.Models;

namespace FisioTurno.Views;

public partial class ListaFisioterapeutasPage : ContentPage
{
    private readonly AppDatabase _db;

    public ListaFisioterapeutasPage(AppDatabase db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        ListaFisio.ItemsSource = await _db.ObtenerFisioterapeutasAsync();
    }

    private async void Editar_Clicked(object sender, EventArgs e)
    {
        var fisio = (sender as Button)?.CommandParameter as Usuario;

        if (fisio == null)
        {
            await DisplayAlert("Error", "No se pudo obtener el fisioterapeuta.", "OK");
            return;
        }

        await Navigation.PushAsync(new EditarFisioPage(_db, fisio));
    }

    private async void Eliminar_Clicked(object sender, EventArgs e)
    {
        var fisio = (sender as Button)?.CommandParameter as Usuario;

        if (fisio == null)
        {
            await DisplayAlert("Error", "No se pudo obtener el fisioterapeuta.", "OK");
            return;
        }

        bool confirmar = await DisplayAlert(
            "Confirmación",
            $"¿Deseas eliminar a {fisio.NombreCompleto}?",
            "Sí", "No");

        if (confirmar)
        {
            await _db.EliminarUsuarioAsync(fisio);
            ListaFisio.ItemsSource = await _db.ObtenerFisioterapeutasAsync();
        }
    }
}



