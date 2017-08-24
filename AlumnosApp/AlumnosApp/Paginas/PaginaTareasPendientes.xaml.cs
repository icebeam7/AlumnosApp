using System;
using AlumnosApp.Servicios;
using AlumnosApp.Clases;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlumnosApp.Paginas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaginaTareasPendientes : ContentPage
	{
		public PaginaTareasPendientes ()
		{
			InitializeComponent ();
		}

        private void ActualizarActivityIndicator(bool estado)
        {
            activityIndicator.IsRunning = estado;
            activityIndicator.IsEnabled = estado;
            activityIndicator.IsVisible = estado;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            ActualizarActivityIndicator(true);
            lsvTareas.ItemsSource = await ServicioWebApi.GetTareasNoRealizadasByAlumno(App.Alumno.Id);
            ActualizarActivityIndicator(false);
        }

        private async void lsvTareas_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                Tarea dato = (Tarea)e.SelectedItem;
                await Navigation.PushAsync(new PaginaDetalleTarea(dato));
            }
            catch (Exception ex)
            {
            }
        }

    }
}