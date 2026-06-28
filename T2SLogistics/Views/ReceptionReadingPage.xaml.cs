using T2SLogistics.ViewModels;

namespace T2SLogistics.Views;

public partial class ReceptionReadingPage : ContentPage
{
    private readonly ReceptionReadingViewModel _viewModel;

    public ReceptionReadingPage(ReceptionReadingViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Activate();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.Deactivate();
    }
}
