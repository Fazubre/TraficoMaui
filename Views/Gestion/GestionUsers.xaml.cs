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

            if (usuarios != null && usuarios.Count > 0)
            {
                usuariosListView.ItemsSource = usuarios.Select(u => new
                {
                    Username = u.ContainsKey("username") ? u["username"]?.ToString() : "",
                    NivelAcceso = u.ContainsKey("NivelAcceso") ? u["NivelAcceso"]?.ToString() : "",
                    UsuarioId = u.ContainsKey("id") ? u["id"]?.ToString() : ""
                }).ToList();
            }
            else
            {
                await DisplayAlert("Sin resultados", "No se encontraron usuarios.", "OK");
                usuariosListView.ItemsSource = null;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Hubo un problema al cargar los usuarios: {ex.Message}", "OK");
        }
    }
    private void OnBuscarClicked(object sender, EventArgs e)
    {
        var filtro = busquedaEntry.Text?.Trim() ?? "";
        CargarUsuarios(filtro);
    }

    //private async void OnEliminarUsuarioClicked(object sender, EventArgs e)
    //{
    //    if (sender is Button btn && btn.CommandParameter is string userId)
    //    {
    //        await EliminarUsuario(userId);
    //    }
    //}

    // Método para eliminar un usuario por ID
    //private async Task EliminarUsuario(string usuarioId)
    //{
    //    try
    //    {
    //        var body = new { usuarioId = usuarioId };
    //        var json = JsonSerializer.Serialize(body);
    //        var content = new StringContent(json, Encoding.UTF8, "application/json");

    //        var response = await _httpClient.PostAsync("/eliminarUsuario", content);

    //        if (response.IsSuccessStatusCode)
    //        {
    //            await DisplayAlert("Éxito", "Usuario eliminado correctamente.", "OK");
    //            CargarUsuarios(); // refresca la lista
    //        }
    //        else
    //        {
    //            await DisplayAlert("Error", $"No se pudo eliminar el usuario: {response.StatusCode}", "OK");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        await DisplayAlert("Error", $"Error al eliminar usuario: {ex.Message}", "OK");
    //    }
    //}

    // Método para eliminar al usuario actual
    private async Task EliminarUsuarioPropio()
    {
        var confirmacion = await DisplayAlert("Confirmar", "¿Deseas eliminar tu propia cuenta?", "Sí", "Cancelar");
        if (!confirmacion) return;

        try
        {
            var response = await _client.PostAsync("eliminarUsuarioPropio", null);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await DisplayAlert("Sesión expirada", "Por favor inicia sesión nuevamente", "OK");
                return;
            }

            response.EnsureSuccessStatusCode();
            await DisplayAlert("Cuenta eliminada", "Tu cuenta ha sido eliminada", "OK");

            
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo eliminar tu cuenta: {ex.Message}", "OK");
        }
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
