using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Dtos;
using T2SLogistics.Models;
using T2SLogistics.ViewModels.Expedicao;
using T2SLogistics.ViewModels.Recepcao;

namespace T2SLogistics.Services
{
    public class CaiacaApi
    {

        HttpClient clientApi = new HttpClient();

        readonly string urlSaidas = Helpers.Settings.UrlApiRegSaidas;
        readonly string urlEntradas = Helpers.Settings.UrlApiRegEntradas;



        public List<LeiturasCaiacaDto> AddOrCloseSaidas(ExpedicaoCaiacaViewModel exp)
        {

            var _url = urlSaidas + "AddExpedicaoBulkCaiaca";

            try
            {

                string objSerialized = JsonConvert.SerializeObject(exp.ItemsRead);
                HttpContent content = new StringContent(objSerialized, Encoding.UTF8, "application/json");


                var response = clientApi.PostAsync(_url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;

                    List<LeiturasCaiacaDto> _result;
                    _result = JsonConvert.DeserializeObject<List<LeiturasCaiacaDto>>(responseString);

                    return _result;
                }


                return new List<LeiturasCaiacaDto>();
            }
            catch (Exception)
            {
                return new List<LeiturasCaiacaDto>();
            }


        }


        public List<LeiturasCaiacaDto> AddOrCloseEntradas(RecCaiaca1CodBarViewModel rec)
        {

            var _url = urlEntradas + "AddRecepcaoBulkCaiaca";
            try
            {

                string objSerialized = JsonConvert.SerializeObject(rec.ItemsRead);
                HttpContent content = new StringContent(objSerialized, Encoding.UTF8, "application/json");


                var response = clientApi.PostAsync(_url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;

                    List<LeiturasCaiacaDto> _result;
                    _result = JsonConvert.DeserializeObject<List<LeiturasCaiacaDto>>(responseString);

                    return _result;
                }


                return new List<LeiturasCaiacaDto>();
            }
            catch (Exception)
            {
                return new List<LeiturasCaiacaDto>();
            }

        }





        public async Task<IEnumerable<FornecedoresCaiacaDto>> GetRemoteFornecedoresAsync()
        {
            var _listFornec = new List<FornecedoresCaiacaDto>();
            var _url = urlEntradas + "GetFornecedoresCaiaca";

            try
            {

                string resp = await clientApi.GetStringAsync(_url);

                _listFornec = JsonConvert.DeserializeObject<List<FornecedoresCaiacaDto>>(resp);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _listFornec;
        }



        public async Task<IEnumerable<LoteCaiaca>> GetRemoteLotesAsync()
        {
            var _listLotes = new List<LoteCaiaca>();
            var _url = urlSaidas + "GetLotesExpedicao";

            try
            {

                string resp = await clientApi.GetStringAsync(_url);

                _listLotes = JsonConvert.DeserializeObject<List<LoteCaiaca>>(resp);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _listLotes;
        }

    }
}
