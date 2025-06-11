using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Interface;
using T2SLogistics.Repository;

namespace T2SLogistics.Models
{
    public class LoteCaiaca : ILotesCaiaca
    {

        LoteCaiacaRepository _repo;

        [PrimaryKey]
        public string SeStamp { get; set; }

        [MaxLength(18)]
        public string Ref { get; set; }

        [MaxLength(50)]
        public string Lote { get; set; }

        [MaxLength(60)]
        public string Design { get; set; }

        public bool Inactivo { get; set; }
        public decimal Stock { get; set; }

        [MaxLength(10)]
        public string Unidade { get; set; }

        [MaxLength(60)]
        public string NomeCliente { get; set; }

        public int NumCliente { get; set; }

        public int TipoMov { get; set; }

        [MaxLength(100)]
        public string StampLeitura { get; set; }

        public static int Entradas = 1;
        public static int Saidas = 2;

        /// <summary>
        /// Methods
        /// </summary>
        /// <param name="entities"></param>

        public void AddToLocalDb(IEnumerable<LoteCaiaca> entities)
        {

            _repo = new LoteCaiacaRepository();
            _repo.AddToLocalDb(entities);

        }

        public void AddToLocalDb(LoteCaiaca entity)
        {
            _repo = new LoteCaiacaRepository();
            _repo.AddToLocalDb(entity);
        }

        public ObservableCollection<LoteCaiaca> GetAllLotes()
        {

            //var _listLotes = GetAllLocalData().Where(l => l.TipoMov == LoteCaiaca.Saidas);
            var _listLotes = GetAllLocalData();
            var _listReturn = new ObservableCollection<LoteCaiaca>();

            foreach (var l in _listLotes)
            {
                _listReturn.Add(new LoteCaiaca
                {
                    Design = l.Design,
                    SeStamp = l.SeStamp,
                    Inactivo = l.Inactivo,
                    Lote = l.Lote,
                    NomeCliente = l.NomeCliente,
                    NumCliente = l.NumCliente,
                    Ref = l.Ref,
                    Stock = l.Stock,
                    Unidade = l.Unidade
                });
            }

            return _listReturn;

        }

        public IEnumerable<LoteCaiaca> GetAllLocalData()
        {
            _repo = new LoteCaiacaRepository();
            var _list = _repo.GetAllLocalData();
            return _list;
        }

        public void ResetLocalDb()
        {
            _repo = new LoteCaiacaRepository();
            _repo.ResetLocalDb();
        }

        public LoteCaiaca Get(string stampId)
        {
            return _repo.Get(stampId);
        }

        LoteCaiaca ILotesCaiaca.Get(string stampId)
        {
            throw new NotImplementedException();
        }

        IEnumerable<LoteCaiaca> IRepository<LoteCaiaca>.GetAllLocalData()
        {
            throw new NotImplementedException();
        }
    }
}
