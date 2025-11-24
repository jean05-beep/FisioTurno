using FisioTurno.Data;
using FisioTurno.Models;

namespace FisioTurno.Views
{
    public partial class CitaReservadaPage : ContentPage
    {
        private readonly AppDatabase _db;
        private readonly Usuario _usuario;

        public CitaReservadaPage(string nombre, string fecha, string hora, string servicio,
                                 AppDatabase db, Usuario usuario)
        {
            InitializeComponent();

            lblNombre.Text = nombre;
            lblFecha.Text = fecha;
            lblHora.Text = hora;
            lblServicio.Text = servicio;

            _db = db;
            _usuario = usuario;
        }

        private async void Entendido_Clicked(object sender, EventArgs e)
        {
            // 👉 Regresar al menú del paciente con db y usuario correcto
            await Navigation.PushAsync(new MenuPacientePage(_db, _usuario));

            // Si quieres REEMPLAZAR completamente la pantalla (sin atrás):
            // Application.Current.MainPage = new NavigationPage(new MenuPacientePage(_db, _usuario));
        }
    }
}

