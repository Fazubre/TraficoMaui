using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using TraficoCRFront.Views.LogIn;

namespace TraficoCRFront.Views.Register
{

    public partial class Register : ContentPage
    {
        private readonly HttpClient _client;
        private readonly datosUsuario _user;

        public Register(HttpClient client)
        {
            InitializeComponent();
            _client = client;
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            string nombre = NombreApellido.Text?.Trim();
            string pass = Password.Text;
            string confirmPass = ConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(pass) ||
                string.IsNullOrWhiteSpace(confirmPass))
            {
                await DisplayAlert("Error", "Por favor completa todos los campos.", "OK");
                return;
            }

            if (pass != confirmPass)
            {
                await DisplayAlert("Error", "Las contraseñas no coinciden.", "OK");
                return;
            }

            bool success = await SignupRequest(nombre, pass);
            if (success)
            {
                await DisplayAlert("Éxito", "Registro completado correctamente.", "OK");
                await Navigation.PushAsync(new MainPage(_client, _user)); // Ir al login tras éxito
            }
            else
            {
                await DisplayAlert("Error", "No se pudo completar el registro. Intenta más tarde.", "OK");
            }
        }

        private async Task<bool> SignupRequest(string uname, string pword)
        {
            try
            {
                var payload = new
                {
                    username = uname,
                    password = pword
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync("signup", content);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine($"Registro fallido: {response.StatusCode}");
                    return false;
                }

                if (response.Headers.TryGetValues("Set-Cookie", out var values))
                {
                    var token = values.FirstOrDefault();
                    if (!string.IsNullOrEmpty(token))
                    {
                        if (!_client.DefaultRequestHeaders.Contains("Cookie"))
                            _client.DefaultRequestHeaders.Add("Cookie", token);

                        Console.WriteLine("Token guardado");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en SignupRequest: " + ex.Message);
                return false;
            }
        }
    }

}