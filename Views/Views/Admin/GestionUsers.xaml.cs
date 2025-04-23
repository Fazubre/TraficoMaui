using Newtonsoft.Json;
using System.Text;

namespace TraficoCRFront.Views.Admin;

public partial class GestionUsers : ContentPage
{
    private readonly HttpClient client;

    public GestionUsers()
    {
        InitializeComponent();

        client = new HttpClient
        {
            BaseAddress = new Uri("http://127.0.0.1:8000/") 
        };

        LoadUsuarios();
    }

    private async void LoadUsuarios()
    {
        try
        {
            var response = await client.GetAsync("buscarUsuarios");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);
                UsersCollectionView.ItemsSource = usuarios;
            }
            else
            {
                await DisplayAlert("Error", "No se pudieron cargar los usuarios", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnEliminarUsuarioClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is int userId)
        {
            bool confirm = await DisplayAlert("Confirmación", "¿Eliminar este usuario?", "Sí", "No");
            if (!confirm) return;

            var jsonContent = new StringContent(JsonConvert.SerializeObject(new { IdUsuario = userId }), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("eliminarUsuario", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Usuario eliminado", "OK");
                LoadUsuarios();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo eliminar el usuario", "OK");
            }
        }
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } // Corresponde a username
    }
}
