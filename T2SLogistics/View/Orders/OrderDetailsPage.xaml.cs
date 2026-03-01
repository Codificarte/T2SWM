using T2SLogistics.Model;
using T2SLogistics.ViewModel.Orders;

namespace T2SLogistics.View.Orders;

public partial class OrderDetailsPage : ContentPage
{
     
    public OrderDetailsPage(OrderDetailsPageViewModel orderDetailsPageViewModel)
	{
		InitializeComponent();
		BindingContext = orderDetailsPageViewModel;
    }

  
}