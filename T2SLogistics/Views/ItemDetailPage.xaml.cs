using T2SLogistics.ViewModels;

namespace T2SLogistics.Views;

public partial class ItemDetailPage : ContentPage
{
    public ItemDetailPage()
    {
        InitializeComponent();
        BindingContext = new ItemDetailViewModel();
    }
}