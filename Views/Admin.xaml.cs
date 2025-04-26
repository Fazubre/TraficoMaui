using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TraficoCRFront.Views.LogIn;
using TraficoCRFront.Views.Gestion;

namespace TraficoCRFront.Views
{
    public partial class Admin : ContentPage
    {
        private readonly HttpClient _client;
        private readonly datosUsuario _user;

        public Admin(HttpClient client, datosUsuario user)
        {
            InitializeComponent();
            _client = client;
            _user = user;
             CargarEstadisticas(); 
        }

        private async void CargarEstadisticas()
        {
            try
            {
                // users registrados al momento
                var usuariosResponse = await _client.GetAsync("buscarUsuarios?usuario=");
                if (usuariosResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    //await DisplayAlert("No posee cuenta de administrador", "Por favor inicia sesión de nuevo.", "OK");
                    //await Navigation.PopToRootAsync();
                    tUsuarios.IsVisible = false;
                }
                else if (usuariosResponse.IsSuccessStatusCode)
                {
                    var usuarios = await usuariosResponse.Content.ReadFromJsonAsync<List<object>>();
                    //await DisplayAlert("Usuarios", $"Usuarios encontrados: {usuarios?.Count}", "OK");
                    labelUsuarios.Text = usuarios?.Count.ToString() ?? "0";
                }
                else if (usuariosResponse.StatusCode != HttpStatusCode.Unauthorized)
                {
                    await DisplayAlert("Error al cargar usuarios", $"Código: {usuariosResponse.StatusCode}", "OK");
                }

                // Reportes en distritos propios activos al momento
                var activosResponse = await _client.GetAsync("getReportesDistritosPropios");
                if (activosResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await DisplayAlert("No posee cuenta de administrador", "Por favor inicia sesión de nuevo.", "OK");
                    await Navigation.PopToRootAsync();
                    return;
                }
                else if (activosResponse.IsSuccessStatusCode)
                {
                    //var reportesActivos = await activosResponse.Content.ReadFromJsonAsync<List<object>>();
                    var contenido = await activosResponse.Content.ReadAsStringAsync();
                    var reportesActivos = JsonSerializer.Deserialize<List<Reporte>>(contenido);
                    reportesActivos.RemoveAll(item => item.activo == false);
                    await DisplayAlert("Reportes Activos", $"Activos: {reportesActivos?.Count}", "OK");
                    labelActivos.Text = reportesActivos?.Count.ToString() ?? "0";
                }
                else
                {
                    await DisplayAlert("Error al cargar reportes activos", $"Código: {activosResponse.StatusCode}", "OK");
                }

                // Reportes completos
                var completosResponse = await _client.GetAsync("getReportesDistritosPropios");
                if (completosResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await DisplayAlert("No posee cuenta de administrador", "Por favor inicia sesión de nuevo.", "OK");
                    await Navigation.PopToRootAsync();
                    return;
                }
                else if (completosResponse.IsSuccessStatusCode)
                {
                    //var reportesActivos = await activosResponse.Content.ReadFromJsonAsync<List<object>>();
                    var contenido = await completosResponse.Content.ReadAsStringAsync();
                    var reportesCompletos = JsonSerializer.Deserialize<List<Reporte>>(contenido);
                    reportesCompletos.RemoveAll(item => item.activo == true);
                    await DisplayAlert("Reportes Activos", $"Activos: {reportesCompletos?.Count}", "OK");
                    labelActivos.Text = reportesCompletos?.Count.ToString() ?? "0";
                }
                else
                {
                    await DisplayAlert("Error al cargar reportes activos", $"Código: {activosResponse.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error general", $"Detalles: {ex.Message}", "OK");
            }
        }


        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Confirmar", "¿Está seguro que desea cerrar la sesión?", "Sí", "No");

            if (confirm)
            {
                try
                {
                    await Navigation.PopToRootAsync();

                    var loginPage = new LogIn.LogIn(_client, _user);
                    loginPage.LimpiarCampos(); // <- Llama al método para limpiar

                    await Navigation.PushAsync(loginPage);
                    await DisplayAlert("Éxito", "La sesión se ha cerrado correctamente", "Aceptar");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Ocurrió un error al cerrar la sesión: {ex.Message}", "Aceptar");
                }
            }
        }


        private async void OnGestionUsersClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GestionUsers(_client, _user));
        }

        private async void OnGestionReportesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GestionReportes(_client, _user));
        }

        private async void OnReportesCompletosClicked(object sender, EventArgs e)
        {
            
            await Navigation.PushAsync(new ReportesCompletos(_client, _user));
        }
    }
}
