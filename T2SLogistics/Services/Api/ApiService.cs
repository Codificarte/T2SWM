using Newtonsoft.Json;
using T2SLogistics.Helpers;
using T2SLogistics.Model;
using T2SLogistics.Models;
using T2SLogistics.Services.ApiServices;
using T2SLogistics.Services.Interface;

namespace T2SLogistics.Services.Api;

/// <summary>
/// Implementação HTTP de <see cref="IApiService"/> sobre a nova API, via <see cref="IRequestProvider"/>
/// (que lê BaseUrl/token em runtime). Substitui o <see cref="MockApiService"/>.
/// <para>
/// <b>Encomendas</b>: Clientes → <c>customer-orders</c> (ordertype=2); Fornecedores →
/// <c>supplier-orders</c> (ordertype=1). <b>Inventário</b> ainda não tem endpoint → vazio. A lista é
/// leve (sem nº de linhas) e o estado de separação por linha ainda não vem da leitura (<c>Picked</c>=0).
/// </para>
/// </summary>
public sealed class ApiService : IApiService
{
    private readonly IRequestProvider _requestProvider;

    public ApiService(IRequestProvider requestProvider) => _requestProvider = requestProvider;

    public async Task<IReadOnlyList<OrderSummary>> GetOrdersAsync(
        LogisticsModule module, OrderParty party, CancellationToken cancellationToken = default)
    {
        // Inventário ainda não tem leitura na nova API.
        if (module != LogisticsModule.Orders)
            return Array.Empty<OrderSummary>();

        var endpoint = party == OrderParty.Suppliers
            ? ApiBase.SupplierOrdersReadKey
            : ApiBase.CustomerOrdersReadKey;

        try
        {
            var response = await _requestProvider.Get<List<CustomerOrderSummaryResponse>>(endpoint);
            AppLog.Write($"GetOrdersAsync({party}): {(response is null ? "null" : response.Count.ToString())} encomendas");
            if (response is null)
                return Array.Empty<OrderSummary>();

            return response.Select(r => new OrderSummary
            {
                Number = r.orderNumber.ToString(), // nº da encomenda (obrano) — visível ao operador
                PhcOrderId = r.phcOrderId ?? string.Empty, // bostamp — chave interna p/ abrir o detalhe
                ClientName = r.customerName ?? string.Empty, // p/ fornecedores, carrega o nome do Fornecedor
                Date = r.orderDate ?? default,
                LineCount = r.lineCount, // nº de linhas contado pela API na listagem
                Status = MapStatus(r.status),
                Party = party,
            }).ToList();
        }
        catch (Exception ex)
        {
            AppLog.Error(nameof(GetOrdersAsync), ex);
            return Array.Empty<OrderSummary>();
        }
    }

    public async Task<OrderDetail?> GetOrderAsync(
        LogisticsModule module, string number, CancellationToken cancellationToken = default)
    {
        if (module != LogisticsModule.Orders || string.IsNullOrWhiteSpace(number))
            return null;

        // O bostamp é único entre cliente/fornecedor — o detalhe não recebe o party, por isso tenta
        // primeiro cliente e, se não casar (404→null), tenta fornecedor.
        return await FetchDetailAsync(ApiBase.CustomerOrderDetailKey, number)
            ?? await FetchDetailAsync(ApiBase.SupplierOrderDetailKey, number);
    }

    private async Task<OrderDetail?> FetchDetailAsync(string keyFormat, string number)
    {
        try
        {
            var endpoint = string.Format(keyFormat, Uri.EscapeDataString(number));
            var d = await _requestProvider.Get<CustomerOrderDetailResponse>(endpoint);
            if (d is null || string.IsNullOrEmpty(d.phcOrderId))
                return null;

            return new OrderDetail
            {
                Number = d.orderNumber.ToString(), // nº da encomenda (obrano) — visível ao operador
                PhcOrderId = d.phcOrderId ?? string.Empty, // bostamp — chave interna
                ClientName = d.customerName ?? string.Empty,
                Address = string.Empty, // a leitura não fornece morada
                Date = d.orderDate ?? default,
                Status = MapStatus(d.status),
                Lines = (d.items ?? new List<CustomerOrderItemResponse>()).Select(i => new OrderLine
                {
                    Description = i.productDescription ?? i.productRef ?? string.Empty,
                    Code = string.IsNullOrWhiteSpace(i.eanCode) ? (i.productRef ?? string.Empty) : i.eanCode!,
                    ProductRef = i.productRef ?? string.Empty,
                    Picked = 0, // estado de separação ainda não vem da leitura
                    Total = i.quantity,
                    Lote = i.lote,
                    Expiry = i.expiry,
                    BinLocation = i.binLocation,
                }).ToList(),
            };
        }
        catch (Exception ex)
        {
            AppLog.Error(nameof(GetOrderAsync), ex);
            return null;
        }
    }

    public async Task<StartedReception?> StartReceptionAsync(
        string phcOrderId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(phcOrderId))
            return null;

        try
        {
            var body = JsonConvert.SerializeObject(new { phcOrderId });
            var res = await _requestProvider.PostWithStatus(ApiBase.ReceptionsKey, body);
            if (!res.IsSuccess)
            {
                AppLog.Write($"StartReceptionAsync <- {res.StatusCode} (sem receção)");
                return null;
            }

            var dto = JsonConvert.DeserializeObject<ReceptionApiResponse>(res.Body);
            if (dto is null || string.IsNullOrWhiteSpace(dto.receptionId))
                return null;

            return new StartedReception
            {
                ReceptionId = dto.receptionId!,
                PhcOrderId = dto.phcOrderId ?? phcOrderId,
                Status = dto.status ?? string.Empty,
                Items = (dto.items ?? new List<CustomerOrderItemResponse>()).Select(i => new ReceptionExpectedLine
                {
                    ProductRef = i.productRef ?? string.Empty,
                    Description = i.productDescription ?? i.productRef ?? string.Empty,
                    Code = string.IsNullOrWhiteSpace(i.eanCode) ? (i.productRef ?? string.Empty) : i.eanCode!,
                    Expected = i.quantity,
                }).ToList(),
            };
        }
        catch (Exception ex)
        {
            AppLog.Error(nameof(StartReceptionAsync), ex);
            return null;
        }
    }

    public async Task<ReceptionReadingResult> RecordReceptionReadingAsync(
        string receptionId, ReceptionReadingInput input, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(receptionId) || input is null || string.IsNullOrWhiteSpace(input.Barcode) || input.Quantity <= 0)
            return new ReceptionReadingResult { Success = false, Message = "Código e quantidade (> 0) são obrigatórios." };

        try
        {
            var endpoint = string.Format(ApiBase.ReceptionReadingsKey, Uri.EscapeDataString(receptionId));
            var body = JsonConvert.SerializeObject(new
            {
                barcode = input.Barcode,
                quantity = input.Quantity,
                lote = input.Lote,
                expiryDate = input.ExpiryDate,
                observation = input.Observation,
                alveolo = input.Alveolo,
            });

            var res = await _requestProvider.PostWithStatus(endpoint, body);

            if (res.IsSuccess)
            {
                var item = JsonConvert.DeserializeObject<ReceptionItemApiResponse>(res.Body);
                return new ReceptionReadingResult
                {
                    Success = true,
                    ProductRef = item?.productRef ?? string.Empty,
                    ReceivedQuantity = item?.receivedQuantity ?? input.Quantity,
                    LineStatus = item?.status ?? string.Empty,
                };
            }

            return new ReceptionReadingResult { Success = false, Message = ExtractMessage(res) };
        }
        catch (Exception ex)
        {
            AppLog.Error(nameof(RecordReceptionReadingAsync), ex);
            return new ReceptionReadingResult { Success = false, Message = "Erro de comunicação com o servidor." };
        }
    }

    public async Task<StartedSeparation?> StartSeparationAsync(
        string phcOrderId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(phcOrderId))
            return null;

        try
        {
            var body = JsonConvert.SerializeObject(new { phcOrderId });
            var res = await _requestProvider.PostWithStatus(ApiBase.SeparationsKey, body);
            if (!res.IsSuccess)
            {
                AppLog.Write($"StartSeparationAsync <- {res.StatusCode} (sem separação)");
                return null;
            }

            var dto = JsonConvert.DeserializeObject<SeparationApiResponse>(res.Body);
            if (dto is null || string.IsNullOrWhiteSpace(dto.separationId))
                return null;

            return new StartedSeparation
            {
                SeparationId = dto.separationId!,
                PhcOrderId = dto.phcOrderId ?? phcOrderId,
                Status = dto.status ?? string.Empty,
            };
        }
        catch (Exception ex)
        {
            AppLog.Error(nameof(StartSeparationAsync), ex);
            return null;
        }
    }

    public async Task<SeparationReadingResult> RecordSeparationReadingAsync(
        string separationId, SeparationReadingInput input, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(separationId) || input is null || string.IsNullOrWhiteSpace(input.Barcode) || input.Quantity <= 0)
            return new SeparationReadingResult { Success = false, Message = "Código e quantidade (> 0) são obrigatórios." };

        try
        {
            var endpoint = string.Format(ApiBase.SeparationReadingsKey, Uri.EscapeDataString(separationId));
            var body = JsonConvert.SerializeObject(new
            {
                barcode = input.Barcode,
                quantity = input.Quantity,
                lote = input.Lote,
                alveolo = input.Alveolo,
                observation = input.Observation,
            });

            var res = await _requestProvider.PostWithStatus(endpoint, body);

            if (res.IsSuccess)
            {
                var item = JsonConvert.DeserializeObject<SeparationItemApiResponse>(res.Body);
                return new SeparationReadingResult
                {
                    Success = true,
                    ProductRef = item?.productRef ?? string.Empty,
                    Lote = item?.lote,
                    SeparatedQuantity = item?.separatedQuantity ?? input.Quantity,
                    RequiredQuantity = item?.requiredQuantity ?? 0,
                    LineStatus = item?.status ?? string.Empty,
                };
            }

            return new SeparationReadingResult { Success = false, Message = ExtractSeparationMessage(res) };
        }
        catch (Exception ex)
        {
            AppLog.Error(nameof(RecordSeparationReadingAsync), ex);
            return new SeparationReadingResult { Success = false, Message = "Erro de comunicação com o servidor." };
        }
    }

    // Traduz uma resposta não-2xx de uma leitura de separação numa mensagem para o operador.
    private static string ExtractSeparationMessage(HttpCallResult res)
    {
        if (res.IsBlocked)
            return "Funcionalidade em migração.";
        if (res.StatusCode == 401)
            return "Sessão expirada. Inicie sessão novamente.";
        if (res.StatusCode == 404)
            return "Separação não encontrada.";

        if (!string.IsNullOrWhiteSpace(res.Body))
        {
            try
            {
                var msg = JsonConvert.DeserializeObject<ApiMessageResponse>(res.Body)?.message;
                if (!string.IsNullOrWhiteSpace(msg))
                    return msg!;
            }
            catch { /* corpo não-JSON: cai no genérico */ }
        }

        return "Não foi possível registar a leitura.";
    }

    public async Task<string?> ResolveArticleRefByBarcodeAsync(
        string barcode, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(barcode))
            return null;

        try
        {
            var endpoint = string.Format(ApiBase.ArticleByBarcodeKey, Uri.EscapeDataString(barcode.Trim()));
            var dto = await _requestProvider.Get<ArticleByBarcodeApiResponse>(endpoint);
            var reference = dto?.articleRef;
            return string.IsNullOrWhiteSpace(reference) ? null : reference!.Trim();
        }
        catch (Exception ex)
        {
            AppLog.Error(nameof(ResolveArticleRefByBarcodeAsync), ex);
            return null;
        }
    }

    public async Task<ParsedScanResult?> ParseScanAsync(
        string payload, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(payload))
            return null;

        try
        {
            // A App envia o payload cru; o parsing GS1 é exclusivamente do servidor (FR-10).
            var body = JsonConvert.SerializeObject(new { payload });
            var res = await _requestProvider.PostWithStatus(ApiBase.ScansParseKey, body);

            if (res.IsSuccess)
            {
                var dto = JsonConvert.DeserializeObject<ParsedScanApiResponse>(res.Body);
                if (dto is null)
                    return new ParsedScanResult { Success = false, Message = "Resposta inválida do servidor." };

                return new ParsedScanResult
                {
                    Success = true,
                    ArticleCode = dto.articleCode ?? string.Empty,
                    IsGs1 = dto.isGs1,
                    Gtin = dto.gtin,
                    Lote = dto.lote,
                    ExpiryDate = dto.expiryDate,
                    SerialNumber = dto.serialNumber,
                };
            }

            return new ParsedScanResult { Success = false, Message = ExtractParseMessage(res) };
        }
        catch (Exception ex)
        {
            AppLog.Error(nameof(ParseScanAsync), ex);
            return new ParsedScanResult { Success = false, Message = "Erro de comunicação com o servidor." };
        }
    }

    // Mensagem de erro do parsing para o operador (o malformado/400/422 traz {message} do servidor).
    private static string ExtractParseMessage(HttpCallResult res)
    {
        if (res.IsBlocked)
            return "Funcionalidade em migração.";
        if (res.StatusCode == 401)
            return "Sessão expirada. Inicie sessão novamente.";

        if (!string.IsNullOrWhiteSpace(res.Body))
        {
            try
            {
                var msg = JsonConvert.DeserializeObject<ApiMessageResponse>(res.Body)?.message;
                if (!string.IsNullOrWhiteSpace(msg))
                    return msg!;
            }
            catch { /* corpo não-JSON: cai no genérico */ }
        }

        return "Não foi possível interpretar a leitura.";
    }

    // Traduz uma resposta não-2xx numa mensagem para o operador (usa o corpo {message} quando existe).
    private static string ExtractMessage(HttpCallResult res)
    {
        if (res.IsBlocked)
            return "Funcionalidade em migração.";
        if (res.StatusCode == 401)
            return "Sessão expirada. Inicie sessão novamente.";
        if (res.StatusCode == 404)
            return "Receção não encontrada.";

        if (!string.IsNullOrWhiteSpace(res.Body))
        {
            try
            {
                var msg = JsonConvert.DeserializeObject<ApiMessageResponse>(res.Body)?.message;
                if (!string.IsNullOrWhiteSpace(msg))
                    return msg!;
            }
            catch { /* corpo não-JSON: cai no genérico */ }
        }

        return "Não foi possível registar a leitura.";
    }

    // A nova API devolve "por separar"/"fechada" (sem estado "em curso" na leitura).
    private static OrderStatus MapStatus(string? status) =>
        string.Equals(status, "fechada", StringComparison.OrdinalIgnoreCase)
            ? OrderStatus.Done
            : OrderStatus.Pending;
}
