using FisioTurno.Data;
using FisioTurno.Views;

namespace FisioTurno
{
    public partial class App : Application
    {
        private readonly AppDatabase _db;

        public App()
        {
            InitializeComponent();

            // Crear base de datos
            _db = new AppDatabase();

            // Crear tablas
            Task.Run(async () => await _db.InitializeAsync()).Wait();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Página inicial
            var login = new LoginPage(_db);

            return new Window(new NavigationPage(login));
        }
    }
}




