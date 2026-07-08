using T2SLogistics.Models;

namespace T2SLogistics.Services.Api;

/// <summary>
/// Toda a comunicação com a API passa por aqui. Os ViewModels dependem desta interface,
/// nunca de HttpClient. Por agora existe apenas a implementação mock (<see cref="MockApiService"/>);
/// quando a nova API expuser os endpoints, cria-se uma implementação HTTP e troca-se no DI.
/// </summary>
public interface IApiService
{
    Task<IReadOnlyList<OrderSummary>> GetOrdersAsync(
        LogisticsModule module,
        OrderParty party,
        CancellationToken cancellationToken = default);

    Task<OrderDetail?> GetOrderAsync(
        LogisticsModule module,
        string number,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Inicia uma Receção a partir de uma Encomenda de Fornecedor (POST api/receptions). O <paramref name="pin"/>
    /// identifica o operador que faz as leituras (PDA partilhado). Devolve a Receção (id + itens esperados) ou
    /// <c>null</c> se a encomenda não existe/é inelegível, o PIN é inválido, ou a chamada falha. Início
    /// idempotente no servidor: repetir devolve a Receção em curso.
    /// </summary>
    Task<StartedReception?> StartReceptionAsync(
        string phcOrderId, string pin,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Regista uma leitura de conferência contra uma Receção em curso (POST api/receptions/{id}/readings).
    /// As regras (lote/validade/alvéolo/quantidade) são validadas no servidor; em recusa o resultado traz a
    /// mensagem pronta para mostrar ao operador.
    /// </summary>
    Task<ReceptionReadingResult> RecordReceptionReadingAsync(
        string receptionId,
        ReceptionReadingInput input,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Inicia uma Separação a partir de uma Encomenda de Cliente (POST api/separations). O <paramref name="pin"/>
    /// identifica o operador que faz as leituras (PDA partilhado). Devolve a Separação (id) ou <c>null</c> se a
    /// encomenda não existe/é inelegível, o PIN é inválido, ou a chamada falha. Início idempotente no servidor.
    /// </summary>
    Task<StartedSeparation?> StartSeparationAsync(
        string phcOrderId, string pin,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Regista uma leitura de separação contra uma Separação em curso (POST api/separations/{id}/readings).
    /// As regras estritas (lote==encomenda, alvéolo==linha, quantidade não excede o pedido) são validadas no
    /// servidor; em recusa o resultado traz a mensagem pronta para mostrar ao operador.
    /// </summary>
    Task<SeparationReadingResult> RecordSeparationReadingAsync(
        string separationId,
        SeparationReadingInput input,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Resolve um código de barras para a referência do Artigo no PHC (GET articles/by-barcode/{barcode}/
    /// capabilities). Usado na Expedição para validar que o código lido pertence à referência da linha.
    /// Devolve a referência resolvida, ou <c>null</c> se o código não existe / não foi possível resolver.
    /// </summary>
    Task<string?> ResolveArticleRefByBarcodeAsync(
        string barcode,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Interpreta uma Leitura (código de barras / QR GS1) na API (POST api/scans/parse) — FR-10. A App
    /// envia o payload cru e nunca interpreta GS1 localmente. Em sucesso devolve a identificação do Artigo
    /// + Lote/Validade/Nº Série conforme presentes; em conteúdo malformado/erro, traz a mensagem tratada.
    /// Devolve <c>null</c> se não houver nada a interpretar (payload vazio).
    /// </summary>
    Task<ParsedScanResult?> ParseScanAsync(
        string payload,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Enfileira a impressão do PDF A4 de uma Encomenda de Cliente (POST api/customer-orders/{id}/print). A
    /// App não envia caminhos — o servidor resolve o u_filePath e enfileira; um agente externo imprime. O
    /// <paramref name="pin"/> identifica quem imprime (PDA partilhado); <see cref="PrintResult.InvalidPin"/>
    /// quando o PIN não pertence a nenhum operador.
    /// </summary>
    Task<PrintResult> PrintOrderAsync(
        string phcOrderId, string pin,
        CancellationToken cancellationToken = default);

    // ---- PIN do Operador (PDA partilhado: o PIN identifica quem age) ----

    /// <summary>Lista de operadores para o seletor de Definir/Alterar PIN (GET auth/operators).</summary>
    Task<IReadOnlyList<OperatorSummary>> GetOperatorsAsync(CancellationToken cancellationToken = default);

    /// <summary>Verifica um PIN e devolve o operador dono, ou <c>null</c> se inválido (POST auth/verify-pin).</summary>
    Task<OperatorIdentity?> VerifyPinAsync(string pin, CancellationToken cancellationToken = default);

    /// <summary>Define o 1.º PIN de um operador (POST auth/set-initial-pin).</summary>
    Task<PinOperationResult> SetInitialPinAsync(string operatorId, string newPin, CancellationToken cancellationToken = default);

    /// <summary>Altera o PIN de um operador, exigindo o PIN atual (POST auth/change-pin).</summary>
    Task<PinOperationResult> ChangePinAsync(string operatorId, string currentPin, string newPin, CancellationToken cancellationToken = default);
}
