using AppRiegoIoT.Mobile.Services;

namespace AppRiegoIoT.Mobile;

public partial class App : Application
{
    public App(SupabaseService supabaseService, AppShell appShell)
    {
        InitializeComponent();
        
        // Inicializar Supabase
        Task.Run(async () => await supabaseService.InitializeAsync());
        
        MainPage = appShell;
    }
}