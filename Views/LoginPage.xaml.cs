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

            // Guardar usuario global
            App.UsuarioActual = user;

            // =====================================================
            //     NAVEGACIÓN PROFESIONAL SEGÚN ROL
            // =====================================================
            switch (user.Rol.ToUpper())
            {
                case "ADMIN":
                    Application.Current.MainPage =
                        new NavigationPage(new AdminMenuPage(_db, user));
                    break;

                case "FISIOTERAPEUTA":
                    Application.Current.MainPage =
                        new NavigationPage(new MenuFisioPage(_db, user));
                    break;

                case "PACIENTE":
                default:
                    Application.Current.MainPage =
                        new NavigationPage(new MenuPacientePage(_db, user));
                    break;
            }
        }

        private async void IrRegistro_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage(_db));
        }
    }
}






