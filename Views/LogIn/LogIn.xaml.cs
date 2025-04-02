namespace TraficoCRFront.Views.LogIn;
using TraficoCRFront.Views.Register;

public partial class LogIn : ContentPage
{
    public LogIn()
    {
        InitializeComponent();
    }


    private async void OnLoginClicked(object sender, EventArgs e)
    {
        try
        {

            MainPage mainPage = new MainPage();
            await Navigation.PushAsync(mainPage);


        }
        catch (Exception ex)
        {
            Console.WriteLine($"Navigation error: {ex.Message}");

            await DisplayAlert("Error", "No podemos acceder a la pagina principal", "OK");
        }
    }

    public async void OnRegisterClicked(object sender, EventArgs e)
    {
        try
        {

            Register register = new Register();
            await Navigation.PushAsync(register);


        }
        catch (Exception ex)
        {
            Console.WriteLine($"Navigation error: {ex.Message}");

            await DisplayAlert("Error", "No podemos acceder a la pagina principal", "OK");
        }



    }

}
