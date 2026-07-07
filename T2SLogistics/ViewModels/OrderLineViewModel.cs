using CommunityToolkit.Mvvm.ComponentModel;
using T2SLogistics.Models;

namespace T2SLogistics.ViewModels;

/// <summary>
/// Linha (artigo) no detalhe da encomenda. Na Expedição guarda o que TEM de sair (ref/lote/validade/
/// alvéolo) para o modal de leitura validar contra a linha; <see cref="Separated"/> atualiza o progresso
/// ao registar uma leitura.
/// </summary>
public partial class OrderLineViewModel : ObservableObject
{
    public string Description { get; }
    public string Code { get; }            // EAN ou ref (o que se lê)
    public string ProductRef { get; }
    public string? Lote { get; }
    public DateTime? Expiry { get; }
    public string? BinLocation { get; }    // alvéolo esperado
    public int Expected { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ProgressText))]
    [NotifyPropertyChangedFor(nameof(IsComplete))]
    private int _separated;

    public OrderLineViewModel(OrderLine line)
    {
        Description = line.Description;
        Code = line.Code;
        ProductRef = line.ProductRef;
        Lote = line.Lote;
        Expiry = line.Expiry;
        BinLocation = line.BinLocation;
        Expected = line.Total;
        Separated = line.Picked;
    }

    public string ProgressText => $"{Separated} / {Expected}";
    public bool IsComplete => Expected > 0 && Separated >= Expected;
}
