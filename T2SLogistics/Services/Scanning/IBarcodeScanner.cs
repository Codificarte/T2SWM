namespace T2SLogistics.Services.Scanning;

/// <summary>
/// Abstração única para leitura de código de barras. A página/VM subscreve <see cref="BarcodeScanned"/>
/// e nunca conhece a origem concreta (Zebra DataWedge, keyboard-wedge, câmara ZXing ou mock).
/// A implementação concreta é escolhida por injeção de dependências (ver MauiProgram).
/// </summary>
public interface IBarcodeScanner
{
    /// <summary>Disparado quando um código é lido. O argumento é o código já normalizado (trim).</summary>
    event Action<string>? BarcodeScanned;

    /// <summary>Começa a escutar (ex.: regista o BroadcastReceiver do DataWedge). Idempotente.</summary>
    void Start();

    /// <summary>Pára de escutar e liberta recursos. Idempotente.</summary>
    void Stop();

    /// <summary>
    /// Injeta manualmente um código. Usado pelo mock (input manual) e pelo keyboard-wedge
    /// (Entry escondida que recebe os caracteres + Enter). Nas implementações de hardware é no-op.
    /// </summary>
    void Submit(string code);
}

/// <summary>Base partilhada: gere o evento e a normalização.</summary>
public abstract class BarcodeScannerBase : IBarcodeScanner
{
    public event Action<string>? BarcodeScanned;

    protected void RaiseScanned(string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return;

        BarcodeScanned?.Invoke(code.Trim());
    }

    public virtual void Start() { }
    public virtual void Stop() { }
    public virtual void Submit(string code) { }
}
