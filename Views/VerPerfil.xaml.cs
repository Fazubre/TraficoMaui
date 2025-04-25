//using static Android.Gms.Common.Apis.Api;




using System.Diagnostics;
using System.Text.Json;

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
        asignarNumReportes();
    }

    private async void OnCrearReporteClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CrearReporte(_client,_user));
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


    private async void asignarNumReportes()
    {
        try
        {
            Debug.WriteLine("aqui estoy 1");

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


                int cantidadReportes = 0;

                //se usar el metodo .Count para contabilizar el numero de objetos en la lista llamada reportes
                cantidadReportes = reportes.Count;


                Debug.WriteLine("test" + reportes[0]);
  

                Debug.WriteLine($"Cantidad de reportes: {cantidadReportes}");

                numReportes.Text = $"Cantidad de reportes: {cantidadReportes}";


            }
            else
            {
                Console.WriteLine($"Error en la solicitud: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Excepci�n durante la solicitud: {ex.Message}");
        }

    }


}