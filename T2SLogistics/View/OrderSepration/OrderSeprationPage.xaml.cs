using T2SLogistics.ViewModel.OrderSepration;

namespace T2SLogistics.View.OrderSepration;

public partial class OrderSeprationPage : ContentPage
{
	public OrderSeprationPage(OrderSeprationPageViewModel orderSeprationPageViewModel)
	{
		InitializeComponent();
		BindingContext = orderSeprationPageViewModel;
    }
}