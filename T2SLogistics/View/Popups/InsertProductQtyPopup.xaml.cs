using T2SLogistics.Services.Interface;
using T2SLogistics.ViewModel.Popups;
using Mopups.Pages;
using Mopups.Services;

namespace T2SLogistics.View.Popups;

public partial class InsertProductQtyPopup : PopupPage
{
    IServiceProvider _serviceProvider;
    InsertProductQtyPopupViewModel insertProductQtyPopupViewModel;
    string _refProd;
    string _description;
    string _stampLinOrderProd;
    string _operationName;
    public InsertProductQtyPopup(IServiceProvider serviceProvider,
        string refProd, string description, string stampLinOrderProd, string operationName)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        _refProd= refProd;
        _description= description;
        _stampLinOrderProd= stampLinOrderProd;
        _operationName=operationName;
        BindingContext = insertProductQtyPopupViewModel= _serviceProvider.GetService<InsertProductQtyPopupViewModel>();
    }
    protected override void OnAppearing()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            InsertQtyEntry.Focus();
        });
        base.OnAppearing();
    }
    protected override void OnDisappearing()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            InsertQtyEntry.Unfocus();
        });
        base.OnDisappearing();

    }
    private void OnClose(object sender, EventArgs e)
    {
        MopupService.Instance.PopAsync();

    }

    private async void InsertQtyEntry_Completed(object sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            InsertQtyEntry.Focus();
        });
        if (string.IsNullOrWhiteSpace(InsertQtyEntry.Text)|| string.IsNullOrWhiteSpace(QtyBreakEntry.Text))
        {
            
            return;
        }
        await insertProductQtyPopupViewModel.InsertQtyAsync(refProd:_refProd, description: _description,
          stampLinOrderProd: _stampLinOrderProd, operationName:_operationName);
    }
}