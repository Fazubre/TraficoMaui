using TraficoCRFront.Views.LogIn;
using System.Net.Http;

namespace TraficoCRFront
{
    public partial class App : Application
    {
        public App(HttpClient httpClient) // <- ahora el HttpClient se inyecta aquí
        {
            InitializeComponent();
            MainPage = new NavigationPage(new LogIn(httpClient));
        }
    }
}
