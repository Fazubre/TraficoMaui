using Microsoft.Maui.Controls.Maps;

namespace TraficoCRFront.Views;

public partial class VerMapa : ContentPage
{
    private readonly HttpClient _client;
    private readonly datosUsuario _user;
    public VerMapa(HttpClient client, datosUsuario user)
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

    public void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        MyMap.Pins.Clear();

        var position = e.Location;

        var pin = new Pin
        {
            Label = "Latitud y Longitud del Lugar:",
            Address = $"Lat: {position.Latitude}, Lng: {position.Longitude}",
            Type = PinType.Place,
            Location = position
        };

        MyMap.Pins.Add(pin);
    }


    private async void OnVerPerfilClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new VerPerfil(_client, _user));
    }

    private async void OnMisReportesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new VerReportes(_client, _user));
    }

}