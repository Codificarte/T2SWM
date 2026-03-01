using T2SLogistics.ViewModel.Register;

namespace T2SLogistics.View.Register;

public partial class RegisterEnteryExitPage : ContentPage
{
    RegisterEnteryExitPageViewModel _registerEnteryExitPageViewModel;
    public RegisterEnteryExitPage(RegisterEnteryExitPageViewModel registerEnteryExitPageViewModel)
	{
		InitializeComponent();
		BindingContext = _registerEnteryExitPageViewModel= registerEnteryExitPageViewModel;
       
    }
    protected  override void OnAppearing()
    {


        MainThread.BeginInvokeOnMainThread(async() =>{
            await Task.Delay(100); // Adjust delay as needed
            UserIdEntry.Focus();
        });
           
       

        base.OnAppearing();
    }

    private void UserIdEntry_Completed(object sender, EventArgs e)
    {
        _registerEnteryExitPageViewModel.SaveCommand.Execute(null);
    }
}