using T2SLogistics.ViewModels;

namespace T2SLogistics.Views;

public partial class MovementListPage : ContentPage
{
    public MovementListPage(MovementListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
