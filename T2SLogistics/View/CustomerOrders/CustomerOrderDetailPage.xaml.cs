using T2SLogistics.ViewModel.CustomerOrders;

namespace T2SLogistics.View.CustomerOrders;

public partial class CustomerOrderDetailPage : ContentPage
{
	public CustomerOrderDetailPage(CustomerOrderDetailViewModel customerOrderDetailViewModel)
	{
		InitializeComponent();
		BindingContext = customerOrderDetailViewModel;
	}
}