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

        public Register(HttpClient client)
        {
            InitializeComponent();
            _client = client;
        }
        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            string nombre = NombreApellido.Text;
            string correo = Email.Text;
            string pass = Password.Text;
            string confirmPass = ConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(pass) || string.IsNullOrWhiteSpace(confirmPass))
            {
                await DisplayAlert("Error", "Por favor completa todos los campos.", "OK");
                return;
            }

            if (pass != confirmPass)
            {
                await DisplayAlert("Error", "Las contraseñas no coinciden.", "OK");
                return;
            }

            await SignupRequest(nombre, pass , _client);
            await DisplayAlert("Éxito", "Registro completado correctamente.", "OK");
        }
        
        static async Task SignupRequest(string uname, string pword, HttpClient client)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(new
                {
                    username = uname,
                    password = pword
                }), Encoding.UTF8,
                "application/json");
            using HttpResponseMessage response = await client.PostAsync(
                "signup",
                jsonContent
            );
            IEnumerable<string> values;
            string token = string.Empty;
            if (response.Headers.TryGetValues("Set-Cookie", out values))
            {
                token = values.FirstOrDefault();
                Console.WriteLine(token);
            }
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.ToString());
            } 
            client.DefaultRequestHeaders.Add("Cookie",token);
        }

    }
}