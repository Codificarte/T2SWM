using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Exceptions;
using T2SLogistics.Helpers;

namespace T2SLogistics.ViewModels
{
    public class LeituraExpedicao
    {


        public OrderDetailViewModel IsValid(List<ArtigosViewModel> _listArtigos, OrderViewModel _orderVM)
        {

            if (_listArtigos.Count() > 1)
                throw new LeituraException("Código associado a mais do que uma referência!", UtilsForMessage.TitleException);


            if (_listArtigos.Count() == 0)
                throw new LeituraException("Código desconhecido!", UtilsForMessage.TitleException);


            var _artigosVM = _listArtigos.ToList()[0];

            if (_artigosVM.Ref == null)
                throw new LeituraException("Código desconhecido!", UtilsForMessage.TitleException);

            var od = _orderVM.OrderDetail.Where(d => d.Ref.Trim().ToUpper() == _artigosVM.Ref.Trim().ToUpper()).ToList().FirstOrDefault();

            if (od == null)
                throw new LeituraException("O artigo não existe na encomenda!", UtilsForMessage.TitleException);

            if (od.Quanty < _orderVM.QttActual && _orderVM.QttActual != 0)
                throw new LeituraQttException("Quantidade diferente da encomenda!", UtilsForMessage.TitleException);

            return od;

        }


        public virtual LeiturasViewModel GetNewItem(OrderViewModel _orderVM, OrderDetailViewModel _orderDetailVM, string _fullCode)
        {

            var _leitura = new LeiturasViewModel();

            _leitura.Quanty = _orderVM.QttActual;
            _leitura.StampLeitura = _orderVM.StampLeitura;
            _leitura.FullCode = _fullCode.Trim();
            _leitura.ProductCode = _orderDetailVM.Ref.Trim();
            _leitura.OrderType = _orderVM.OrderType;
            _leitura.IdEntidade = _orderVM.IdEntidade;
            _leitura.UseBatch = _orderVM.UsarLote;
            _leitura.BatchId = _orderVM.LoteActual;
            _leitura.BatchExpire = _orderDetailVM.BatchExp;

            if (Helpers.Settings.UseAlveolos)
                _leitura.Alveolo = _orderDetailVM.Alveolo.Trim().ToUpper();

            return _leitura;

        }

    }
}
