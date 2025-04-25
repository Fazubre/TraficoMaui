
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
            InicializarImagen();
        }

        private async void InicializarImagen()
        {
            string[] listaimagenes = { "dossiete.jpg", "tresdos.jpg","cerro_de_la_muerte.jpg","costanera_sur.jpg","monteverde.jpg","rohrmoser.jpg" };
            var random = new Random();
            int index = random.Next(listaimagenes.Length);
            Imagen.Source = listaimagenes[index];
        }
        private async void OnCrearReporteClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MapPage(_client, _user));
        }

        private async void OnInicioClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage(_client, _user));
        }

        private async void OnVerMapaClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VerMapa(_client, _user));
        }

        private async void OnVerPerfilClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VerPerfil(_client, _user));
        }
        private async void OnAdminClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Admin(_client, _user));
        }

        private async void OnMisReportesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VerReportes(_client, _user));
        }

        private async void OnVerMisReportesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VerReportes(_client, _user));
        }

        

    }

}
