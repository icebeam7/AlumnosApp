using System;
using AlumnosApp.Servicios;
using AlumnosApp.Clases;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace AlumnosApp.Paginas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaginaDetalleTarea : ContentPage
	{
        Tarea dato;
        MemoryStream stream;

        public PaginaDetalleTarea (Tarea dato)
		{
			InitializeComponent ();

            ActualizarActivityIndicator(true);

            this.dato = dato;
            this.BindingContext = dato;

            ActualizarActivityIndicator(false);
        }

        private void ActualizarActivityIndicator(bool estado)
        {
            activityIndicator.IsRunning = estado;
            activityIndicator.IsEnabled = estado;
            activityIndicator.IsVisible = estado;
        }

        private void Ver_Clicked(object sender, EventArgs e)
        {
            if (dato.Id > 0)
            {
                var servicioStorage = new ServicioStorage();
                Device.OpenUri(new Uri(servicioStorage.GetFullDownloadTareaURL(dato.Id)));
                //var stream = await servicioStorage.DownloadTarea(dato.Id);
            }
        }

        private async void Enviar_Clicked(object sender, EventArgs e)
        {
            TareaAlumno dato = new TareaAlumno()
            {
                Alumno = App.Alumno,
                Tarea = this.dato,
                IdAlumno = App.Alumno.Id,
                IdTarea = this.dato.Id,
                ArchivoURL = string.Empty,
                Fecha = DateTime.Now,
                Calificacion = 0,
                Evaluado = false,
                Mensaje = string.Empty
            };

            await Navigation.PushAsync(new PaginaTareaAlumno(dato, true));
        }
    }
}