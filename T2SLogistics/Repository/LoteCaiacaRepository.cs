using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Interface;
using T2SLogistics.Models;

namespace T2SLogistics.Repository
{
    public class LoteCaiacaRepository : ILotesCaiaca
    {

        private SQLiteAsyncConnection _connDb;

        public LoteCaiacaRepository()
        {
            _connDb = DependencyService.Get<ISQLiteDb>().GetConnection();
            _connDb.CreateTableAsync<LoteCaiaca>();
        }

        public async void AddToLocalDb(IEnumerable<LoteCaiaca> entities)
        {
            //var _totalRowsPosDelete = 0;
            //var _totalRowsAfterDel = 0;
            var _totalRowsAdded = 0;
            var _totalUpdated = 0;

            //var _resultDel = await _connDb.DeleteAllAsync<LoteCaiaca>();

            //_totalRowsAfterDel = _resultDel;

            foreach (var item in entities)
            {

                var _exist = Get(item.SeStamp);

                if (_exist == null)
                    _totalRowsAdded = await _connDb.InsertAsync(item);
                else
                    _totalUpdated = await _connDb.UpdateAsync(item);

            }

            //_totalRowsAfterDel = GetAllLocalData().Count();
            //var _resut = _totalUpdated + _totalRowsAdded;
        }

        public void AddToLocalDb(LoteCaiaca entity)
        {
            throw new NotImplementedException();
        }

        public LoteCaiaca Get(string stampId)
        {
            try
            {
                var _LoteCaiaca = _connDb.Table<LoteCaiaca>().ToListAsync().Result.Where(s => s.SeStamp == stampId).FirstOrDefault();

                return _LoteCaiaca;

            }
            catch (Exception)
            {
                return new LoteCaiaca();
            }
        }

        public IEnumerable<LoteCaiaca> GetAllLocalData()
        {
            try
            {
                var _list = _connDb.Table<LoteCaiaca>().ToListAsync().Result;

                if (_list == null)
                    return new List<LoteCaiaca>();

                return _list;

            }
            catch (Exception ex)
            {
                string _msg = ex.Message;
                return new List<LoteCaiaca>();
            }
        }

        public void ResetLocalDb()
        {
            _connDb.DropTableAsync<LoteCaiaca>();
        }
    }
}
