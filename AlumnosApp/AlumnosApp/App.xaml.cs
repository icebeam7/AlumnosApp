using AlumnosApp.Clases;
using Xamarin.Forms;

namespace AlumnosApp
{
    public partial class App : Application
    {
        public static Alumno Alumno;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage (new AlumnosApp.Paginas.PaginaLogin());
        }
    }
}
