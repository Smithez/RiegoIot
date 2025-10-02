using DashboardRiego.Web.Components;
using DashboardRiego.Web.Services;
using Supabase;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5198");

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

var supabaseUrl = "https://mazzjsllcahxiwqtjmsc.supabase.co";
Console.WriteLine($"Usando Supabase URL: {supabaseUrl}");
var supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im1henpqc2xsY2FoeGl3cXRqbXNjIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTg3NjIxMjIsImV4cCI6MjA3NDMzODEyMn0.XTpxv37W68hG8yH9c1RMBUWld_anU_F65d7v3A4Vyv4";
Console.WriteLine("Supabase Key configurada");

try
{
    if (builder.Environment.IsDevelopment() && 
        (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseKey) ||
        supabaseUrl == "YOUR_SUPABASE_URL" || supabaseKey == "YOUR_SUPABASE_KEY"))
    {
        builder.Services.AddSingleton<SupabaseService>(_ => new MockSupabaseService());
    }
    else
    {
        var service = new SupabaseService(supabaseUrl, supabaseKey);
        service.InitializeAsync().GetAwaiter().GetResult();
        builder.Services.AddSingleton<SupabaseService>(service);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error initializing Supabase: {ex.Message}");
    builder.Services.AddSingleton<SupabaseService>(_ => new MockSupabaseService());
}

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error no manejado: {ex}");
        throw;
    }
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

var cts = new CancellationTokenSource();

Console.CancelKeyPress += (s, e) =>
{
    Console.WriteLine("Recibida señal de cancelación");
    e.Cancel = true;
    cts.Cancel();
};

try
{
    await app.RunAsync(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Aplicación cerrada correctamente.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error fatal: {ex}");
    throw;
}