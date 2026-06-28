using T2SLogistics.ViewModels;

namespace T2SLogistics.Views;

public partial class MovementDetailPage : ContentPage
{
    public MovementDetailPage(MovementDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
