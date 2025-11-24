using System;
using FisioTurno.Data;
using FisioTurno.Models;

namespace FisioTurno.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly AppDatabase _db;

        public LoginPage(AppDatabase db)
        {
            InitializeComponent();
            _db = db;
        }

        private async void Ingresar_Clicked(object sender, EventArgs e)
        {
            var u = txtUsuario.Text?.Trim();
            var p = txtPassword.Text?.Trim();

            if (string.IsNullOrWhiteSpace(u) || string.IsNullOrWhiteSpace(p))
            {
                await DisplayAlert("Error", "Ingrese usuario y contraseña", "OK");
                return;
            }

            Usuario? user = await _db.LoginAsync(u, p);

            if (user == null)
            {
                await DisplayAlert("Error", "Credenciales incorrectas", "OK");
                return;
            }

            await DisplayAlert("Bienvenido", $"Usuario: {user.Username}", "OK");

            // 👉 Guardar usuario global
            App.UsuarioActual = user;

            // 👉 OPCIÓN 1 (RECOMENDADA): reemplazar pantalla root, NO permite volver al login
            Application.Current.MainPage = new NavigationPage(new MenuPacientePage(_db, user));
            return;

            // 👉 OPCIÓN 2: Mantener navegación normal (puede volver al login)
            // await Navigation.PushAsync(new MenuPacientePage(_db, user));
        }

        private async void IrRegistro_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage(_db));
        }
    }
}






