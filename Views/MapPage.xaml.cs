using Microsoft.Maui.Controls.Maps;
using System.Diagnostics;

namespace TraficoCRFront.Views
{
    public partial class MapPage : ContentPage
    {
        
        private Location _locacionInci;

        private readonly HttpClient _client;
        private readonly datosUsuario _user;

        public MapPage(HttpClient client, datosUsuario user)
        {
            InitializeComponent();
            _client = client;
            _user = user;
        }

        private void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            MyMap.Pins.Clear();

            _locacionInci = e.Location;

            var pin = new Pin
            {
                Label = "Incidente",
                /*Address = $"Lat: {position.Latitude}, Lng: {position.Longitude}",*/
                Address = "Ubicación seleccionada",
                Type = PinType.Place,
                Location = _locacionInci
            };

            MyMap.Pins.Add(pin);
            
            DisplayAlert("Ubicación seleccionada", $"Lat: {e.Location.Latitude}, Lng: {e.Location.Longitude}", "OK");
        }
        
        private async void OnReportIncident(object sender, EventArgs e)
        {
           
            if (_locacionInci == null)
            {
                await DisplayAlert("Error", "Seleccione una ubicación en el mapa.", "OK");
                return;
            }

            string description = IncidentDescription.Text;
            string incidentType = (string)IncidentType.SelectedItem;

            if (string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(incidentType))
            {
                await DisplayAlert("Error", "Ingrese todos los datos del incidente.", "OK");
                return;
            }

            // Simulación de envío del reporte (se podría integrar con una API)
            Debug.WriteLine($"Reporte enviado:\nTipo: {incidentType}\nDescripción: {description}\nUbicación: {_locacionInci.Latitude}, {_locacionInci.Longitude}");

            await DisplayAlert("Reporte Enviado", "El incidente ha sido reportado con éxito.", "OK");

            // Limpiar campos después del envío
            IncidentDescription.Text = string.Empty;
            IncidentType.SelectedIndex = -1;
            _locacionInci = null;
            MyMap.Pins.Clear();
        }
        
        private async void OnCrearReporteClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MapPage(_client,_user));
        }
        
        private async void OnInicioClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage(_client,_user));
        }
        
    }
}