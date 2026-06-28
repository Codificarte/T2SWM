using CommunityToolkit.Mvvm.ComponentModel;
using T2SLogistics.Models;

namespace T2SLogistics.ViewModels;

/// <summary>Linha de conferência da Receção: artigo esperado + quantidade já lida (atualiza ao registar).</summary>
public partial class ReceptionLineViewModel : ObservableObject
{
    public string ProductRef { get; }
    public string Description { get; }
    public string Code { get; }
    public int Expected { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ProgressText))]
    [NotifyPropertyChangedFor(nameof(IsComplete))]
    private int _read;

    public ReceptionLineViewModel(ReceptionExpectedLine line)
    {
        ProductRef = line.ProductRef;
        Description = line.Description;
        Code = line.Code;
        Expected = line.Expected;
    }

    public string ProgressText => $"{Read} / {Expected}";
    public bool IsComplete => Expected > 0 && Read >= Expected;
}
