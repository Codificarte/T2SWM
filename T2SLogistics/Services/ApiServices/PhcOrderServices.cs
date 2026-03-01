using T2SLogistics.Model;
using T2SLogistics.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Services.ApiServices
{
    public class PhcOrderServices : ApiBase
    {
        IRequestProvider _requestProvider;

        public PhcOrderServices(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }
        public async Task<List<PhcOrderModel>> GetPhcOrders()
        {
            try
            {
                var response = await _requestProvider.Get<List<PhcOrderModel>>(PhcOrdersKey);
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<PhcOrderModel>> GetPhcOrdersByUserCode(string userCode)
        {
            try
            {
                string endpoint = String.Format(PhcOrdersByCodeKey, userCode);
                var response = await _requestProvider.Get<List<PhcOrderModel>>(endpoint);
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> AddSeprationItems(List<AddSeprationItemsRequestModel> addSeprationItemsRequestModel)
        {
            try
            {
                var jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(addSeprationItemsRequestModel);

                var response = await _requestProvider.PostAsync<bool>(AddSeprationItemsKey, jsonObject);
                return response;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> AddSeprationItem(AddSeprationItemRequestModel addSeprationItemRequestModel)
        {
            try
            {
                var jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(addSeprationItemRequestModel);

                var response = await _requestProvider.PostAsync<bool>(AddSeprationItemKey, jsonObject);
                return response;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<AddExpeditionReadItemResponseModel> AddExpeditionReadItems(AddExpeditionRequestModel addExpeditionRequestModel)
        {
            try
            {
                var jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(addExpeditionRequestModel);

                var response = await _requestProvider.Post<AddExpeditionReadItemResponseModel>(AddExpeditionKey, jsonObject);
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> DeleteExpeditionReadItems(int expeditionId)
        {
            try
            {
                string endpoint = String.Format(DeleteExpeditionKey, expeditionId);
                var response = await _requestProvider.DeleteAsync(endpoint);
                return response;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<RefProductResponseModel> GetRefProduct(string refEan)
        {
            try
            {
                string endpoint = String.Format(GetRefProductKey, refEan);
                var response = await _requestProvider.Get<RefProductResponseModel>(endpoint);
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<int> CloseExpedition(CloseExpeditionRequestModel closeExpeditionRequestModel)
        {
            try
            {
                var jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(closeExpeditionRequestModel);

                var response = await _requestProvider.Post<int>(CloseExpeditionKey, jsonObject);
                return response;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public async Task<bool> CancelReadExpedition(string itemReadId)
        {
            try
            {
                string endpoint = String.Format(CancelReadExpeditionKey, itemReadId);
                var response = await _requestProvider.PostAsync<bool>(endpoint, string.Empty);
                return response;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
