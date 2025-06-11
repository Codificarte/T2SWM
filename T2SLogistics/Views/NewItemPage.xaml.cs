using T2SLogistics.Models;
using T2SLogistics.ViewModels;

namespace T2SLogistics.Views;

public partial class NewItemPage : ContentPage
{
    public Item Item { get; set; }

    public NewItemPage()
    {
        InitializeComponent();
        BindingContext = new NewItemViewModel();
    }
}