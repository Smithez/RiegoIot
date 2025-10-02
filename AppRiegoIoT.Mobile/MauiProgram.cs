using AppRiegoIoT.Mobile.Services;
using AppRiegoIoT.Mobile.ViewModels;
using Microsoft.Extensions.Logging;

namespace AppRiegoIoT.Mobile;

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
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Registrar servicios
        builder.Services.AddSingleton<SupabaseService>();
        
        // Registrar ViewModels
        builder.Services.AddTransient<MainViewModel>();
        
        // Registrar Pages
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddSingleton<AppShell>();

        return builder.Build();
    }
}