using System;
using AlumnosApp.Servicios;
using AlumnosApp.Clases;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace AlumnosApp.Paginas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaginaTareaAlumno : ContentPage
	{
        TareaAlumno dato;
        MemoryStream stream;
        bool esNuevo;

        public PaginaTareaAlumno(TareaAlumno dato, bool esNuevo)
        {
            InitializeComponent();
            this.dato = dato;
            this.esNuevo = esNuevo;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (!esNuevo)
            {
                ActualizarActivityIndicator(true);
                dato = await ServicioWebApi.GetTareaAlumno(dato.IdTarea, dato.IdAlumno);
                ActualizarActivityIndicator(false);
            }

            this.BindingContext = dato;
        }

        private void ActualizarActivityIndicator(bool estado)
        {
            activityIndicator.IsRunning = estado;
            activityIndicator.IsEnabled = estado;
            activityIndicator.IsVisible = estado;
        }

        private async void SeleccionarArchivo_Clicked(object sender, EventArgs e)
        {
            ServicioFilePicker servicioFilePicker = new ServicioFilePicker();
            stream = await servicioFilePicker.GetFile();
        }

        private void VerTarea_Clicked(object sender, EventArgs e)
        {
            var servicioStorage = new ServicioStorage();
            Device.OpenUri(new Uri(servicioStorage.GetFullDownloadTareaURL(dato.IdTarea)));
        }

        private async void VerRespuesta_Clicked(object sender, EventArgs e)
        {
            var servicioStorage = new ServicioStorage();
            Device.OpenUri(new Uri(servicioStorage.GetFullDownloadTareaAlumnoURL(dato.IdTarea, dato.IdAlumno)));
        }

        private async void EnviarTarea_Clicked(object sender, EventArgs e)
        {
            if (!dato.Evaluado)
            {
                DateTime fechaHoy = DateTime.Now.Date;

                if (fechaHoy <= dato.Tarea.FechaLimite)
                {
                    if (stream != null || !esNuevo)
                    {
                        ActualizarActivityIndicator(true);

                        if (esNuevo)
                            await ServicioWebApi.AddTareaAlumno(dato, stream);
                        else
                            await ServicioWebApi.UpdateTareaAlumno(dato, stream);
                        ActualizarActivityIndicator(false);

                        await DisplayAlert("Información", "Dato registrado con éxito", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Información", "Debes agregar un archivo primero", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Información", "La tarea ya no puede ser enviada porque ha pasado el límite establecido", "OK");
                }
            }
            else
            {
                await DisplayAlert("Información", "La tarea ya no puede ser enviada porque ya ha sido evaluada por el profesor", "OK");
            }
        }
    }
}