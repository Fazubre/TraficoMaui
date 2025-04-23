using Newtonsoft.Json;
using System.Text;

namespace TraficoCRFront.Views.Admin;

public partial class GestionReportes : ContentPage
{
    HttpClient client = new HttpClient();
    List<Reporte> allReportes = new();

    public GestionReportes()
    {
        InitializeComponent();
        LoadReportes();
    }

    private async void LoadReportes()
    {
        try
        {
            var response = await client.GetAsync("http://10.0.2.2:8000/getReportesDistritosPropios");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                allReportes = JsonConvert.DeserializeObject<List<Reporte>>(json);
                ReportesCollectionView.ItemsSource = allReportes;
            }
            else
            {
                await DisplayAlert("Error", "No se pudieron cargar los reportes", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al conectar: {ex.Message}", "OK");
        }
    }

    private async void OnDesactivarClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is int reporteId)
        {
            var confirm = await DisplayAlert("Confirmar", "¿Deseas desactivar este reporte?", "Sí", "No");
            if (!confirm) return;

            var content = new StringContent(JsonConvert.SerializeObject(new { IdReporte = reporteId }), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://10.0.2.2:8000/desactivarReporte", content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Reporte desactivado", "OK");
                LoadReportes();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo desactivar el reporte", "OK");
            }
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var keyword = e.NewTextValue.ToLower();
        var filtrados = allReportes
            .Where(r => r.Titulo.ToLower().Contains(keyword) || r.Descripcion.ToLower().Contains(keyword))
            .ToList();

        ReportesCollectionView.ItemsSource = filtrados;
    }

    public class Reporte
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
    }
}
