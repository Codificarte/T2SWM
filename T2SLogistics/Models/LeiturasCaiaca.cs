using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Interface;
using T2SLogistics.Repository;

namespace T2SLogistics.Models
{
    public class LeiturasCaiaca : ILeiturasCaiaca
    {
        LeiturasCaiacaRepository _repo;

        public LeiturasCaiaca()
        {
            _repo = new LeiturasCaiacaRepository();
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string SeStamp { get; set; }

        [MaxLength(18)]
        public string Ref { get; set; }

        [MaxLength(18)]
        public string DesignLcb { get; set; }

        public bool UsaLote { get; set; }

        [MaxLength(50)]
        public string Lote { get; set; }

        [MaxLength(60)]
        public string Design { get; set; }

        public bool Inactivo { get; set; }
        public decimal Stock { get; set; }

        public decimal Peso { get; set; }

        public string InfoBobiba2CodBar { get; set; }

        [MaxLength(10)]
        public string Unidade { get; set; }

        [MaxLength(60)]
        public string NomeEntidade { get; set; } // Cliente ou Fornecedor

        public int NumEntidade { get; set; }    // Cliente ou Fornecedor

        [MaxLength(100)]
        public string StampLeitura { get; set; }

        public int NrGuiaOnServer { get; set; }

        public DateTime DataLeitura { get; set; }
        public string UserLeitura { get; set; }

        public int TipoMov { get; set; }
        public int IdStatus { get; set; }

        public int TipoMP { get; set; }


        public static readonly int Pendente = 1;
        public static readonly int Sincronizado = 2;

        public static readonly int Entradas = 1;
        public static readonly int Saidas = 2;

        public void ClearReadItemsSendSuccess(string _stampLeitura)
        {
            _repo.ClearReadItemsSendSuccess(_stampLeitura);
        }

        public void AddToLocalDb(IEnumerable<LeiturasCaiaca> entities)
        {
            _repo.AddToLocalDb(entities);
        }

        public void AddToLocalDb(LeiturasCaiaca entity)
        {
            _repo.AddToLocalDb(entity);
        }

        public IEnumerable<LeiturasCaiaca> GetAllLocalData()
        {
            return _repo.GetAllLocalData();
        }

        public void ResetLocalDb()
        {
            _repo.ResetLocalDb();
        }

        public LeiturasCaiaca Get(string stampId, string lote)
        {
            return _repo.Get(stampId, lote);
        }

        LeiturasCaiaca ILeiturasCaiaca.Get(string stampId, string lote)
        {
            throw new NotImplementedException();
        }

        IEnumerable<LeiturasCaiaca> IRepository<LeiturasCaiaca>.GetAllLocalData()
        {
            throw new NotImplementedException();
        }
    }
}
