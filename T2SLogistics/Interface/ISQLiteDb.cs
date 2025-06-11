using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Interface
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}
