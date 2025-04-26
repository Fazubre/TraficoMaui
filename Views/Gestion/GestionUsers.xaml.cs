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
                await DisplayAlert("Sesi�n expirada", "Por favor inicia sesi�n nuevamente", "OK");
                return;
            }

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var usuarios = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json);

            usuariosListView.ItemsSource = usuarios?.Select(u => new
            {
                Username = u.GetValueOrDefault("username")?.ToString(),
                NivelAcceso = u.GetValueOrDefault("NivelAcceso")?.ToString(),
                UsuarioId = u.GetValueOrDefault("UserID")?.ToString()
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
        var butn = sender as Button;
        var aaaaa = butn?.CommandParameter as string;
        var UsuarioId = int.Parse(aaaaa);
        var opcion = await DisplayActionSheet($"Acciones para {UsuarioId}", "Cancelar", null,
            "Cambiar nivel de acceso", "Asociar regi�n", "Eliminar usuario", "Eliminar asociaciones");

        switch (opcion)
        {
            case "Cambiar nivel de acceso":
                await CambiarNivelAcceso(UsuarioId);
                break;
            case "Asociar regi�n":
                await AsociarRegion(UsuarioId);
                break;
            case "Eliminar usuario":
                await EliminarUsuario(UsuarioId);
                break;
            case "Eliminar asociaciones":
                await EliminarAsociacion(UsuarioId);
                break;
        }
    }



    private async Task EliminarUsuario(int username)
    {
        var confirmacion = await DisplayAlert("Confirmar", $"�Eliminar a el usuario", "S�", "Cancelar");
        if (!confirmacion) return;

        try
        {
            var body = new { usuarioId = username };
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("eliminarUsuario", content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("�xito", "Usuario eliminado correctamente.", "OK");
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

    private async Task CambiarNivelAcceso(int username)
    {
        string nuevoNivel = await DisplayPromptAsync(
            "Cambiar nivel de acceso",
            $"Ingrese nuevo nivel para {username} (1 a 4):",
            maxLength: 1,
            keyboard: Keyboard.Numeric);

        if (string.IsNullOrWhiteSpace(nuevoNivel)) return;

        try
        {
            var body = new 
            {
                usuarioId = username,
                NivelAccesoDeseado = int.Parse(nuevoNivel)
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("cambiarNivelAcceso", content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("�xito", "Nivel de acceso actualizado.", "OK");
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

    private async Task AsociarRegion(int username)
    {
        string region = await DisplayPromptAsync(
            "Asociar regi�n",
            $"Ingresa el codigo postal del canton a asociar:");

        if (string.IsNullOrWhiteSpace(region)) return;

        try
        {
            var body = new { usuarioId = username , DistritoIdDeseado = int.Parse(region) };
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("asociarRegion", content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("�xito", "Regi�n asociada correctamente.", "OK");
            }
            else
            {
                await DisplayAlert("Error", $"No se pudo asociar regi�n: {response.StatusCode}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al asociar regi�n: {ex.Message}", "OK");
        }
    }

    private async Task EliminarAsociacion(int username)
    {
        try
        {
            var endpoint = "eliminarAsociacionTodas";
            var body = new { usuarioId = username };
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(endpoint, content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("�xito", "Asociaci�n eliminada.", "OK");
            }
            else
            {
                await DisplayAlert("Error", $"No se pudo eliminar: {response.StatusCode}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al eliminar asociaci�n: {ex.Message}", "OK");
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
