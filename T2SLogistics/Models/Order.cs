namespace T2SLogistics.Models;

public enum OrderStatus
{
    Pending,
    InProgress,
    Done
}

/// <summary>Filtro Clientes / Fornecedores da lista.</summary>
public enum OrderParty
{
    Clients,
    Suppliers
}

/// <summary>Cabeçalho leve para a listagem (sem linhas).</summary>
public sealed class OrderSummary
{
    public string Number { get; init; } = string.Empty;
    public string ClientName { get; init; } = string.Empty;
    public DateTime Date { get; init; }
    public int LineCount { get; init; }
    public OrderStatus Status { get; init; }
    public OrderParty Party { get; init; }
}

/// <summary>Detalhe da encomenda, com as linhas.</summary>
public sealed class OrderDetail
{
    public string Number { get; init; } = string.Empty;
    public string ClientName { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public DateTime Date { get; init; }
    public OrderStatus Status { get; init; }
    public IReadOnlyList<OrderLine> Lines { get; init; } = Array.Empty<OrderLine>();
}

public sealed class OrderLine
{
    public string Description { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public string ProductRef { get; init; } = string.Empty; // ref do PHC (casa a leitura com a linha esperada)
    public int Picked { get; init; }
    public int Total { get; init; }
}
