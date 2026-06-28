using T2SLogistics.Models;

namespace T2SLogistics.ViewModels;

/// <summary>Linha (artigo) no detalhe da encomenda.</summary>
public sealed class OrderLineViewModel
{
    public string Description { get; }
    public string Code { get; }
    public string ProgressText { get; }

    public OrderLineViewModel(OrderLine line)
    {
        Description = line.Description;
        Code = line.Code;
        ProgressText = $"{line.Picked} / {line.Total}";
    }
}
