using FisioTurno.Data;
using FisioTurno.Models;
using Microsoft.Maui.Media;

namespace FisioTurno.Views
{
    public partial class MenuFisioPage : ContentPage
    {
        private readonly AppDatabase _db;
        private readonly Usuario _usuario;

        public MenuFisioPage(AppDatabase db, Usuario usuario)
        {
            InitializeComponent();
            _db = db;
            _usuario = usuario;

            lblUsuario.Text = $"Bienvenido, {_usuario.NombreCompleto}";
        }

        // Ver citas del día
        private async void VerCitas_Tapped(object sender, EventArgs e )
        {
            await Navigation.PushAsync(
                new DashboardCitasFisioPage(_db, _usuario )
            );
        }

        // Cerrar sesión
        private void CerrarSesion_Tapped(object sender, EventArgs e)
        {
            App.UsuarioActual = null;
            Application.Current.Windows[0].Page =
                new NavigationPage(new LoginPage(_db));
        }

        private async void btnfoto_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Verificar si la cámara está disponible
                if (!MediaPicker.Default.IsCaptureSupported)
                {
                    await DisplayAlert("Error", "La cámara no está soportada en este dispositivo", "OK");
                    return;
                }

                // Abrir la cámara
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo == null)
                    return;

                // Guardar en carpeta temporal
                var stream = await photo.OpenReadAsync();

                // Mostrar en un control Image

                imgFoto.Source = ImageSource.FromStream(() => stream);

                // Si necesitas guardar como archivo local:
                string localPath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                using var localStream = File.OpenWrite(localPath);
                stream.Position = 0;
                await stream.CopyToAsync(localStream);

                Console.WriteLine("Foto guardada en: " + localPath);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }

        }
        public async Task<string> ConvertirFotoBase64(FileResult photo)
        {
            using var stream = await photo.OpenReadAsync();
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}
