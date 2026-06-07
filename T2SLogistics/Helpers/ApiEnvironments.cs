using System.Collections.Generic;

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
    }
}
