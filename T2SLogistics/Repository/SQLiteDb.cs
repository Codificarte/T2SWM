using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Interface;
using T2SLogistics.Repository;

[assembly: Dependency(typeof(SQLiteDb))]

namespace T2SLogistics.Repository
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            //var documentsPath = ApplicationData.Current.LocalFolder.Path;
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var path = Path.Combine(documentsPath, "T2SLogistics.db3");
            return new SQLiteAsyncConnection(path);
        }
    }
}

