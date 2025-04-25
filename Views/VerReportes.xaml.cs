
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;

using System.Diagnostics;
using System.Text.Json;
//using GoogleGson;
using JsonElement = System.Text.Json.JsonElement;

namespace TraficoCRFront.Views;

public partial class VerReportes : ContentPage
{
    private readonly HttpClient _client;
    private readonly datosUsuario _user;
    public VerReportes(HttpClient client, datosUsuario user)
	{
		InitializeComponent();
        _client = client;
        _user = user;
        InicializarImagen();
        cargarReportes();
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

    private async void OnVerDetallesClicked(object sender, EventArgs e)
    {
        // se obtiene el boton que se clickeo
        var button = sender as Button;

        // se obtiene el commandparameter, el cual es el reporte al que esta ligado el boton
        var reporte = button.CommandParameter as Reporte;

        if (reporte != null)
        {
            var popup = new PopupDetallesReportes(reporte);
            this.ShowPopup(popup);
        }
    }

    private async void cargarReportes()
    {
        var reportes = await obtenerReportesRequest();


        if (reportes != null)
        {
            

            for (int i = 0; i < reportes.Count; i++)
            {
                var datosDistritoIdByRequest = await getDistritoByIdRequest(reportes[i].distritoId.ToString());
                var datosCantonIdByRequest = await getCantonByIdRequest(datosDistritoIdByRequest.idCanton.ToString());

                reportes[i].nomDistrito = datosDistritoIdByRequest.nombreDistrito;
                reportes[i].nomCanton = datosCantonIdByRequest.nombreCanton;
                reportes[i].nomProvincia = await getProvinciaByIdRequest(datosCantonIdByRequest.idProvincia.ToString());
               
            }
           

            ReportesCollectionView.ItemsSource = reportes;
        }
        else
        {
            var listaVaciaMostrar = new List<Reporte>();

            ReportesCollectionView.ItemsSource = listaVaciaMostrar;
            Debug.WriteLine("aqui estoy 1");
            Debug.WriteLine("Reportes nulos");
        }
        
    }

    private async void OnMisReportesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new VerReportes(_client, _user));
    }

    private async Task<string> getProvinciaByIdRequest(string idProvincia)
    {
        try
        {
            string id = idProvincia;
            Debug.WriteLine("id de la provincia"+id);

            // Construir la URL con el par�metro 'id'
            string url = $"getProvinciaById?id={id}";

            Debug.WriteLine(url);
            Debug.WriteLine(id);


            var response = await _client.GetAsync(url);


            if (response.IsSuccessStatusCode)
            {

                var contenido = await response.Content.ReadAsStringAsync();


                // Deserializar el JSON en un JsonElement
                JsonDocument doc = JsonDocument.Parse(contenido);

                JsonElement root = doc.RootElement;

                string nombreProvincia = root.GetProperty("nombreProvincia").GetString();

                return nombreProvincia;

            }
            else
            {
                Console.WriteLine($"Error en la solicitud: aaa {response.StatusCode}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Excepci�n durante la solicitud: {ex.Message}");
            return null;
        }
    }

    private async Task<(string nombreCanton, int idProvincia)> getCantonByIdRequest(string idCanton)
    {
        try
        {
            string id = idCanton;

            // Construir la URL con el par�metro 'id'
            string url = $"getCantonById?id={id}";

            Debug.WriteLine(url);
            Debug.WriteLine(id);


            var response = await _client.GetAsync(url);


            if (response.IsSuccessStatusCode)
            {

                var contenido = await response.Content.ReadAsStringAsync();
            

                // Deserializar el JSON en un JsonElement
                JsonDocument doc = JsonDocument.Parse(contenido);
               
                JsonElement root = doc.RootElement;

                int idProvincia = root.GetProperty("idProvincia").GetInt32();
              

                string nombreCanton = root.GetProperty("nombreCanton").GetString();

                return (nombreCanton, idProvincia);

            }
            else
            {
                Console.WriteLine($"Error en la solicitud: aaa {response.StatusCode}");
                return (null, 0);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Excepci�n durante la solicitud: {ex.Message}");
            return (null, 0);
        }
    }

    private async Task<(string nombreDistrito, int idCanton)> getDistritoByIdRequest(string idDistrito)
    {
        try
        {
            string id = idDistrito; 

            // Construir la URL con el par�metro 'id'
            string url = $"getDistritoById?id={id}";

            Debug.WriteLine(url);
            Debug.WriteLine(id);


            var response = await _client.GetAsync(url);

           
            if (response.IsSuccessStatusCode)
            {

               


                var contenido = await response.Content.ReadAsStringAsync();
              

                // Deserializar el JSON en un JsonElement
                JsonDocument doc = JsonDocument.Parse(contenido);
                
                JsonElement root = doc.RootElement;
           


                int idCanton = root.GetProperty("idCanton").GetInt32();
               
               
                string nombreDistrito = root.GetProperty("nombreDistrito").GetString();
              
                

                return (nombreDistrito,idCanton);

            }
            else
            {
                Console.WriteLine($"Error en la solicitud: aaa {response.StatusCode}");
                return (null,0);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Excepci�n durante la solicitud: {ex.Message}");
            return (null,0);
        }
    }

    private async Task<List<Reporte>> obtenerReportesRequest()
    {
        try
        {

            //se hace una solicitud a la api a la direccion getReportesPropios
            var response = await _client.GetAsync("getReportesPropios");

            // verifica si la respuesta fue exitosa
            if (response.IsSuccessStatusCode)
            {
                // se almacena en la variable contenido los valores JSON que devuelva la solicitud
                var contenido = await response.Content.ReadAsStringAsync();

                //en esta variable "reportes" se almacena una lista con objetos de tipo "Reporte"
                //se utiliza el metodo JsonSerializer.Deserialize<List<Reporte>> para que deserealice el json que se almacena en la variable contenido
                var reportes = JsonSerializer.Deserialize<List<Reporte>>(contenido);

                

                return reportes;

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
}