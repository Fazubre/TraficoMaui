using Newtonsoft.Json;

namespace TraficoCRFront.Views.Admin;

public partial class Reportes : ContentPage
{
    HttpClient client = new HttpClient();
    List<Reporte> allReportes = new();

    public Reportes()
    {
        InitializeComponent();
        LoadReportes();
    }

    private async void LoadReportes()
    {
        try
        {
            var response = await client.GetAsync("http://localhost:8000/getReportesFinalizados");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                allReportes = JsonConvert.DeserializeObject<List<Reporte>>(json);
                ReportesCollectionView.ItemsSource = allReportes;
            }
            else
            {
                await DisplayAlert("Error", "No se pudieron cargar los reportes finalizados", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var keyword = e.NewTextValue.ToLower();
        ReportesCollectionView.ItemsSource = allReportes
            .Where(r => r.Titulo.ToLower().Contains(keyword) || r.Descripcion.ToLower().Contains(keyword))
            .ToList();
    }

    public class Reporte
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
    }
}