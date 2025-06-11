using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Models;

namespace T2SLogistics.Interface
{
    public interface IRecepcaoCaiaca1CodBar
    {

        ObservableCollection<TipoPapelCaiaca> LoadTiposPapel();
        ObservableCollection<LarguraPapelCaiaca> LoadLarguraPapel(string tipoPapel);

        bool IsValid(LeiturasCaiaca leitura);
    }
}
