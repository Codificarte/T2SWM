using T2SLogistics.ViewModels;

namespace T2SLogistics.Views;

public partial class MainMenuPage : ContentPage
{
    public MainMenuPage(MainMenuViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
