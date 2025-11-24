using FisioTurno.Data;
using FisioTurno.Models;
using FisioTurno.Views;

namespace FisioTurno
{
    public partial class App : Application
    {
        public static AppDatabase Database { get; private set; } = null!;
        public static Usuario? UsuarioActual { get; set; }

        public App()
        {
            InitializeComponent();

            // Crear base de datos global
            Database = new AppDatabase();

            // Crear tablas e inicializar admin (no bloquear el hilo principal)
            Task.Run(async () => await Database.InitializeAsync());
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Crear ventana principal con Login
            var login = new LoginPage(Database);

            return new Window(new NavigationPage(login));
        }
    }
}





