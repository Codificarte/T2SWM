using CommunityToolkit.Mvvm.Input;
using T2SLogistics.Models;

namespace T2SLogistics.ViewModels;

/// <summary>Item de apresentação para um cartão da lista de encomendas.</summary>
public sealed class OrderListItemViewModel
{
    public string Number { get; }
    public string ClientName { get; }
    public string DateText { get; }
    public string Status { get; }
    public Color StatusBg { get; }
    public Color StatusText { get; }
    public string ActionText { get; }
    public IAsyncRelayCommand OpenCommand { get; }

    /// <summary>Mostra o botão de imprimir no cartão (há PDF associado e foi passada uma ação de impressão).</summary>
    public bool CanPrint { get; }
    public IAsyncRelayCommand PrintCommand { get; }

    public OrderListItemViewModel(OrderSummary order, OrderParty party, string actionText, Func<Task> open, Func<Task>? print = null)
    {
        Number = OrderLabels.Title(party, order.Number); // "Enc. Cliente/Fornecedor Nº {obrano}"
        ClientName = order.ClientName;
        DateText = $"{order.Date:dd/MM/yyyy} · {order.LineCount} {(order.LineCount == 1 ? "linha" : "linhas")}";
        Status = StatusVisuals.Label(order.Status);
        StatusBg = StatusVisuals.Background(order.Status);
        StatusText = StatusVisuals.Text(order.Status);
        ActionText = actionText;
        OpenCommand = new AsyncRelayCommand(open);
        CanPrint = order.CanPrint && print is not null;
        PrintCommand = new AsyncRelayCommand(print ?? (() => Task.CompletedTask));
    }
}
