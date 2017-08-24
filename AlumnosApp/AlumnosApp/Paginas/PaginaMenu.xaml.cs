using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlumnosApp.Paginas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaginaMenu : ContentPage
	{
		public PaginaMenu ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.BindingContext = App.Alumno;
        }

        private async void TareasPendientes_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PaginaTareasPendientes());
        }

        private async void MisTareas_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PaginaListaTareasAlumno());
        }

        private async void MiPerfil_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PaginaPerfil());
        }
    }
}