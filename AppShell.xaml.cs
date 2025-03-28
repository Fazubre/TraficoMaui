using TraficoCRFront.Views;
using TraficoCRFront.Views.LogIn;

namespace TraficoCRFront
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("LogIn", typeof(LogIn));
            Routing.RegisterRoute("MainPage", typeof(MainPage));
            Routing.RegisterRoute("MapPage", typeof(MapPage));
        }
    }
}