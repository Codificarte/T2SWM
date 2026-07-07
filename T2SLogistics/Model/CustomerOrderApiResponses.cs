using System;
using System.Collections.Generic;

namespace T2SLogistics.Model
{
    // DTOs de leitura isolados do contrato plano da nova API (api/customer-orders).
    // Mantidos separados do CustomersOrderModel/OrderItem (partilhados com a separação) — o
    // CustomerOrderService mapeia estes para o modelo existente.

    public class CustomerOrderSummaryResponse
    {
        public string? phcOrderId { get; set; }
        public int orderNumber { get; set; }
        public int lineCount { get; set; }
        public string? customerId { get; set; }
        public string? customerName { get; set; }
        public DateTime? orderDate { get; set; }
        public DateTime? deliveryDate { get; set; }
        public string? status { get; set; }
    }

    public class CustomerOrderItemResponse
    {
        public string? phcOrderItemLineId { get; set; }
        public string? productRef { get; set; }
        public string? productDescription { get; set; }
        public string? eanCode { get; set; }
        public int quantity { get; set; }
        // Expedição: o que tem de sair nesta linha (correspondência estrita na leitura). expiry é informativa.
        public string? lote { get; set; }
        public DateTime? expiry { get; set; }
        public string? binLocation { get; set; }
    }

    public class CustomerOrderDetailResponse
    {
        public string? phcOrderId { get; set; }
        public int orderNumber { get; set; }
        public string? customerId { get; set; }
        public string? customerName { get; set; }
        public DateTime? orderDate { get; set; }
        public DateTime? deliveryDate { get; set; }
        public string? status { get; set; }
        public string? requisicao { get; set; }
        public List<CustomerOrderItemResponse>? items { get; set; }
    }
}
