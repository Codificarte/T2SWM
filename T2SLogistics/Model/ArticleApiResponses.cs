namespace T2SLogistics.Model
{
    // Resposta de resolver um código de barras para o Artigo (GET articles/by-barcode/{barcode}/capabilities).
    // camelCase para casar com o System.Text.Json do servidor (Newtonsoft desserializa case-insensitive).
    public class ArticleByBarcodeApiResponse
    {
        public string? articleRef { get; set; }
        public bool usesLote { get; set; }
        public bool usesSerialNumber { get; set; }
        public bool usesAlveolo { get; set; }
    }
}
