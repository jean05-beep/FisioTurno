using FisioTurno.Data;
using FisioTurno.Models;
using FisioTurno.Views;

namespace FisioTurno
{
    public partial class App : Application
    {
        public static AppDatabase Database { get; private set; }
        public static Usuario UsuarioActual { get; set; }

        public App()
        {
            InitializeComponent();

            // Crear base de datos global
            Database = new AppDatabase();

            // Crear tablas
            Task.Run(async () => await Database.InitializeAsync()).Wait();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Abrir página inicial (LOGIN) enviando la base de datos
            var login = new LoginPage(Database);

            return new Window(new NavigationPage(login));
        }
    }
}




