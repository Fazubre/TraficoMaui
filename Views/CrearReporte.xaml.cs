using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Maps;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;

namespace TraficoCRFront.Views
{
    public partial class CrearReporte : ContentPage
    {
        
        
        private Location _locacionInci;

        private readonly HttpClient _client;
        private readonly datosUsuario _user;
        

        public CrearReporte(HttpClient client, datosUsuario user)
        {
            InitializeComponent();
            InicializarMapaAsync();
            
            _client = client;
            _user = user;
        }
        
        
        
        // Funcion que corre al iniciar CrearReporte. Agarra la ubi actual del dispositivo para presentarla en el mapa. Si no muestra CR
        private async void InicializarMapaAsync()
        {
            try
            {
                
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(
                        new GeolocationRequest
                        {
                            DesiredAccuracy = GeolocationAccuracy.Medium,
                            Timeout = TimeSpan.FromSeconds(10)
                        });
                }

                if (location != null)
                {
                    var position = new Location(location.Latitude, location.Longitude);
                    MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                        new Location(position.Latitude, position.Longitude),
                        Distance.FromKilometers(0.5)));
                }
                else
                {
                    var defaultLocation = new Location(9.6, -84.3534);
                    MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(defaultLocation, Distance.FromKilometers(3)));
                }
            }
            catch (Exception)
            {
                var defaultLocation = new Location(9.6, -84.3534);
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(defaultLocation, Distance.FromKilometers(3)));
            }
        }
        
        
        
        
        
        
        
        // Funcion para que usuario ponga un pin en el mapa y se guarda la ubicacion
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

            bool enviado = await EnviarReporte(incidentType, description, _locacionInci);

            if (enviado)
            {
                await DisplayAlert("Reporte Enviado", "El incidente ha sido reportado con éxito.", "OK");

                // Limpiar campos después del envío
                IncidentDescription.Text = string.Empty;
                IncidentType.SelectedIndex = -1;
                _locacionInci = null;
                MyMap.Pins.Clear();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo enviar el reporte. Intenta nuevamente.", "OK");
            }

        }
        
        private async Task<bool> EnviarReporte(string tipo, string descripcion, Location ubicacion)
        {
            try
            {
                // Convertir el tipo de incidente (string) a int
                // Podés usar un diccionario si los tipos tienen un mapeo específico
                int tipoReporteInt = ConvertirTipoAInt(tipo);
        
                var payload = new
                {
                    mensaje = descripcion,
                    tipoReporte = tipoReporteInt,
                    CoordenadaX = ubicacion.Longitude, // Longitud es X
                    CoordenadaY = ubicacion.Latitude   // Latitud es Y
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync("/crearReporte", content);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine($"Error al enviar reporte: {response.StatusCode}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Excepción al enviar el reporte: " + ex.Message);
                return false;
            }
        }
        
        
        private int ConvertirTipoAInt(string tipo)
        {
            return tipo.ToLowerInvariant() switch
            {
                "hueco" => 1,
                "calle bloqueada" => 2,
                "deslizamiento" => 3,
                "inundacion" => 4,
                "infraestructura peligrosa" => 5,
                "vehiculo bloqueando calle" => 8,
                "caballo muerto" => 7,
                "otro" => 6,
                _ => 0 // tipo desconocido
            };
        }
        
        
        
        private async void OnCrearReporteClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CrearReporte(_client,_user));
        }
        
        private async void OnInicioClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage(_client,_user));
        }
        
    }
}