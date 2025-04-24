//using static Android.Gms.Common.Apis.Api;



namespace TraficoCRFront.Views;

public partial class VerPerfil : ContentPage
{

     private readonly HttpClient _client;
     private readonly datosUsuario _user;
	public VerPerfil(HttpClient client, datosUsuario user)
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

    private async void OnCerrarSesionClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Cerrar Sesi�n", "La sesi�n se ha cerrado con �xito", "OK");

        await Navigation.PushAsync(new TraficoCRFront.Views.LogIn.LogIn(_client,_user));
    }
}