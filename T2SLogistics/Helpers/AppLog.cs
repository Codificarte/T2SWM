using System;

namespace T2SLogistics.Helpers
{
    /// <summary>
    /// Log simples para o logcat (tag DOTNET no Android). Usar em qualquer sítio:
    /// <c>AppLog.Write("a minha mensagem");</c> e ver com <c>adb logcat -s DOTNET</c>
    /// (ou filtrar por <c>[T2S]</c>). Prefixo + timestamp para leitura fácil.
    /// </summary>
    public static class AppLog
    {
        public static void Write(string message)
        {
            System.Console.WriteLine($"[T2S] {DateTime.Now:HH:mm:ss.fff} | {message}");
        }

        /// <summary>Log de exceção com contexto.</summary>
        public static void Error(string context, Exception ex)
        {
            System.Console.WriteLine($"[T2S] {DateTime.Now:HH:mm:ss.fff} | ERRO em {context}: {ex.Message}");
        }
    }
}
