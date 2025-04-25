

namespace TraficoCRFront.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient _client;
        private readonly datosUsuario _user;

        public MainPage(HttpClient client, datosUsuario user)
        {
            InitializeComponent();
            _client = client;
            _user = user;
        }

        private async void OnCrearReporteClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MapPage(_client,_user));
        }

        private async void OnInicioClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage(_client,_user));
        }

        private async void OnVerMapaClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VerMapa(_client,_user));
        }

        private async void OnVerPerfilClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VerPerfil(_client,_user));
        }

    }

}
