using FisioTurno.Data;

namespace FisioTurno.Views;

public partial class DashboardCitasAdminPage : ContentPage
{
    private readonly AppDatabase _db;

    public DashboardCitasAdminPage(AppDatabase db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            ListaCitas.ItemsSource = await _db.ObtenerCitasAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}

}
