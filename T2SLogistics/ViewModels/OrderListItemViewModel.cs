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

    public OrderListItemViewModel(OrderSummary order, string actionText, Func<Task> open)
    {
        Number = order.Number;
        ClientName = order.ClientName;
        DateText = $"{order.Date:dd/MM/yyyy} · {order.LineCount} linhas";
        Status = StatusVisuals.Label(order.Status);
        StatusBg = StatusVisuals.Background(order.Status);
        StatusText = StatusVisuals.Text(order.Status);
        ActionText = actionText;
        OpenCommand = new AsyncRelayCommand(open);
    }
}
