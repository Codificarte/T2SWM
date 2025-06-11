using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Exceptions;
using T2SLogistics.ViewModels;

namespace T2SLogistics.Helpers
{
    public abstract class LeituraValidator
    {

        public virtual void ItemReadValidator(ArtigosViewModel artigoVM)
        {

            if (artigoVM == null || string.IsNullOrEmpty(artigoVM.Ref))
                throw new LeituraException("Artigo não encontrado!", UtilsForMessage.TitleException);



        }

        public abstract LeiturasViewModel LeituraIsValid(List<ArtigosViewModel> _listArtigos, OrderViewModel _orderVM, LeiturasViewModel _leitura);

        public virtual LeiturasViewModel GetNewItem(OrderViewModel _orderVM)
        {

            var _leitura = new LeiturasViewModel();

            _leitura.Quanty = 1;
            _leitura.StampLeitura = _orderVM.StampLeitura;

            if (!string.IsNullOrEmpty(_orderVM.Armazem))
                _leitura.NumArmazem = Convert.ToInt32(_orderVM.Armazem.Substring(0, 1));

            _leitura.FullCode = _orderVM.LeituraActual.Trim();
            _leitura.OrderType = _orderVM.OrderType;
            _leitura.IdEntidade = _orderVM.IdEntidade;

            if (Settings.UseAlveolos && !Settings.ControlaAlvRec)
                _leitura.Alveolo = _orderVM.AlvActual.Trim().ToUpper();

            return _leitura;
        }

        public abstract void PrepareToSave();

    }
}
