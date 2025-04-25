using System.Net;
using System.Text;
using System.Text.Json;

namespace TraficoCRFront.Views.Gestion;

public partial class GestionUsers : ContentPage
{
    private readonly HttpClient _client;
    private readonly datosUsuario _user;

    public GestionUsers(HttpClient httpClient, datosUsuario user)
    {
        InitializeComponent();
        _client = httpClient;
        _user = user;

        CargarUsuarios(); 
    }

    private async void CargarUsuarios()
    {
        try
        {
            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;
            mensajeVacio.IsVisible = false;

            var jsonContent = JsonSerializer.Serialize(new { nombreUsuario = "" });
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.GetAsync("buscarUsuarios?usuario=");


            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await DisplayAlert("Sesión expirada", "Por favor inicia sesión nuevamente", "OK");
                return;
            }

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var usuarios = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json);

            if (usuarios != null && usuarios.Count > 0)
            {
                usuariosListView.ItemsSource = usuarios.Select(u => new
                {
                    Username = u.ContainsKey("username") ? u["username"]?.ToString() : "",
                    NivelAcceso = u.ContainsKey("NivelAcceso") ? u["NivelAcceso"]?.ToString() : ""
                }).ToList();
            }
            else
            {
                mensajeVacio.IsVisible = true;
                usuariosListView.ItemsSource = null;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Hubo un problema al cargar los usuarios: {ex.Message}", "OK");
        }
        finally
        {
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(); 
    }
}
