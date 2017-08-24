using System;
using AlumnosApp.Servicios;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace AlumnosApp.Paginas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaginaPerfil : ContentPage
	{
        MemoryStream stream;

        public PaginaPerfil ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.BindingContext = App.Alumno;
        }

        private void ActualizarActivityIndicator(bool estado)
        {
            activityIndicator.IsRunning = estado;
            activityIndicator.IsEnabled = estado;
            activityIndicator.IsVisible = estado;
        }

        private async void Foto_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Foto del alumno", "Cancelar", null, "Usar cámara", "Seleccionar de la biblioteca");

            bool camara = action.Contains("cámara");
            var file = await ServicioImagen.TomarFoto(camara);

            imgFoto.Source = ImageSource.FromStream(() => {
                var stream = file.GetStream();
                this.stream = new MemoryStream();
                stream.CopyTo(this.stream);
                stream.Seek(0, SeekOrigin.Begin);
                file.Dispose();
                return stream;
            });
        }

        private async void Guardar_Clicked(object sender, EventArgs e)
        {
            ActualizarActivityIndicator(true);
            stream?.Seek(0, SeekOrigin.Begin);
            await ServicioWebApi.UpdateAlumno(App.Alumno, stream);
            App.Alumno = await ServicioWebApi.GetAlumno(App.Alumno.Id);
            ActualizarActivityIndicator(false);

            await DisplayAlert("Información", "Perfil actualizado", "OK");
            await Navigation.PopAsync();
        }
    }
}