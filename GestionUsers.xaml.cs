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

    private async void CargarUsuarios(string filtro = "")
    {
        try
        {
            var response = await _client.GetAsync($"buscarUsuarios?usuario={Uri.EscapeDataString(filtro)}");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await DisplayAlert("Sesión expirada", "Por favor inicia sesión nuevamente", "OK");
                return;
            }

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var usuarios = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json);

            usuariosListView.ItemsSource = usuarios?.Select(u => new
            {
                Username = u.GetValueOrDefault("username")?.ToString(),
                NivelAcceso = u.GetValueOrDefault("NivelAcceso")?.ToString(),
                UsuarioId = u.GetValueOrDefault("id")?.ToString()
            }).ToList();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar los usuarios: {ex.Message}", "OK");
        }
    }

    private void OnBuscarClicked(object sender, EventArgs e)
    {
        CargarUsuarios(busquedaEntry.Text?.Trim() ?? "");
    }

    private async void OnAccionesClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is string username)
        {
            var opcion = await DisplayActionSheet($"Acciones para {username}", "Cancelar", null,
                "Cambiar nivel de acceso", "Asociar región", "Eliminar usuario", "Eliminar asociaciones");

            switch (opcion)
            {
                case "Cambiar nivel de acceso":
                    await CambiarNivelAcceso(username);
                    break;
                case "Asociar región":
                    await AsociarRegion(username);
                    break;
                case "Eliminar usuario":
                    await EliminarUsuario(username);
                    break;
                case "Eliminar asociaciones":
                    await EliminarAsociacion(username);
                    break;
            }
        }
    }



    private async Task EliminarUsuario(string username)
    {
        var confirmacion = await DisplayAlert("Confirmar", $"¿Eliminar a {username}?", "Sí", "Cancelar");
        if (!confirmacion) return;

        try
        {
            var body = new { username };
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("eliminarUsuario", content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Usuario eliminado correctamente.", "OK");
                CargarUsuarios();
            }
            else
            {
                await DisplayAlert("Error", $"No se pudo eliminar el usuario: {response.StatusCode}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al eliminar usuario: {ex.Message}", "OK");
        }
    }

    private async Task CambiarNivelAcceso(string username)
    {
        string nuevoNivel = await DisplayPromptAsync(
            "Cambiar nivel de acceso",
            $"Ingrese nuevo nivel para {username} (1 a 4):",
            maxLength: 1,
            keyboard: Keyboard.Numeric);

        if (string.IsNullOrWhiteSpace(nuevoNivel)) return;

        try
        {
            var body = new CambioNivelRequest
            {
                Username = username,
                NivelAcceso = int.Parse(nuevoNivel)
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("cambiarNivelAcceso", content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Nivel de acceso actualizado.", "OK");
                CargarUsuarios();
            }
            else
            {
                await DisplayAlert("Error", $"No se pudo cambiar el nivel: {response.StatusCode}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cambiar nivel: {ex.Message}", "OK");
        }
    }


    private async Task AsociarRegion(string username)
    {
        string region = await DisplayPromptAsync(
            "Asociar región",
            $"Ingresa la región a asociar a {username}:");

        if (string.IsNullOrWhiteSpace(region)) return;

        try
        {
            var body = new { username, region };
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("asociarRegion", content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Región asociada correctamente.", "OK");
            }
            else
            {
                await DisplayAlert("Error", $"No se pudo asociar región: {response.StatusCode}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al asociar región: {ex.Message}", "OK");
        }
    }

    private async Task EliminarAsociacion(string username)
    {
        bool eliminarTodas = await DisplayAlert(
            "Eliminar asociación",
            $"¿Eliminar todas las asociaciones de {username}?",
            "Sí, todas", "Solo una");

        try
        {
            var endpoint = eliminarTodas ? "eliminarAsociacionTodas" : "eliminarAsociacion";
            var body = new { username };
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(endpoint, content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Asociación eliminada.", "OK");
            }
            else
            {
                await DisplayAlert("Error", $"No se pudo eliminar: {response.StatusCode}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al eliminar asociación: {ex.Message}", "OK");
        }
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
   
    public class CambioNivelRequest
    {
        public string Username { get; set; }
        public int NivelAcceso { get; set; }
    }


}
