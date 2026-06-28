using CommunityToolkit.Mvvm.ComponentModel;

namespace T2SLogistics.ViewModels;

/// <summary>Base dos ViewModels da nova UI (Shell + CommunityToolkit.Mvvm).</summary>
public partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _title = string.Empty;
}
