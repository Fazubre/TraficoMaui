namespace TraficoCRFront.Views.Admin;
using TraficoCRFront.Views;

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
    private async void OnGestionUsersClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
        await Navigation.PushAsync(new GestionUsers());
    }
    private async void OnGestionReportesClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
        await Navigation.PushAsync(new GestionReportes());
    }
    private async void OnReportesClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
        await Navigation.PushAsync(new Reportes());
    }
}