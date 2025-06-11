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
    public class ArtigosCaiacaRepository : IArtigosCaiaca
    {
        private SQLiteAsyncConnection _connDb;

        public ArtigosCaiacaRepository()
        {
            _connDb = DependencyService.Get<ISQLiteDb>().GetConnection();
            _connDb.CreateTableAsync<ArtigosCaiaca>();
        }


        public async void AddToLocalDb(IEnumerable<ArtigosCaiaca> entities)
        {

            var _totalRowsAdded = 0;
            var _totalUpdated = 0;

            try
            {

                foreach (var item in entities)
                {

                    var _exist = Get(item.Id);
                    if (_exist == null)
                        _totalRowsAdded = Task.Run(async () => await _connDb.InsertAsync(item)).Result;
                    else
                        _totalUpdated = Task.Run(async () => await _connDb.UpdateAsync(item)).Result;

                }

                var _resut = _totalUpdated + _totalRowsAdded;
            }
            catch (Exception ex)
            {
                string _msg = ex.Message;
            }



        }

        public void AddToLocalDb(ArtigosCaiaca entity)
        {
            _connDb.InsertAsync(entity);
        }

        public void UpdateLocalArtigosRecepcao()
        {

            var _artigosInBd = GetAllLocalData().ToList();
            foreach (var item in _artigosInBd)
            {
                item.HasOpenEnc = false;

                var _result = Task.Run(async () => await _connDb.UpdateAsync(item)).Result;

            }

            var _artigosInBdSoActivos = GetAllLocalData().Where(a => a.HasOpenEnc == true).ToList();

        }


        public ArtigosCaiaca Get(string id)
        {
            try
            {
                var _ArtigosCaiaca = _connDb.Table<ArtigosCaiaca>().ToListAsync().Result.Where(s => s.Id == id).FirstOrDefault();

                return _ArtigosCaiaca;

            }
            catch (Exception)
            {
                return new ArtigosCaiaca();
            }
        }

        public IEnumerable<ArtigosCaiaca> GetAllLocalData()
        {
            try
            {
                var _list = _connDb.Table<ArtigosCaiaca>().ToListAsync().Result;

                if (_list == null)
                    return new List<ArtigosCaiaca>();

                return _list;

            }
            catch (Exception ex)
            {
                string _msg = ex.Message;
                return new List<ArtigosCaiaca>();
            }
        }

        public void ResetLocalDb()
        {
            _connDb.DropTableAsync<ArtigosCaiaca>();
        }

    }
}
