using T2SLogistics.ViewModels;

namespace T2SLogistics.Views;

public partial class ManagePinPage : ContentPage
{
    private readonly ManagePinViewModel _viewModel;

    public ManagePinPage(ManagePinViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }
}
