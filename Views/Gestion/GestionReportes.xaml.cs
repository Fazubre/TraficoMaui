namespace TraficoCRFront.Views.Gestion;

public partial class GestionReportes : ContentPage
{
    private readonly HttpClient _client;
    private readonly datosUsuario _user;

    public GestionReportes(HttpClient httpClient, datosUsuario user)
    {
        InitializeComponent();
        _client = httpClient;
        _user = user;

    }
}