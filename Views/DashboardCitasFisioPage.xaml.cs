using FisioTurno.Data;
using FisioTurno.Models;
using System.Collections.ObjectModel;

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

        // ===============================
        //   CARGAR CITAS DEL FISIOTERAPEUTA
        // ===============================
        private async void CargarCitas()
        {
            var todas = await _db.ObtenerCitasAsync();
            var hoy = DateTime.Today;

            // Filtrar citas de HOY + del fisioterapeuta logueado
            var citasHoy = todas
                .Where(c =>
                    c.FechaCompleta.Date == hoy &&
                    c.FisioterapeutaId == _usuario.Id)
                .OrderBy(c => c.FechaCompleta)
                .ToList();

            // Convertir foto Base64 → ImageSource
            foreach (var cita in citasHoy)
            {
                if (!string.IsNullOrEmpty(cita.Foto))
                {
                    try
                    {
                        byte[] bytes = Convert.FromBase64String(cita.Foto);
                        cita.FotoImagen = ImageSource.FromStream(() => new MemoryStream(bytes));
                    }
                    catch
                    {
                        cita.FotoImagen = null;
                    }
                }
            }

            ListaCitas.ItemsSource = citasHoy;
        }

        // ===============================
        //         ATENDER CITA
        // ===============================
        private async void Atender_Clicked(object sender, EventArgs e)
        {
            var boton = sender as Button;
            if (boton == null) return;

            var cita = boton.BindingContext as Cita;
            if (cita == null) return;

            bool confirmar = await DisplayAlert(
                "Confirmar",
                $"¿Deseas marcar como atendida la cita de:\n\n{cita.NombrePaciente}?",
                "Sí", "No");

            if (!confirmar)
                return;

            cita.Estado = "Atendida";

            await _db.ActualizarCitaAsync(cita);
            await DisplayAlert("✔ Éxito", "La cita ha sido marcada como atendida", "OK");

            CargarCitas();
        }
    }
}


