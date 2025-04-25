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