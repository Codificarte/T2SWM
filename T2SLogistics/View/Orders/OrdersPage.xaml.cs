using T2SLogistics.ViewModel.Orders;

namespace T2SLogistics.View.Orders;

public partial class OrdersPage : ContentPage
{
    OrdersPageViewModel _ordersPageViewModel;
    public OrdersPage(OrdersPageViewModel ordersPageViewModel)
	{
		InitializeComponent();
        BindingContext = _ordersPageViewModel = ordersPageViewModel;

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
    }
}