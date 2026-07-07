namespace T2SLogistics.Models;

/// <summary>Separação iniciada: identidade (id) da separação em curso de uma encomenda de cliente.</summary>
public sealed class StartedSeparation
{
    public string SeparationId { get; init; } = string.Empty;
    public string PhcOrderId { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
}

/// <summary>
/// Linha esperada da encomenda a expedir. Na Expedição, <see cref="Lote"/>/<see cref="Expiry"/>/
/// <see cref="BinLocation"/> são o que TEM de sair — a leitura tem de bater certo com estes valores.
/// </summary>
public sealed class SeparationExpectedLine
{
    public string ProductRef { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty; // EAN ou ref (o que se lê)
    public int Expected { get; init; }
    public string? Lote { get; init; }
    public DateTime? Expiry { get; init; } // só informativa (deriva do lote)
    public string? BinLocation { get; init; } // alvéolo a ler
}

/// <summary>Dados de uma leitura de separação a enviar para a API (a validade não vai — deriva do lote).</summary>
public sealed class SeparationReadingInput
{
    public string Barcode { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public string? Lote { get; init; }
    public string? Alveolo { get; init; }
    public string? Observation { get; init; }
}

/// <summary>
/// Resultado de registar uma leitura de separação. Em sucesso traz a linha persistida (para atualizar o
/// progresso); em recusa (422) ou erro traz a <see cref="Message"/> pronta para mostrar ao operador.
/// </summary>
public sealed class SeparationReadingResult
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public string ProductRef { get; init; } = string.Empty;
    public string? Lote { get; init; }
    public int SeparatedQuantity { get; init; }
    public int RequiredQuantity { get; init; }
    public string LineStatus { get; init; } = string.Empty; // "completo" / "incompleto"
}
