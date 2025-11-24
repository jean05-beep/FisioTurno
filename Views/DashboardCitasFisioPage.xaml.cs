using FisioTurno.Data;
using FisioTurno.Models;

namespace FisioTurno.Views
{
    public partial class DashboardCitasFisioPage : ContentPage
    {
        private readonly AppDatabase _db;
        private readonly Usuario _usuario;

        public DashboardCitasFisioPage(AppDatabase db, Usuario usuario)
        {
            InitializeComponent();
            _db = db;
            _usuario = usuario;

            CargarCitas();
        }

        // Cargar solo citas del día actual
        private async void CargarCitas()
        {
            var todas = await _db.ObtenerCitasAsync();

            var hoy = DateTime.Today;

            var citasHoy = todas
                .Where(c => c.FechaCompleta.Date == hoy)
                .OrderBy(c => c.FechaCompleta)
                .ToList();

            ListaCitas.ItemsSource = citasHoy;
        }

        // Atender cita
        private async void Atender_Clicked(object sender, EventArgs e)
        {
            var boton = sender as Button;
            if (boton == null) return;

            var cita = boton.BindingContext as Cita;
            if (cita == null) return;

            bool confirmar = await DisplayAlert(
                "Confirmar",
                $"¿Marcar la cita de '{cita.NombrePaciente}' como Atendida?",
                "Sí", "No");

            if (!confirmar) return;

            cita.Estado = "Atendida";

            await _db.ActualizarCitaAsync(cita);
            await DisplayAlert("Éxito", "Cita marcada como atendida", "OK");

            CargarCitas();
        }
    }
}
