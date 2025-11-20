using FisioTurno.Data;
using FisioTurno.Views;

namespace FisioTurno
{
    public partial class App : Application
    {
        private readonly AppDatabase _db;
        private readonly LoginPage _login;

        public App(AppDatabase db, LoginPage login)
        {
            InitializeComponent();

            _db = db;
            _login = login;

            Task.Run(async () => await _db.InitializeAsync()).Wait();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // 👉 Arranca DIRECTAMENTE el Login, sin Shell ni MainPage
            return new Window(new NavigationPage(_login));
        }
    }
}



