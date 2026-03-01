using Mopups.Pages;
using Mopups.Services;

namespace T2SLogistics.View.Popups;

public partial class ObservationDescriptionPopup : PopupPage
{
	public ObservationDescriptionPopup()
	{
		InitializeComponent();
        TaskCompletionSource = new TaskCompletionSource<string>();
    }
    public TaskCompletionSource<string> TaskCompletionSource { get; set; }

    private void OnClose(object sender, EventArgs e)
    {
        TaskCompletionSource.SetResult(string.Empty);
        MopupService.Instance.PopAsync();

    }

    private void SaveNotes_Clicked(object sender, EventArgs e)
    {
        TaskCompletionSource.SetResult(obsNotesTextField.Text);
        MopupService.Instance.PopAsync();

    }
}