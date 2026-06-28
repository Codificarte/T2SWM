namespace T2SLogistics.Models;

/// <summary>
/// Contextos que partilham a mesma lista/detalhe reutilizáveis, variando apenas título e ação.
/// "Encomendas" unifica expedição (clientes) e receção (fornecedores) — a distinção é feita pelo
/// filtro Clientes/Fornecedores na própria lista.
/// </summary>
public enum LogisticsModule
{
    Orders,
    Inventory
}

/// <summary>Metadados de apresentação por módulo (o que muda entre os contextos da lista/detalhe).</summary>
public sealed record LogisticsModuleInfo(
    LogisticsModule Module,
    string Title,
    string ActionText)
{
    public static LogisticsModuleInfo For(LogisticsModule module) => module switch
    {
        LogisticsModule.Orders => new(module, "Encomendas", "Iniciar expedição"),
        LogisticsModule.Inventory => new(module, "Inventário", "Iniciar inventário"),
        _ => throw new ArgumentOutOfRangeException(nameof(module))
    };

    /// <summary>Resolve a partir do nome do enum (query param do Shell). Default: Encomendas.</summary>
    public static LogisticsModuleInfo Parse(string? value) =>
        For(Enum.TryParse<LogisticsModule>(value, ignoreCase: true, out var m) ? m : LogisticsModule.Orders);
}
