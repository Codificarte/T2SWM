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
    public class FornecedoresCaiacaRepository : IFornecedoresCaiaca
    {

        private SQLiteAsyncConnection _connDb;

        public FornecedoresCaiacaRepository()
        {
            _connDb = DependencyService.Get<ISQLiteDb>().GetConnection();
            _connDb.CreateTableAsync<FornecedoresCaiaca>();
        }



        public async void AddToLocalDb(IEnumerable<FornecedoresCaiaca> entities)
        {

            var _totalRowsAdded = 0;
            var _totalUpdated = 0;

            var totalRowsInDb = GetAllLocalData().ToList();

            foreach (var item in entities)
            {

                var _result = Task.Run(async () => await _connDb.UpdateAsync(item)).Result;

                try
                {
                    AddToLocalDb(item);
                }
                catch (Exception ex)
                {
                    string _msg = ex.Message;
                }

            }

            var _fornecActivos = GetAllLocalData().Where(f => f.Anulado == false && f.Fechado == false).ToList();

            var _resut = _totalUpdated + _totalRowsAdded;

        }

        public void AddToLocalDb(FornecedoresCaiaca entity)
        {
            _connDb.InsertAsync(entity);
        }

        public void UpdateLocalDb()
        {

            var _fornecIDb = GetAllLocalData().Where(f => f.Anulado == false || f.Fechado == false);

            foreach (var f in _fornecIDb)
            {
                f.Anulado = true;
                f.Fechado = true;

                var _result = Task.Run(async () => await _connDb.UpdateAsync(f)).Result;
            }


            //var _soActivos = GetAllLocalData().Where(f => f.Anulado == false && f.Fechado == false).ToList();
            //var _todos = GetAllLocalData().ToList();

        }

        public FornecedoresCaiaca Get(string id)
        {
            try
            {
                var _FornecedoresCaiaca = _connDb.Table<FornecedoresCaiaca>().ToListAsync().Result.Where(s => s.Id == id).FirstOrDefault();

                return _FornecedoresCaiaca;

            }
            catch (Exception)
            {
                return new FornecedoresCaiaca();
            }
        }

        public IEnumerable<FornecedoresCaiaca> GetAllLocalData()
        {
            try
            {
                var _list = _connDb.Table<FornecedoresCaiaca>().ToListAsync().Result;

                if (_list == null)
                    return new List<FornecedoresCaiaca>();

                return _list;

            }
            catch (Exception ex)
            {
                string _msg = ex.Message;
                return new List<FornecedoresCaiaca>();
            }
        }

        public void ResetLocalDb()
        {
            _connDb.DropTableAsync<FornecedoresCaiaca>();
        }

    }
}
