using T2SLogistics.ViewModel.InsertProduction;
using T2SLogistics.ViewModel.Orders;

namespace T2SLogistics.View.Orders;

public partial class ViewProductOperationPage : ContentPage
{
	public ViewProductOperationPage(ViewProductOperationPageViewModel viewProductOperationPageViewModel)
	{
		InitializeComponent();
		BindingContext= viewProductOperationPageViewModel;
    }
}