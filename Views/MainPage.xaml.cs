namespace TraficoCRFront.Views
{
    public partial class MainPage : ContentPage
    {
        

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnStartButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MapPage());
        }
<<<<<<< Updated upstream
=======
        
        private async void OnInicioClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private async void OnVerMapaClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VerMapa());
        }
>>>>>>> Stashed changes
    }

}
