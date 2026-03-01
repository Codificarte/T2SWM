using T2SLogistics.Model;
using T2SLogistics.ViewModel.CustomerOrders;

namespace T2SLogistics.View.CustomerOrders;

public partial class CustomerOrdersPage : ContentPage
{
  CustomerOrdersPageViewModel _customerOrdersPageViewModel;
	public CustomerOrdersPage(CustomerOrdersPageViewModel customerOrdersPageViewModel)
	{
		InitializeComponent();
		BindingContext =_customerOrdersPageViewModel= customerOrdersPageViewModel;
    }
    private async void OnInputBarcode_Completed(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(BarcodeEntry.Text))
        {
            BarcodeEntry.Focus();
            return;
        }
        _customerOrdersPageViewModel.RefEanCode = BarcodeEntry.Text.Trim();
        _customerOrdersPageViewModel.CheckIfItemExists();
        
        // Clear the entry after processing
        BarcodeEntry.Text = string.Empty;
        BarcodeEntry.Focus();
    }
    protected override void OnAppearing()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Task.Delay(100); // Adjust delay as needed

            BarcodeEntry.Focus();
        });
        base.OnAppearing();


    }
    //private void Expander_ExpandedChanged(object sender, CommunityToolkit.Maui.Core.ExpandedChangedEventArgs e)
    //{
    //    if (sender is CommunityToolkit.Maui.Views.Expander expander && expander.BindingContext is CustomersOrderModel order)
    //    {
    //        order.IsExpanded= e.IsExpanded;
    //    }
    //}
}