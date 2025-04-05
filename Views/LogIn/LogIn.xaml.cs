namespace TraficoCRFront.Views.LogIn;
using TraficoCRFront.Views.Register;
using TraficoCRFront.Views;

public partial class LogIn : ContentPage
{
    private readonly HttpClient _client;
    public LogIn(HttpClient client)
    {
        InitializeComponent();
        _client = client;
    }


    private async void OnLoginClicked(object sender, EventArgs e)
    {
        try
        {
            
            await Navigation.PushAsync(new MainPage());


        }
        catch (Exception ex)
        {
            Console.WriteLine($"Navigation error: {ex.Message}");

            await DisplayAlert("Error", "No podemos acceder a la pagina principal", "OK");
        }
    }

    public async void OnRegisterClicked(object sender, EventArgs e)
    {
        try
        {
            var httpClient = DependencyService.Get<HttpClient>();

            if (httpClient != null)
            {
                // Crear una nueva instancia de Register con el HttpClient
                var registerPage = new Register(httpClient);

                // Navegar a la página Register
                await Navigation.PushAsync(registerPage);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Navigation error: {ex.Message}");
            await DisplayAlert("Error", "No podemos acceder a la página de Registrar", "OK");
        }






    }

}
