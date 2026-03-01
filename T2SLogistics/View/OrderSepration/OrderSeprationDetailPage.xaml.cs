using T2SLogistics.ViewModel.OrderSepration;

namespace T2SLogistics.View.OrderSepration;

public partial class OrderSeprationDetailPage : ContentPage
{
	public OrderSeprationDetailPage(OrderSeprationDetailPageViewModel orderSeprationDetailPageViewModel)
	{
		InitializeComponent();
		BindingContext = orderSeprationDetailPageViewModel;
    }
}