using FisioTurno.Data;
using FisioTurno.Models;
using System.Collections.Generic;

namespace FisioTurno.Views
{
    public partial class AgendarCitaPage : ContentPage
    {
        // API PHP
        private const string UrlCrud = "http://192.168.100.1/FisioTurno/post.php";
        private readonly HttpClient cliente = new HttpClient();

        private readonly AppDatabase _db;
        private readonly Usuario _usuario;
        private List<Usuario> _fisioterapeutas = new();

        FileResult fotoTomada;

        public AgendarCitaPage(AppDatabase db, Usuario user)
        {
            InitializeComponent();

            _db = db;
            _usuario = user;

            // Nombre del paciente
            txtNombre.Text = user.NombreCompleto;
            txtNombre.IsEnabled = false;

            pickFecha.MinimumDate = DateTime.Today;

            // Cargar fisioterapeutas
            CargarFisioterapeutas();

            // Cargar horas disponibles
            CargarHorasDisponibles();
        }

        // ======================================================
        //   Cargar fisioterapeutas desde SQLite
        // ======================================================
        private async void CargarFisioterapeutas()
        {
            _fisioterapeutas = await _db.ObtenerFisioterapeutasAsync();

            pickFisio.ItemsSource = _fisioterapeutas;
            pickFisio.ItemDisplayBinding = new Binding("NombreCompleto");
        }

        // ======================================================
        //    Generar horarios 08:00 - 17:00 cada 30 min
        // ======================================================
        private void CargarHorasDisponibles()
        {
            var horas = new List<string>();

            for (int h = 8; h <= 17; h++)
            {
                horas.Add($"{h}:00");
                horas.Add($"{h}:30");
            }

            pickHora.ItemsSource = horas;
        }

        // ======================================================
        //                 Tomar foto
        // ======================================================
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

        // ======================================================
        //                 Reservar cita
        // ======================================================
        private async void Reservar_Clicked(object sender, EventArgs e)
        {
            if (fotoTomada == null)
            {
                await DisplayAlert("Error", "Debes tomar una foto.", "OK");
                return;
            }

            if (pickFisio.SelectedItem is null)
            {
                await DisplayAlert("Error", "Seleccione un fisioterapeuta.", "OK");
                return;
            }

            if (pickHora.SelectedItem is null)
            {
                await DisplayAlert("Error", "Seleccione una hora.", "OK");
                return;
            }

            string base64Foto = await ConvertirFotoBase64(fotoTomada);

            var fisio = (Usuario)pickFisio.SelectedItem;

            // Formar fecha completa
            string horaText = pickHora.SelectedItem.ToString();
            DateTime fechaCompleta = DateTime.Parse($"{pickFecha.Date:yyyy-MM-dd} {horaText}");

            // Validar disponibilidad
            bool ocupado = await _db.FisioOcupadoAsync(fisio.Id, fechaCompleta);
            if (ocupado)
            {
                await DisplayAlert("No disponible",
                    "Este fisioterapeuta ya tiene una cita en esa hora.", "OK");
                return;
            }

            // Crear objeto Cita
            var cita = new Cita
            {
                PacienteId = _usuario.Id,
                FisioterapeutaId = fisio.Id,
                NombreFisioterapeuta = fisio.NombreCompleto,

                NombrePaciente = _usuario.NombreCompleto,
                FechaCompleta = fechaCompleta,
                Fecha = fechaCompleta.ToString("dd/MM/yyyy"),
                Hora = fechaCompleta.ToString("HH:mm"),
                Servicio = pickServicio.SelectedItem?.ToString() ?? "Sin servicio",
                Notas = txtNotas.Text ?? "",
                Estado = "Pendiente",
                Foto = base64Foto
            };

            // Guardar internamente en SQLite
            await _db.GuardarCitaAsync(cita);

            // Ir a pantalla de confirmación
            await Navigation.PushAsync(
                new CitaReservadaPage(
                    cita.NombrePaciente,
                    cita.NombreFisioterapeuta,
                    cita.Fecha,
                    cita.Hora,
                    cita.Servicio,
                    _db,
                    _usuario
                )
            );
        }
    }
}











