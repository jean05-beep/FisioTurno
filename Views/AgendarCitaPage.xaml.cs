using FisioTurno.Data;
using FisioTurno.Models;

namespace FisioTurno.Views
{
    public partial class AgendarCitaPage : ContentPage
    {
        // ⚠️ Nota: Cambia esta IP si usas el emulador (debe ser 10.0.2.2) 
        private const string UrlCrud = "http://192.168.100.1/FisioTurno/post.php";
        private readonly HttpClient cliente = new HttpClient();
        private readonly AppDatabase _db;
        private readonly Usuario _usuario;
        FileResult fotoTomada;

        public AgendarCitaPage(AppDatabase db, Usuario user)
        {
            InitializeComponent();
            _db = db;
            _usuario = user;

            // Mostrar el nombre del paciente automáticamente
            txtNombre.Text = user.Username;
            txtNombre.IsEnabled = false;

            // Fecha mínima permitida
            pickFecha.MinimumDate = DateTime.Today;

            // Servicio por defecto
            if (pickServicio.Items.Count > 0)
                pickServicio.SelectedIndex = 0;

            // Hora por defecto
            pickHora.Time = new TimeSpan(8, 0, 0);

        }

        private async void Reservar_Clicked(object sender, EventArgs e)
        {
            if (fotoTomada == null)
            {
                await DisplayAlert("Error", "Debes tomar una foto.", "OK");
                return;
            }

            // Convertir foto
            string base64Foto = await ConvertirFotoBase64(fotoTomada);

            // Validación del nombre
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                await DisplayAlert("Error", "Ingrese el nombre del paciente.", "OK");
                return;
            }

            string nombre = txtNombre.Text;
            DateTime fechaSeleccionada = pickFecha.Date;
            TimeSpan horaSeleccionada = pickHora.Time;

            // Combinar fecha y hora en un DateTime
            DateTime fechaCompleta = fechaSeleccionada.Add(horaSeleccionada);

            string fecha = fechaCompleta.ToString("dd/MM/yyyy");
            string hora = fechaCompleta.ToString("hh:mm tt");

            string servicio = pickServicio.SelectedItem?.ToString() ?? "Sin servicio";
            string notas = txtNotas.Text ?? "";

            // Crear cita completa
            var cita = new Cita
            {
                PacienteId = _usuario.Id,
                NombrePaciente = nombre,
                Fecha = fecha,
                Hora = hora,
                Servicio = servicio,
                Notas = notas,
                Foto = base64Foto
            };

            // Guardar cita en SQLite
            await _db.GuardarCitaAsync(cita);

            // Ir a pantalla confirmación
            await Navigation.PushAsync(
                new CitaReservadaPage(nombre, fecha, hora, servicio, _db, _usuario)
            );
        }

        private async void btnfoto_Clicked(object sender, EventArgs e)
        {
            try
            {
                fotoTomada = await MediaPicker.Default.CapturePhotoAsync();

                if (fotoTomada != null)
                {
                    var stream = await fotoTomada.OpenReadAsync();
                    imgFoto.Source = ImageSource.FromStream(() => stream);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }


        }
        private async Task<string> ConvertirFotoBase64(FileResult foto)
        {
            using var stream = await foto.OpenReadAsync();
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}










