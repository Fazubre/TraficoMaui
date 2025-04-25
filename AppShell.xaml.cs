using TraficoCRFront.Views;
using TraficoCRFront.Views.LogIn;
using TraficoCRFront.Views.Register;

namespace TraficoCRFront
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("LogIn", typeof(LogIn));
            Routing.RegisterRoute("MainPage", typeof(MainPage));
            Routing.RegisterRoute("CrearReporte", typeof(CrearReporte));
            Routing.RegisterRoute("Register", typeof(Register));  
        }
    }
}