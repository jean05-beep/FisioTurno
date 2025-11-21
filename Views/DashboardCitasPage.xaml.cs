using FisioTurno.Data;
using FisioTurno.Models;

namespace FisioTurno.Views;

public partial class DashboardCitasPage : ContentPage
{
    private readonly AppDatabase _db;
    private readonly Usuario _usuario;

    public DashboardCitasPage(AppDatabase db, Usuario usuario)
    {
        InitializeComponent();
        _db = db;
        _usuario = usuario;

        CargarCitas();
    }

    private async void CargarCitas()
    {
        var citas = await _db.ObtenerCitasPorNombreAsync(_usuario.Username);

        ListaCitas.ItemsSource = citas;
    }
}
