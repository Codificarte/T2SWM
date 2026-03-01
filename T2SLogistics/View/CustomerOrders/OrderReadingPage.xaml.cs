using T2SLogistics.ViewModel.CustomerOrders;

namespace T2SLogistics.View.CustomerOrders;

public partial class OrderReadingPage : ContentPage
{
    OrderReadingPageViewModel _orderReadingPageViewModel;
    public OrderReadingPage(OrderReadingPageViewModel orderReadingPageViewModel)
    {
        InitializeComponent();
        BindingContext = _orderReadingPageViewModel = orderReadingPageViewModel;
    }

    private async void OnInputBarcode_Completed(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(BarcodeEntry.Text))
        {
            BarcodeEntry.Focus();
            return;
        }
        _orderReadingPageViewModel.RefEanCode = BarcodeEntry.Text.Trim();
        await _orderReadingPageViewModel.GetRefProduct();
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

    private void Switch_Toggled(object sender, ToggledEventArgs e)
    {

    }
}