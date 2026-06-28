using Android.Content;
using AndroidApp = Android.App.Application;

namespace T2SLogistics.Services.Scanning;

/// <summary>
/// Leitura em terminais Zebra através do DataWedge com saída por Intent (broadcast). Regista um
/// <see cref="BroadcastReceiver"/> em runtime que apanha o intent emitido pelo DataWedge e extrai
/// a string do código.
///
/// CONFIGURAÇÃO DO PERFIL DATAWEDGE (tem de ser criado e associado a esta app):
///   1. App DataWedge no terminal Zebra → criar um perfil (ex.: "T2SLogistics").
///   2. Associar o perfil ao pacote da app (<c>com.companyname.t2slogistics</c>).
///   3. Input: ativar "Barcode input".
///   4. Output: ativar "Intent output" e desativar "Keystroke output".
///        - Intent action: <c>com.t2slogistics.SCAN</c> (igual a <see cref="ActionScan"/>).
///        - Intent delivery: "Broadcast intent".
///   O código lido chega no extra <c>com.symbol.datawedge.data_string</c> (chave padrão DataWedge).
/// </summary>
public sealed class DataWedgeBarcodeScanner : BarcodeScannerBase
{
    public const string ActionScan = "com.t2slogistics.SCAN";
    private const string ExtraData = "com.symbol.datawedge.data_string";

    private Receiver? _receiver;

    public override void Start()
    {
        if (_receiver is not null)
            return;

        _receiver = new Receiver(RaiseScanned);
        var filter = new IntentFilter(ActionScan);

        if (OperatingSystem.IsAndroidVersionAtLeast(33))
            AndroidApp.Context.RegisterReceiver(_receiver, filter, ReceiverFlags.Exported);
        else
            AndroidApp.Context.RegisterReceiver(_receiver, filter);
    }

    public override void Stop()
    {
        if (_receiver is null)
            return;

        AndroidApp.Context.UnregisterReceiver(_receiver);
        _receiver.Dispose();
        _receiver = null;
    }

    private sealed class Receiver : BroadcastReceiver
    {
        private readonly Action<string?> _onScan;

        public Receiver(Action<string?> onScan) => _onScan = onScan;

        public override void OnReceive(Context? context, Intent? intent)
            => _onScan(intent?.GetStringExtra(ExtraData));
    }
}
