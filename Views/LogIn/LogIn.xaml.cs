using System.Net;
using System.Text;
using System.Text.Json;
using TraficoCRFront.Views.Register;

namespace TraficoCRFront.Views.LogIn
{
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
            string user = usuarioEntry.Text?.Trim();
            string pass = passwordEntry.Text;

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
            {
                await DisplayAlert("Error", "Por favor ingresa tu usuario y contraseña.", "OK");
                return;
            }

            try
            {
                loadingIndicator.IsVisible = true;
                loadingIndicator.IsRunning = true;
                loginButton.IsEnabled = false;

                var loginExitoso = await LoginRequest(user, pass);

                loadingIndicator.IsRunning = false;
                loadingIndicator.IsVisible = false;
                loginButton.IsEnabled = true;

                if (loginExitoso)
                {
                    await DisplayAlert("Éxito", "Sesión iniciada correctamente.", "OK");
                    await Navigation.PushAsync(new MainPage());
                }
                else
                {
                    await DisplayAlert("Error", "Credenciales incorrectas o fallo en el servidor.", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error en login: " + ex.Message);
                await DisplayAlert("Error", "Ocurrió un error al intentar iniciar sesión.", "OK");
                loadingIndicator.IsRunning = false;
                loadingIndicator.IsVisible = false;
                loginButton.IsEnabled = true;
            }
        }

        private async Task<bool> LoginRequest(string uname, string pword)
        {
            var payload = new
            {
                username = uname,
                password = pword
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("login", content);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine($"Login fallido: {response.StatusCode}");
                return false;
            }

            if (response.Headers.TryGetValues("Set-Cookie", out var values))
            {
                var token = values.FirstOrDefault();
                if (!string.IsNullOrEmpty(token) && !_client.DefaultRequestHeaders.Contains("Cookie"))
                {
                    _client.DefaultRequestHeaders.Add("Cookie", token);
                    Console.WriteLine("Token recibido y agregado.");
                }
            }

            return true;
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Register.Register(_client));
        }
    }
}
