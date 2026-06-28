using T2SLogistics.Models;

namespace T2SLogistics.ViewModels;

/// <summary>Tradução de <see cref="OrderStatus"/> para etiqueta e cores do badge.</summary>
internal static class StatusVisuals
{
    public static string Label(OrderStatus status) => status switch
    {
        OrderStatus.Pending => "Pendente",
        OrderStatus.InProgress => "Em curso",
        _ => "Concluída"
    };

    public static Color Background(OrderStatus status) => Res.Color(status switch
    {
        OrderStatus.Pending => "PendingBg",
        OrderStatus.InProgress => "ProgressBg",
        _ => "DoneBg"
    });

    public static Color Text(OrderStatus status) => Res.Color(status switch
    {
        OrderStatus.Pending => "PendingText",
        OrderStatus.InProgress => "ProgressText",
        _ => "DoneText"
    });
}
