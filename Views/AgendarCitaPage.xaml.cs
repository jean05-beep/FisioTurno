using FisioTurno.Data;
using FisioTurno.Models;

namespace FisioTurno.Views
{
    public partial class AgendarCitaPage : ContentPage
    {
        private readonly AppDatabase _db;

        public AgendarCitaPage(AppDatabase db)
        {
            InitializeComponent();
            _db = db;

            // Fecha mínima permitida
            pickFecha.MinimumDate = DateTime.Today;

            // Servicio por defecto
            if (pickServicio.Items.Count > 0)
                pickServicio.SelectedIndex = 0;

            // Asignar hora por defecto (para evitar null)
            pickHora.Time = new TimeSpan(8, 0, 0);
        }

        private async void Reservar_Clicked(object sender, EventArgs e)
        {
            // Validación del nombre
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                await DisplayAlert("Error", "Ingrese el nombre del paciente.", "OK");
                return;
            }

            // Obtener valores correctamente
            string nombre = txtNombre.Text;
            string fecha = pickFecha.Date.ToString("dd/MM/yyyy");

            // ✔ CORREGIDO: formato correcto de hora
            string hora = DateTime.Today
                            .Add(pickHora.Time)
                            .ToString("hh:mm tt");

            string servicio = pickServicio.SelectedItem?.ToString() ?? "Sin servicio";
            string notas = txtNotas.Text ?? "";

            // Crear cita
            var cita = new Cita
            {
                NombrePaciente = nombre,
                Fecha = fecha,
                Hora = hora,
                Servicio = servicio,
                Notas = notas
            };

            // Guardar
            await _db.GuardarCitaAsync(cita);

            // Navegar a pantalla de confirmación
            await Navigation.PushAsync(
                new CitaReservadaPage(nombre, fecha, hora, servicio)
            );
        }
    }
}




