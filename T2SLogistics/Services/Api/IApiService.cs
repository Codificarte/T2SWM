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
    /// Inicia uma Receção a partir de uma Encomenda de Fornecedor (POST api/receptions). Devolve a Receção
    /// (id + itens esperados) ou <c>null</c> se a encomenda não existe/é inelegível ou a chamada falha.
    /// Início idempotente no servidor: repetir devolve a Receção em curso.
    /// </summary>
    Task<StartedReception?> StartReceptionAsync(
        string phcOrderId,
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
}
