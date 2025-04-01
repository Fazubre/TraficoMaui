using Microsoft.Maui.Controls.Maps;

namespace TraficoCRFront.Views
{
    public partial class MapPage : ContentPage
    {


        public MapPage()
        {
            InitializeComponent();
        }

        public void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            MyMap.Pins.Clear();

            var position = e.Location;

            var pin = new Pin
            {
                Label = "Nuevo Pin",
                Address = $"Lat: {position.Latitude}, Lng: {position.Longitude}",
                Type = PinType.Place,
                Location = position
            };

            MyMap.Pins.Add(pin);
        }
    }
}