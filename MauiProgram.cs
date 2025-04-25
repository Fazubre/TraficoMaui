using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using TraficoCRFront.Views.Register;

namespace TraficoCRFront
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>() // <- esto usa el constructor de App con DI
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Lato-Bold.ttf", "bold");
                    fonts.AddFont("Font Awesome Solid.otf", "AwesomeSolid");
                })
                .UseMauiCommunityToolkit()
                .UseMauiMaps();

            // ✅ Registra HttpClient como singleton
            builder.Services.AddSingleton(new HttpClient
            {
                BaseAddress = new Uri("http://192.168.56.1:8000/")
            });
            builder.Services.AddSingleton(new datosUsuario());

            // ✅ También registra la clase App para que MAUI pueda inyectarla
            builder.Services.AddSingleton<App>();

#if DEBUG
    builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
