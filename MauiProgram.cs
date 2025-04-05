using TraficoCRFront.Views.Register;

namespace TraficoCRFront
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Lato-Bold.ttf", "bold");
                    fonts.AddFont("Font Awesome Solid.otf", "AwesomeSolid");
                    
                })
                .UseMauiMaps();
            
            builder.Services.AddSingleton<HttpClient>(new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8000/") // Ajusta según tu API
            });
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
