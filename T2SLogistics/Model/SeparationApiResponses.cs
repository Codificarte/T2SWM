using System;
using System.Collections.Generic;

namespace T2SLogistics.Model
{
    // DTOs de resposta da nova API para a Separação/Expedição (Épico 3). Campos em camelCase para casar
    // com o System.Text.Json do servidor (Newtonsoft desserializa case-insensitive de qualquer forma).

    /// <summary>Resposta ao iniciar a Separação (POST api/separations). Itens reutilizam o DTO de linha da encomenda.</summary>
    public class SeparationApiResponse
    {
        public string? separationId { get; set; }
        public string? phcOrderId { get; set; }
        public string? operatorId { get; set; }
        public string? operatorName { get; set; }
        public string? status { get; set; }
        public DateTime? createdAtUtc { get; set; }
        public List<CustomerOrderItemResponse>? items { get; set; }
    }

    /// <summary>Resposta ao registar uma leitura de separação (POST api/separations/{id}/readings) com sucesso (200).</summary>
    public class SeparationItemApiResponse
    {
        public string? separationItemId { get; set; }
        public string? separationId { get; set; }
        public string? phcOrderItemLineId { get; set; }
        public string? productRef { get; set; }
        public int requiredQuantity { get; set; }
        public int separatedQuantity { get; set; }
        public string? lote { get; set; }
        public string? alveolo { get; set; }
        public string? status { get; set; }
        public string? observation { get; set; }
        public DateTime readAtUtc { get; set; }
    }
}
