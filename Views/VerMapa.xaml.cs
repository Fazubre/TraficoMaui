using Microsoft.Maui.Controls.Maps;

namespace TraficoCRFront.Views;

public partial class VerMapa : ContentPage
{
    public VerMapa()
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

    private async void OnVerReportesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new VerReportes());
    }
}