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
    }
}
