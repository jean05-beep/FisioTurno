namespace FisioTurno.Views;

public partial class CitaReservadaPage : ContentPage
{
    public CitaReservadaPage(string nombre, string fecha, string hora, string servicio)
    {
        InitializeComponent();

        lblNombre.Text = nombre;
        lblFecha.Text = fecha;
        lblHora.Text = hora;
        lblServicio.Text = servicio;
    }

    private async void Entendido_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}
