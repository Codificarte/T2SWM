using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Interface
{
    public interface IRecepcaoMercadoria<TEntity> where TEntity : class
    {
        TEntity Add(TEntity entity);

    }

    public interface IExpedicaoMercadoria<TEntity> where TEntity : class
    {
        TEntity Add(TEntity entity);
    }
}
