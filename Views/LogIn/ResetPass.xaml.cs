
namespace TraficoCRFront.Views.LogIn;

public partial class ResetPass : ContentPage
{
    private readonly HttpClient _client;
    public ResetPass()
	{
		InitializeComponent();
	}
    public ResetPass(HttpClient client)
    {
        InitializeComponent();
        _client = client;
    }
    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        
        string pass = Password.Text;
        string confirmPass = ConfirmPassword.Text;

        if (string.IsNullOrWhiteSpace(pass) || string.IsNullOrWhiteSpace(confirmPass))
        {
            await DisplayAlert("Error", "Por favor completa todos los campos.", "OK");
            return;
        }

        if (pass != confirmPass)
        {
            await DisplayAlert("Error", "Las contraseñas no coinciden.", "OK");
            return;
        }

        await DisplayAlert("Éxito", "Contraseña cambiada correctamente.", "OK");
    }

}