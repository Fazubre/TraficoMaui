namespace TraficoCRFront.Views.LogIn;
using TraficoCRFront.Views.Register;
using TraficoCRFront.Views.Admin;
using TraficoCRFront.Views;

public partial class LogIn : ContentPage
{
    private readonly HttpClient _client;
    private HttpClient client;

    public LogIn(HttpClient client)
    {
        InitializeComponent();
        _client = client;
    }


    private async void OnLoginClicked(object sender, EventArgs e)
    {
        try
        {
            var httpClient = DependencyService.Get<HttpClient>();

            if (httpClient != null)
            {
                var loginPage = new LogIn(httpClient);

                await Navigation.PushAsync(loginPage);
            }

            await Navigation.PushAsync(new MainPage());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Navigation error: {ex.Message}");
            await DisplayAlert("Error", "No podemos acceder a la página principal", "OK");
        }
    }

    public async void OnRegisterClicked(object sender, EventArgs e)
    {
        try
        {
            var httpClient = DependencyService.Get<HttpClient>();

            if (httpClient != null)
            {
                var registerPage = new Register(httpClient);

                await Navigation.PushAsync(registerPage);
            }

            await Navigation.PushAsync(new Register(client));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Navigation error: {ex.Message}");
            await DisplayAlert("Error", "No podemos acceder a la página de Registrar", "OK");
        }



    }
    public async void OnForgotPasswordClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ResetPass());
    }

}
