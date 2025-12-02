using FisioTurno.Data;
using FisioTurno.Models;

namespace FisioTurno.Views
{
    public partial class CitaReservadaPage : ContentPage
    {
        private readonly AppDatabase _db;
        private readonly Usuario _usuario;

        // ✔ Constructor actualizado: ahora recibe el fisioterapeuta
        public CitaReservadaPage(
            string nombre,
            string fecha,
            string hora,
            string servicio,
            string fisioterapeuta,
            AppDatabase db,
            Usuario usuario)
        {
            InitializeComponent();

            lblNombre.Text = nombre;
            lblFecha.Text = fecha;
            lblHora.Text = hora;
            lblServicio.Text = servicio;
            lblFisioterapeuta.Text = fisioterapeuta;

            _db = db;
            _usuario = usuario;
        }

        private async void Entendido_Clicked(object sender, EventArgs e)
        {
            // Regresar al menú del paciente con db y usuario
            await Navigation.PushAsync(new MenuPacientePage(_db, _usuario));
        }
    }
}


