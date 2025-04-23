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
        bool confirm = await DisplayAlert("Confirmar", "�Est� seguro que desea cerrar la sesi�n?", "S�", "No");

        if (confirm)
        {
            try
            {
               
                await Navigation.PopToRootAsync();
                await Navigation.PushAsync(new MainPage());

                await DisplayAlert("�xito", "La sesi�n se ha cerrado correctamente", "Aceptar");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurri� un error al cerrar la sesi�n: {ex.Message}", "Aceptar");
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