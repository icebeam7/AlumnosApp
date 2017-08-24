using System;
using AlumnosApp.Servicios;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlumnosApp.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaLogin : ContentPage
    {
        public PaginaLogin()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ActualizarActivityIndicator(false);
        }

        private void ActualizarActivityIndicator(bool estado)
        {
            activityIndicator.IsRunning = estado;
            activityIndicator.IsEnabled = estado;
            activityIndicator.IsVisible = estado;
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            ActualizarActivityIndicator(true);
            App.Alumno = await ServicioWebApi.GetAlumnoByCredentials(txtUsuario.Text, txtPassword.Text);
            ActualizarActivityIndicator(false);

            if (App.Alumno != null)
                await Navigation.PushAsync(new PaginaMenu());
            else
                await DisplayAlert("Error", "Alumno no encontrado. Revisa usuario y contraseña", "OK");
        }
    }
}