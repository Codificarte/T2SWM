using System;
using System.Linq;

namespace T2SLogistics.Services.ApiServices
{
    public class ApiBase
    {
        public const string AuthLoginKey = "auth/login";
        public const string AuthSetInitialPasswordKey = "auth/set-initial-password";
        public const string PhcOrdersKey = "PhcOrders/GetOrdersProd";
        public const string PhcOrdersByCodeKey = "PhcOrders/GetOrdersProd/{0}";

        // Antiga (API antiga) — ainda usada pelo fluxo de separação/expedição (Épico 3).
        public const string GetCustomerOrdersKey = "PhcOrders/GetCustomerOrders";
        // Nova API (leitura) — lista leve + detalhe com linhas.
        public const string CustomerOrdersReadKey = "customer-orders";
        public const string CustomerOrderDetailKey = "customer-orders/{0}";
        // Nova API (leitura) — encomendas a fornecedor (ordertype=1).
        public const string SupplierOrdersReadKey = "supplier-orders";
        public const string SupplierOrderDetailKey = "supplier-orders/{0}";
        // Nova API (escrita) — Receção (Épico 2): iniciar a partir de enc. fornecedor + registar leituras.
        public const string ReceptionsKey = "receptions";
        public const string ReceptionReadingsKey = "receptions/{0}/readings";
        // Nova API (escrita) — Separação/Expedição (Épico 3): iniciar a partir de enc. cliente + registar leituras.
        public const string SeparationsKey = "separations";
        public const string SeparationReadingsKey = "separations/{0}/readings";
        // Nova API (leitura) — parsing de código de barras / QR GS1 (Story 1.8). A App envia o payload
        // cru e a API devolve GTIN/Lote/Validade/Nº Série separados.
        public const string ScansParseKey = "scans/parse";
        // Nova API (leitura) — resolve um código de barras para o Artigo no PHC (st.codigo → ref). Usado
        // na Expedição para validar que o código lido pertence mesmo à referência da linha.
        public const string ArticleByBarcodeKey = "articles/by-barcode/{0}/capabilities";
        public const string CreateUsersAppTimerKey= "UsersAppTimer/create";
        public const string ProductionEntriesKey = "ProductionEntries";
        public const string AddSeprationItemsKey = "PhcOrders/AddReadSeparationItems";
        public const string AddSeprationItemKey = "PhcOrders/AddReadSeparationItem";

        public const string AddExpeditionKey = "PhcOrders/AddExpeditionReadItems";
        public const string GetRefProductKey = "PhcOrders/GetRefProduct/{0}";
        public const string CloseExpeditionKey = "PhcOrders/CloseExpedition";
        public const string DeleteExpeditionKey = "PhcOrders/ExpeditionReadItems/Delete/{0}";
        public const string CancelReadExpeditionKey = "PhcOrders/CancelReadExpedition/{0}";

        // Rotas já migradas para a NOVA API. SÓ estas podem sair para a rede; qualquer outra rota
        // (ainda da API antiga) é bloqueada no RequestProvider e mostra "em migração". Ao migrar
        // uma feature, acrescentar aqui o prefixo da sua rota nova.
        private static readonly string[] MigratedRoutePrefixes = { "auth/", "customer-orders", "supplier-orders", "receptions", "separations", "scans", "articles" };

        /// <summary>True se a rota já foi migrada para a nova API (pode sair para a rede).</summary>
        public static bool IsRouteMigrated(string endpoint)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
                return false;
            var route = endpoint.TrimStart('/');
            return MigratedRoutePrefixes.Any(p => route.StartsWith(p, StringComparison.OrdinalIgnoreCase));
        }
    }
}
