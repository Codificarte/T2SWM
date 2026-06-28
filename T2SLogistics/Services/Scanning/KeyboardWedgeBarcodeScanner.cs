namespace T2SLogistics.Services.Scanning;

/// <summary>
/// Leitores genéricos em modo "keyboard wedge": enviam o código como se fossem teclado, terminando
/// com Enter. O padrão recomendado é colocar uma <c>Entry</c> escondida e sempre focada na página e,
/// no evento <c>Completed</c> (Enter), chamar <see cref="BarcodeScannerBase.Submit"/> com o texto lido,
/// limpando a seguir a Entry. Esta classe centraliza essa entrada num único ponto.
/// </summary>
public sealed class KeyboardWedgeBarcodeScanner : BarcodeScannerBase
{
    public override void Submit(string code) => RaiseScanned(code);
}
