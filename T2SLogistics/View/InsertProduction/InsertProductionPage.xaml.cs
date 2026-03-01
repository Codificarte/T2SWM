using T2SLogistics.ViewModel.InsertProduction;

namespace T2SLogistics.View.InsertProduction;

public partial class InsertProductionPage : ContentPage
{
	public InsertProductionPage(InsertProductionPageViewModel insertProductionPageViewModel)
	{
		InitializeComponent();
        BindingContext = insertProductionPageViewModel;
    }

   
}