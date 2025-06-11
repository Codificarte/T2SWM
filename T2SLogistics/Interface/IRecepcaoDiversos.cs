using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Models;

namespace T2SLogistics.Interface
{
    public interface IRecepcaoDiversos
    {
        bool IsValid(LeiturasCaiaca leitura);
    }
}
