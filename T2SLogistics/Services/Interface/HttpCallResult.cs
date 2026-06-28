namespace T2SLogistics.Services.Interface;

/// <summary>
/// Resultado cru de uma chamada HTTP: status + corpo, sem lançar em respostas não-2xx. Permite ao chamador
/// desserializar o tipo de sucesso (2xx) ou ler a mensagem de erro do corpo (ex.: 422 com <c>{ "message": ... }</c>).
/// </summary>
public sealed class HttpCallResult
{
    public int StatusCode { get; init; }
    public string Body { get; init; } = string.Empty;

    /// <summary>True se o status está em 2xx.</summary>
    public bool IsSuccess => StatusCode is >= 200 and < 300;

    /// <summary>True quando a rota não está migrada (bloqueada antes de tocar na rede) — status 0.</summary>
    public bool IsBlocked => StatusCode == 0;
}
