using System;
using System.Collections.Generic;

namespace T2SLogistics.Model
{
    // DTOs de resposta da nova API para a Receção (Épico 2). Campos em camelCase para casar com o
    // System.Text.Json do servidor (Newtonsoft desserializa case-insensitive de qualquer forma).

    /// <summary>Resposta ao iniciar a Receção (POST api/receptions). Itens esperados reutilizam o DTO de leitura.</summary>
    public class ReceptionApiResponse
    {
        public string? receptionId { get; set; }
        public string? phcOrderId { get; set; }
        public string? operatorId { get; set; }
        public string? operatorName { get; set; }
        public string? status { get; set; }
        public DateTime? createdAtUtc { get; set; }
        public List<CustomerOrderItemResponse>? items { get; set; }
    }

    /// <summary>Resposta ao registar uma leitura de conferência (POST api/receptions/{id}/readings) com sucesso (200).</summary>
    public class ReceptionItemApiResponse
    {
        public string? receptionItemId { get; set; }
        public string? receptionId { get; set; }
        public string? phcOrderItemLineId { get; set; }
        public string? productRef { get; set; }
        public int expectedQuantity { get; set; }
        public int receivedQuantity { get; set; }
        public string? lote { get; set; }
        public DateTime? expiryDate { get; set; }
        public string? status { get; set; }
        public string? observation { get; set; }
        public string? alveolo { get; set; }
        public DateTime readAtUtc { get; set; }
    }

    /// <summary>Corpo de erro padrão da API (ex.: 422/400 devolvem <c>{ "message": "..." }</c>).</summary>
    public class ApiMessageResponse
    {
        public string? message { get; set; }
    }
}
