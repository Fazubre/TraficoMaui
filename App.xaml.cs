using TraficoCRFront.Views.LogIn;

namespace TraficoCRFront
{
    public partial class App : Application
    {
        public App()
        {
            var httpClient = DependencyService.Get<HttpClient>();
            
            InitializeComponent();
            MainPage = new NavigationPage(new LogIn(httpClient));
        }
    }
}
