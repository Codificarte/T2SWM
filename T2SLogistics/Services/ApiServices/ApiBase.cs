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
        private static readonly string[] MigratedRoutePrefixes = { "auth/", "customer-orders" };

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
