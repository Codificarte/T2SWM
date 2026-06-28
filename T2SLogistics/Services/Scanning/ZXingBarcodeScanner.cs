namespace T2SLogistics.Services.Scanning;

/// <summary>
/// Leitura por câmara via ZXing.Net.Maui (já referenciado no projeto). Como a câmara é um controlo
/// visual, o padrão é: uma página hospeda um <c>CameraBarcodeReaderView</c> e, no evento
/// <c>BarcodesDetected</c>, encaminha o valor para <see cref="OnDetected"/>. Mantém a mesma interface
/// que as restantes implementações, pelo que os VMs não precisam de saber que é câmara.
/// </summary>
public sealed class ZXingBarcodeScanner : BarcodeScannerBase
{
    /// <summary>Chamado pela página da câmara quando o ZXing deteta um código.</summary>
    public void OnDetected(string code) => RaiseScanned(code);

    // Submit também encaminha, caso se queira injetar manualmente durante testes.
    public override void Submit(string code) => RaiseScanned(code);
}
