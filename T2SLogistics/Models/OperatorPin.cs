namespace T2SLogistics.Models;

/// <summary>Operador para o seletor do ecrã Definir/Alterar PIN (nomes casam com o DTO da API, camelCase).</summary>
public sealed class OperatorSummary
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool MustSetPin { get; set; }
}

/// <summary>Operador identificado por um PIN válido (resposta do verify-pin).</summary>
public sealed class OperatorIdentity
{
    public string OperatorId { get; set; } = string.Empty;
    public string OperatorName { get; set; } = string.Empty;
}

/// <summary>Desfecho de uma operação de PIN (definir/alterar): sucesso, ou mensagem de erro da API.</summary>
public readonly record struct PinOperationResult(bool Ok, string? Error)
{
    public static PinOperationResult Success => new(true, null);
    public static PinOperationResult Fail(string? error) => new(false, error);
}

/// <summary>Desfecho de um pedido de impressão com PIN.</summary>
public enum PrintResult
{
    Enqueued,   // 2xx: enfileirado
    InvalidPin, // 401: PIN não pertence a nenhum operador
    Failed,     // outro erro
}
