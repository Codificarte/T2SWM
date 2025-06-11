using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Models;

namespace T2SLogistics.Interface
{
    public interface IRecepcaoCaiaca2CodBar
    {
        ObservableCollection<TipoPapelCaiaca> LoadTiposPapel();
        bool IsValid(LeiturasCaiaca leitura);
    }
}
