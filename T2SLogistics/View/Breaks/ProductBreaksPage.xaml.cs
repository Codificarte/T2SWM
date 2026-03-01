using T2SLogistics.ViewModel.Breaks;

namespace T2SLogistics.View.Breaks;

public partial class ProductBreaksPage : ContentPage
{
    ProductBreaksPageViewModel _productBreaksPageViewModel;
    public ProductBreaksPage(ProductBreaksPageViewModel productBreaksPageViewModel)
	{
		InitializeComponent();
		BindingContext = _productBreaksPageViewModel= productBreaksPageViewModel;
    }
    private async void OnInputBarcode_Completed(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(BarcodeEntry.Text))
        {
            BarcodeEntry.Focus();
            return;
        }
        _productBreaksPageViewModel.RefEanCode = BarcodeEntry.Text.Trim();
        await _productBreaksPageViewModel.GetRefProduct();
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
}