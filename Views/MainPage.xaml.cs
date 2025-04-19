namespace TraficoCRFront.Views
{
    public partial class MainPage : ContentPage
    {


        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCrearReporteClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MapPage());
        }

        private async void OnInicioClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private async void OnVerMapaClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VerMapa());
        }

        private async void OnVerReportesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VerReportes());
        }
    }

}
