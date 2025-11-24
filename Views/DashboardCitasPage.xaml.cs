using FisioTurno.Data;
using FisioTurno.Models;

namespace FisioTurno.Views
{
    public partial class DashboardCitasPage : ContentPage
    {
        private readonly AppDatabase _db;
        private readonly Usuario _usuario;

        public DashboardCitasPage(AppDatabase db, Usuario usuario)
        {
            InitializeComponent();
            _db = db;
            _usuario = usuario;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CargarCitas();
        }

        private async Task CargarCitas()
        {
            // ✔ Mejor usar ID en vez de nombre
            var citas = await _db.ObtenerCitasPorIdPacienteAsync(_usuario.Id);

            if (citas == null || citas.Count == 0)
            {
                ListaCitas.ItemsSource = null;
                await DisplayAlert("Sin citas", "No tiene citas registradas.", "OK");
                return;
            }

            ListaCitas.ItemsSource = citas;
        }
    }
}

