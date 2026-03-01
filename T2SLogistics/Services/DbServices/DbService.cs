using T2SLogistics.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Services.DbServices
{
    public class DbService
    {
        public const string DB_Name = "T2SLogistics.db3";
        private readonly string dbPath = Path.Combine(FileSystem.AppDataDirectory, DB_Name);
        public const SQLite.SQLiteOpenFlags Flags =
       // open the database in read/write mode
       SQLite.SQLiteOpenFlags.ReadWrite |
       // create the database if it doesn't exist
       SQLite.SQLiteOpenFlags.Create |
       // enable multi-threaded database access
       SQLite.SQLiteOpenFlags.SharedCache;
        private readonly SQLiteAsyncConnection sQLiteAsyncConnection;
        public DbService()
        {
            sQLiteAsyncConnection = new SQLiteAsyncConnection(dbPath, Flags);
            sQLiteAsyncConnection.CreateTableAsync<OrderItemsSeprationLocalStorageModel>();

        }
     
        public async Task<List<T>> GetItemsAsync<T>() where T : new()
        {
           return await sQLiteAsyncConnection.Table<T>().ToListAsync();
        }

        public async Task<int> SaveItemAsync<T>(T item) where T : new()
        {
         
            return await sQLiteAsyncConnection.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync<T>(T item)
        {
           return await sQLiteAsyncConnection.DeleteAsync(item);

        }
    }
}

