using T2SLogistics.ViewModel.OrderSepration;

namespace T2SLogistics.View.OrderSepration;

public partial class AskUserIdPage : ContentPage
{
	public AskUserIdPage(AskUserIdPageViewModel askUserIdPageViewModel)
	{
		InitializeComponent();
        BindingContext = askUserIdPageViewModel;

    }
}