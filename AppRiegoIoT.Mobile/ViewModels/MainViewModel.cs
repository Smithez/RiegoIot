using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppRiegoIoT.Mobile.Services;
using AppRiegoIoT.Mobile.Models;
using System.Collections.ObjectModel;

namespace AppRiegoIoT.Mobile.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly SupabaseService _supabaseService;
    
    [ObservableProperty]
    private ObservableCollection<Finca> fincas = new();
    
    [ObservableProperty]
    private ObservableCollection<Dispositivo> dispositivos = new();
    
    [ObservableProperty]
    private bool isLoading;
    
    [ObservableProperty]
    private string statusMessage = "Listo para cargar datos";
    
    [ObservableProperty]
    private bool isConnected;
    
    public MainViewModel(SupabaseService supabaseService)
    {
        _supabaseService = supabaseService;
    }
    
    [RelayCommand]
    public async Task LoadDataAsync()
    {
        try
        {
            IsLoading = true;
            StatusMessage = "Conectando a Supabase...";
            
            // Probar conexión primero
            var connectionOk = await _supabaseService.TestConnectionAsync();
            IsConnected = connectionOk;
            
            if (!connectionOk)
            {
                StatusMessage = "❌ Error de conexión con Supabase";
                return;
            }
            
            StatusMessage = "Cargando fincas...";
            var fincasData = await _supabaseService.GetFincasAsync();
            
            StatusMessage = "Cargando dispositivos...";
            var dispositivosData = await _supabaseService.GetDispositivosAsync();
            
            Fincas.Clear();
            foreach (var finca in fincasData)
            {
                Fincas.Add(finca);
            }
            
            Dispositivos.Clear();
            foreach (var dispositivo in dispositivosData)
            {
                Dispositivos.Add(dispositivo);
            }
            
            StatusMessage = $"✅ Datos cargados: {Fincas.Count} fincas, {Dispositivos.Count} dispositivos";
        }
        catch (Exception ex)
        {
            StatusMessage = $"❌ Error: {ex.Message}";
            IsConnected = false;
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    [RelayCommand]
    public async Task RefreshAsync()
    {
        await LoadDataAsync();
    }
}