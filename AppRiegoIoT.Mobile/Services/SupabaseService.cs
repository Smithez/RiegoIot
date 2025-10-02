using Supabase;
using AppRiegoIoT.Mobile.Models;

namespace AppRiegoIoT.Mobile.Services;

public class SupabaseService
{
    private readonly Supabase.Client _supabase;
    
    public SupabaseService()
    {
        var options = new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        };
        
        // ⚠️ CAMBIAR POR TUS DATOS REALES
        _supabase = new Supabase.Client(
            "https://mazzjsllcahxiwqtjmsc.supabase.co", // Tu URL
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im1henpqc2xsY2FoeGl3cXRqbXNjIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTg3NjIxMjIsImV4cCI6MjA3NDMzODEyMn0.XTpxv37W68hG8yH9c1RMBUWld_anU_F65d7v3A4Vyv4", // Tu clave anon real
            options
        );
    }
    
    public Supabase.Client Client => _supabase;
    
    public async Task InitializeAsync()
    {
        await _supabase.InitializeAsync();
    }
    
    // Métodos específicos para el negocio
    public async Task<List<Finca>> GetFincasAsync()
    {
        try
        {
            var response = await _supabase
                .From<Finca>()
                .Select("*")
                .Get();
                
            return response.Models;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error obteniendo fincas: {ex.Message}");
            return new List<Finca>();
        }
    }
    
    public async Task<List<Dispositivo>> GetDispositivosAsync()
    {
        try
        {
            var response = await _supabase
                .From<Dispositivo>()
                .Select("*")
                .Get();
                
            return response.Models;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error obteniendo dispositivos: {ex.Message}");
            return new List<Dispositivo>();
        }
    }
    
    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            var response = await _supabase
                .From<Finca>()
                .Select("id")
                .Limit(1)
                .Get();
                
            System.Diagnostics.Debug.WriteLine($"✅ Conexión exitosa! Fincas: {response.Models.Count}");
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"❌ Error de conexión: {ex.Message}");
            return false;
        }
    }
}