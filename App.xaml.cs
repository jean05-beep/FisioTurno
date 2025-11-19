using FisioTurno.Data;
using FisioTurno.Views;

namespace FisioTurno
{
    public partial class App : Application
    {
        private readonly AppDatabase _db;
        private readonly LoginPage _loginPage;

        public App(AppDatabase db, LoginPage loginPage)
        {
            InitializeComponent();

            _db = db;
            _loginPage = loginPage;

            // Inicializa BD
            Task.Run(async () => await _db.InitializeAsync()).Wait();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // MAUI moderno: la ventana se crea aquí
            var window = new Window(new NavigationPage(_loginPage));
            return window;
        }
    }
}


