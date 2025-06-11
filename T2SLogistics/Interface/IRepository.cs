using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Interface
{
    public interface IRepository<TEntity> where TEntity : class
    {

        void AddToLocalDb(IEnumerable<TEntity> entities);
        void AddToLocalDb(TEntity entity);
        IEnumerable<TEntity> GetAllLocalData();
        void ResetLocalDb();

    }
}
