using T2SLogistics.Models;

namespace T2SLogistics.Services.Api;

/// <summary>
/// Implementação de demonstração com dados de exemplo, em memória. Permite desenvolver e ver
/// toda a UI sem servidor. Substituir por uma implementação HTTP quando a API estiver pronta.
/// </summary>
public sealed class MockApiService : IApiService
{
    public Task<IReadOnlyList<OrderSummary>> GetOrdersAsync(
        LogisticsModule module, OrderParty party, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<OrderSummary> data = BuildOrders(module, party);
        return Task.FromResult(data);
    }

    public Task<OrderDetail?> GetOrderAsync(
        LogisticsModule module, string number, CancellationToken cancellationToken = default)
    {
        // O detalhe é independente do filtro Clientes/Fornecedores; procura nos dois.
        var summary = BuildOrders(module, OrderParty.Clients)
            .Concat(BuildOrders(module, OrderParty.Suppliers))
            .FirstOrDefault(o => o.Number == number);

        OrderDetail? detail = summary is null ? null : new OrderDetail
        {
            Number = summary.Number,
            ClientName = summary.ClientName,
            Address = "Rua das Indústrias, 120 · 4400-999 Vila Nova de Gaia",
            Date = summary.Date,
            Status = summary.Status,
            Lines = BuildLines(summary)
        };

        return Task.FromResult(detail);
    }

    public Task<StartedReception?> StartReceptionAsync(
        string phcOrderId, CancellationToken cancellationToken = default)
    {
        var order = BuildOrders(LogisticsModule.Orders, OrderParty.Suppliers).FirstOrDefault(o => o.Number == phcOrderId)
                    ?? BuildOrders(LogisticsModule.Orders, OrderParty.Suppliers).First();

        var lines = BuildLines(order);
        StartedReception? started = new StartedReception
        {
            ReceptionId = Guid.NewGuid().ToString(),
            PhcOrderId = phcOrderId,
            Status = "em receção",
            Items = lines.Select(l => new ReceptionExpectedLine
            {
                ProductRef = l.Code,
                Description = l.Description,
                Code = l.Code,
                Expected = l.Total,
            }).ToList(),
        };
        return Task.FromResult(started);
    }

    public Task<ReceptionReadingResult> RecordReceptionReadingAsync(
        string receptionId, ReceptionReadingInput input, CancellationToken cancellationToken = default)
    {
        // Demonstração: aceita sempre a leitura como completa.
        var result = new ReceptionReadingResult
        {
            Success = true,
            ProductRef = input.Barcode,
            ReceivedQuantity = input.Quantity,
            LineStatus = "completo",
        };
        return Task.FromResult(result);
    }

    private static IReadOnlyList<OrderSummary> BuildOrders(LogisticsModule module, OrderParty party)
    {
        var prefix = module switch
        {
            LogisticsModule.Orders => "ENC",
            _ => "INV"
        };

        var clients = party == OrderParty.Clients;
        var names = clients
            ? new[] { "Farmácia Central", "Clínica do Sol", "Distribuidora Norte", "Hospital de Gaia", "Parafarmácia Lima" }
            : new[] { "Fornecedor Urvina", "Sândalo Lda.", "VetLima", "HM Pharma", "Logística Ibérica" };

        var statuses = new[]
        {
            OrderStatus.Pending, OrderStatus.InProgress, OrderStatus.Done,
            OrderStatus.Pending, OrderStatus.InProgress
        };

        return Enumerable.Range(0, names.Length).Select(i => new OrderSummary
        {
            Number = $"{prefix}-2026/{(party == OrderParty.Clients ? 100 : 500) + i:000}",
            ClientName = names[i],
            Date = DateTime.Today.AddDays(-i),
            LineCount = 4 + i * 2,
            Status = statuses[i % statuses.Length],
            Party = party
        }).ToList();
    }

    private static IReadOnlyList<OrderLine> BuildLines(OrderSummary summary)
    {
        var products = new (string Desc, string Code)[]
        {
            ("Paracetamol 1000mg 20 comp.", "5601234000019"),
            ("Ibuprofeno 600mg 30 comp.", "5601234000026"),
            ("Soro fisiológico 500ml", "5601234000033"),
            ("Compressas esterilizadas", "5601234000040"),
            ("Luvas nitrilo M (100 un.)", "5601234000057"),
            ("Álcool etílico 70% 1L", "5601234000064"),
            ("Seringa 5ml (caixa 100)", "5601234000071"),
            ("Adesivo hipoalergénico", "5601234000088")
        };

        return Enumerable.Range(0, summary.LineCount).Select(i =>
        {
            var p = products[i % products.Length];
            var total = 4 + (i % 5) * 2;
            var picked = summary.Status == OrderStatus.Done ? total
                       : summary.Status == OrderStatus.InProgress ? total / 2
                       : 0;
            return new OrderLine { Description = p.Desc, Code = p.Code, Picked = picked, Total = total };
        }).ToList();
    }
}
