namespace T2SLogistics.Models;

/// <summary>Receção iniciada: identidade + itens esperados da encomenda de fornecedor.</summary>
public sealed class StartedReception
{
    public string ReceptionId { get; init; } = string.Empty;
    public string PhcOrderId { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public IReadOnlyList<ReceptionExpectedLine> Items { get; init; } = Array.Empty<ReceptionExpectedLine>();
}

/// <summary>Linha esperada da encomenda (o que se prevê rececionar).</summary>
public sealed class ReceptionExpectedLine
{
    public string ProductRef { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty; // EAN ou ref (o que se lê)
    public int Expected { get; init; }
}

/// <summary>Dados de uma leitura de conferência a enviar para a API.</summary>
public sealed class ReceptionReadingInput
{
    public string Barcode { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public string? Lote { get; init; }
    public DateTime? ExpiryDate { get; init; }
    public string? Alveolo { get; init; }
    public string? Observation { get; init; }
}

/// <summary>
/// Resultado de registar uma leitura. Em sucesso traz a linha persistida (para atualizar o progresso);
/// em recusa de negócio (422) ou erro traz a <see cref="Message"/> pronta para mostrar ao operador.
/// </summary>
public sealed class ReceptionReadingResult
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public string ProductRef { get; init; } = string.Empty;
    public int ReceivedQuantity { get; init; }
    public string LineStatus { get; init; } = string.Empty; // "completo" / "incompleto"
}

/// <summary>
/// Resultado de interpretar uma Leitura na API (FR-10). Em sucesso traz a identificação do Artigo
/// (GTIN para GS1, ou o próprio código) + Lote/Validade/Nº Série conforme presentes; em erro traz a
/// <see cref="Message"/> pronta para mostrar. A App nunca interpreta GS1 localmente.
/// </summary>
public sealed class ParsedScanResult
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public string ArticleCode { get; init; } = string.Empty;
    public bool IsGs1 { get; init; }
    public string? Gtin { get; init; }
    public string? Lote { get; init; }
    public DateTime? ExpiryDate { get; init; }
    public string? SerialNumber { get; init; }
}
