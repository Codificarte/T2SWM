using T2SLogistics.Model;
using T2SLogistics.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Services.ApiServices
{
    public class CustomerOrderService : ApiBase
    {
        IRequestProvider _requestProvider;

        public CustomerOrderService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        // Antigo (API antiga) — lista rica com estado de separação. Mantido para o fluxo de
        // separação/expedição (OrderSepration*/OrderReading), ainda não migrado para a nova API.
        public async Task<List<CustomersOrderModel>> GetCustomerOrder()
        {
            try
            {
                var response = await _requestProvider.Get<List<CustomersOrderModel>>(GetCustomerOrdersKey);
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // Lista (cabeçalhos, sem linhas) — GET api/customer-orders (nova API).
        public async Task<List<CustomersOrderModel>> GetCustomerOrders()
        {
            try
            {
                var response = await _requestProvider.Get<List<CustomerOrderSummaryResponse>>(CustomerOrdersReadKey);
                if (response == null)
                {
                    return null;
                }
                return response.Select(MapSummary).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // Detalhe (linhas de uma encomenda) — GET api/customer-orders/{phcOrderId}.
        public async Task<List<OrderItem>> GetCustomerOrderDetail(string phcOrderId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phcOrderId))
                {
                    return new List<OrderItem>();
                }

                var endpoint = string.Format(CustomerOrderDetailKey, Uri.EscapeDataString(phcOrderId));
                var response = await _requestProvider.Get<CustomerOrderDetailResponse>(endpoint);
                if (response?.items == null)
                {
                    return new List<OrderItem>();
                }
                return response.items.Select(MapItem).ToList();
            }
            catch (Exception ex)
            {
                return new List<OrderItem>();
            }
        }

        // Mapeia o DTO plano para o modelo existente (campos de separação ficam no default).
        // dateCustomer recebe orderDate (é o campo que o XAML da lista mostra).
        private static CustomersOrderModel MapSummary(CustomerOrderSummaryResponse r) => new CustomersOrderModel
        {
            phcOrderId = r.phcOrderId,
            customerName = r.customerName,
            orderDate = r.orderDate ?? default,
            dateCustomer = r.orderDate ?? default,
            deliveryDate = r.deliveryDate ?? default,
            status = r.status,
        };

        private static OrderItem MapItem(CustomerOrderItemResponse i) => new OrderItem
        {
            phcOrderItemLinId = i.phcOrderItemLineId,
            productRef = i.productRef,
            productDescription = i.productDescription,
            eanCode = i.eanCode,
            quantity = i.quantity,
        };
    }
}
