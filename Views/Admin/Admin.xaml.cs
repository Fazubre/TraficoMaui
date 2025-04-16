namespace TraficoCRFront.Views.Admin;
using TraficoCRFront.Views;
using TraficoCRFront.Views.LogIn;

public partial class Admin : ContentPage
{
	public Admin()
	{
		InitializeComponent();
	}
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirmar", "¿Está seguro que desea cerrar la sesión?", "Sí", "No");

        if (confirm)
        {
            try
            {
               
                await Navigation.PopToRootAsync();
                await Navigation.PushAsync(new MainPage());

                await DisplayAlert("Éxito", "La sesión se ha cerrado correctamente", "Aceptar");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al cerrar la sesión: {ex.Message}", "Aceptar");
            }
        }
    }
}