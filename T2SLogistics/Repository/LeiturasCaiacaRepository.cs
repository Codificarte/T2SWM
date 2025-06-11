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
    public class LeiturasCaiacaRepository : ILeiturasCaiaca
    {

        private SQLiteAsyncConnection _connDb;

        public LeiturasCaiacaRepository()
        {
            _connDb = DependencyService.Get<ISQLiteDb>().GetConnection();
            _connDb.CreateTableAsync<LeiturasCaiaca>();
        }

        public async void AddToLocalDb(IEnumerable<LeiturasCaiaca> entities)
        {

            var _totalRowsAdded = 0;
            var _totalUpdated = 0;

            foreach (var item in entities)
            {

                var _exist = Get(item.StampLeitura, item.Lote);

                if (_exist == null)
                    _totalRowsAdded = await _connDb.InsertAsync(item);
                else
                    _totalUpdated = await _connDb.UpdateAsync(item);

            }

            var _resut = _totalUpdated + _totalRowsAdded;

        }

        public void AddToLocalDb(LeiturasCaiaca entity)
        {
            throw new NotImplementedException();
        }

        public void ClearReadItemsSendSuccess(string _stampLeitura)
        {
            var _leituras = GetAllLocalData().Where(l => l.StampLeitura == _stampLeitura);
            foreach (var item in _leituras)
                item.IdStatus = LeiturasCaiaca.Sincronizado;

            AddToLocalDb(_leituras);
        }

        public LeiturasCaiaca Get(string stampId)
        {
            try
            {
                var _LeiturasCaiaca = _connDb.Table<LeiturasCaiaca>().ToListAsync().Result.Where(s => s.StampLeitura == stampId).FirstOrDefault();

                return _LeiturasCaiaca;

            }
            catch (Exception)
            {
                return new LeiturasCaiaca();
            }
        }

        public LeiturasCaiaca Get(string stampId, string lote)
        {
            try
            {
                var _LeiturasCaiaca = _connDb.Table<LeiturasCaiaca>().ToListAsync().Result.Where(s => s.StampLeitura == stampId && s.Lote == lote).FirstOrDefault();

                return _LeiturasCaiaca;

            }
            catch (Exception)
            {
                return new LeiturasCaiaca();
            }
        }

        public IEnumerable<LeiturasCaiaca> GetAllLocalData()
        {
            try
            {
                var _list = _connDb.Table<LeiturasCaiaca>().ToListAsync().Result;

                if (_list == null)
                    return new List<LeiturasCaiaca>();

                return _list;

            }
            catch (Exception ex)
            {
                string _msg = ex.Message;
                return new List<LeiturasCaiaca>();
            }
        }

        public void ResetLocalDb()
        {
            _connDb.DropTableAsync<LeiturasCaiaca>();
        }
    }
}
