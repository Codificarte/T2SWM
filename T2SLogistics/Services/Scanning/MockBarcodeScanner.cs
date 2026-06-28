namespace T2SLogistics.Services.Scanning;

/// <summary>
/// Scanner de testes: não fala com hardware. A UI oferece um campo de input manual que chama
/// <see cref="BarcodeScannerBase.Submit"/> para simular uma leitura. Ideal para Windows/emulador.
/// </summary>
public sealed class MockBarcodeScanner : BarcodeScannerBase
{
    public override void Submit(string code) => RaiseScanned(code);
}
