using AppRiegoIoT.Mobile.ViewModels;

namespace AppRiegoIoT.Mobile;

public partial class AppShell : Shell
{
    private readonly MainViewModel _viewModel;
    
    public AppShell(MainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Cargar datos automáticamente al aparecer la página
        await _viewModel.LoadDataCommand.ExecuteAsync(null);
    }
}