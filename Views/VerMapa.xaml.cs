using Microsoft.Maui.Controls.Maps;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Maps;
using System.Diagnostics;
using System.Text.Json;
using JsonElement = System.Text.Json.JsonElement;

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
        cargarPins();
    }

    private async void InicializarImagen()
    {
        string[] listaimagenes = { "dossiete.jpg", "tresdos.jpg","cerro_de_la_muerte.jpg","costanera_sur.jpg","monteverde.jpg","rohrmoser.jpg" };
        var random = new Random();
        int index = random.Next(listaimagenes.Length);
        Imagen.Source = listaimagenes[index];
    }
    
    
    private async void cargarPins()
    {
        MyMap.Pins.Clear();
        var reportesT = await obtenerReportesRequest();


        if (reportesT != null)
        {
            

            for (int i = 0; i < reportesT.Count; i++)
            {
                string tipoReporteStr = ObtenerDescripcionTipo(reportesT[i].tipoReporte);
                var pin = new Pin
                {
                    Label = new (reportesT[i].mensaje),

                    /*Address = $"Lat: {position.Latitude}, Lng: {position.Longitude}",*/
                    Address = tipoReporteStr,
                    Type = PinType.Place,
                    Location = new (reportesT[i].coordenadaY, reportesT[i].coordenadaX)
                };
                MyMap.Pins.Add(pin);
            }   
           

            
        }
        
        
    }
    
    
    
    private async Task<List<Reporte>> obtenerReportesRequest()
    {
        try
        {

            //se hace una solicitud a la api a la direccion getReportesPropios
            var response = await _client.GetAsync("getReportesTodos");

            // verifica si la respuesta fue exitosa
            if (response.IsSuccessStatusCode)
            {
                // se almacena en la variable contenido los valores JSON que devuelva la solicitud
                var contenido = await response.Content.ReadAsStringAsync();

                //en esta variable "reportes" se almacena una lista con objetos de tipo "Reporte"
                //se utiliza el metodo JsonSerializer.Deserialize<List<Reporte>> para que deserealice el json que se almacena en la variable contenido
                var reportesT = JsonSerializer.Deserialize<List<Reporte>>(contenido);

                

                return reportesT;

            }
            else
            {
                Console.WriteLine($"Error en la solicitud: {response.StatusCode}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Excepci�n durante la solicitud: {ex.Message}");
            return null;
        }
    }
    
    
    private string ObtenerDescripcionTipo(int tipoReporte)
    {
        return tipoReporte switch
        {
            1 => "Hueco",
            2 => "Calle Bloqueada",
            3 => "Deslizamiento",
            4 => "Inundación",
            5 => "Infraestructura Peligrosa",
            6 => "Otro",
            7 => "Caballo Muerto Bloqueando Calle",
            8 => "Vehículo Bloqueando Calle",
            _ => "Tipo Desconocido"
        };
    }
    
    
    
    
    
    
    
    
    private async void OnCrearReporteClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CrearReporte(_client, _user));
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

    private async void OnMisReportesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new VerReportes(_client, _user));
    }

}