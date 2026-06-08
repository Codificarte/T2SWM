using System;
using System.Collections.Generic;
using System.Linq;

namespace T2SLogistics.Helpers
{
    public record ApiEnvironment(string Name, string Url);

    public static class ApiEnvironments
    {
        public static readonly ApiEnvironment Dev = new("Dev", "http://192.168.1.77:5135/api/");
        public static readonly ApiEnvironment Teste = new("Teste", "http://192.168.0.0:7275/api/"); // PLACEHOLDER — endereço interno a configurar
        public static readonly ApiEnvironment Prod = new("Prod", "http://192.168.0.0:5135/api/"); // PLACEHOLDER — endereço interno a configurar

        public static readonly List<ApiEnvironment> All = new() { Dev, Teste, Prod };

        public static ApiEnvironment Default => Dev;

        // Hosts da API ANTIGA — a app NUNCA pode falar com estes. Denylist (não allowlist exata)
        // porque o IP de Dev é editável por local; só bloqueamos os servidores antigos conhecidos.
        private static readonly string[] LegacyHostFragments = { "codificarte.pt", "createinfor.pt" };

        /// <summary>True se a URL aponta para um servidor da API antiga (a rejeitar).</summary>
        public static bool IsLegacy(string url) =>
            !string.IsNullOrWhiteSpace(url) &&
            LegacyHostFragments.Any(h => url.IndexOf(h, StringComparison.OrdinalIgnoreCase) >= 0);
    }
}
