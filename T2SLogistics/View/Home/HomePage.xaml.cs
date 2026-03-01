using T2SLogistics.ViewModel.Home;

namespace T2SLogistics.View.Home;

public partial class HomePage : ContentPage
{
    IDispatcherTimer _clockTimer;
	public HomePage(HomePageViewModel homePageViewModel)
	{
		InitializeComponent();
        BindingContext = homePageViewModel;
        homePageViewModel.Initialise();

    }
    protected override void OnAppearing()
    {
        
    }
    override protected void OnDisappearing()
    {
        if (_clockTimer is not null)
        {
            _clockTimer.Stop();
        }
    }
}