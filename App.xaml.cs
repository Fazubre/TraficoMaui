using TraficoCRFront.Views.LogIn;

namespace TraficoCRFront
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new LogIn();
        }
    }
}
