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
    public class FornecedoresCaiaca : IFornecedoresCaiaca
    {

        FornecedoresCaiacaRepository _repo;

        public FornecedoresCaiaca()
        {

        }

        [PrimaryKey]
        public string Id { get; set; }

        [MaxLength(25)]
        public string FlStamp { get; set; }

        public int Num { get; set; }

        [MaxLength(60)]
        public string Nome { get; set; }

        [MaxLength(60)]
        public string Nome2 { get; set; }

        public bool Use2CodBar { get; set; }
        public int TipoMP { get; set; }  // Papel / Colas Verniz / Diversos


        public int NCodBarInfo { get; set; }
        public int NCodBarBobina { get; set; }
        public int PosInicio { get; set; }

        [MaxLength(60)]
        public string P1Tipo { get; set; }
        public int P1Val { get; set; }

        [MaxLength(60)]
        public string P2Tipo { get; set; }
        public int P2Val { get; set; }

        [MaxLength(60)]
        public string P3Tipo { get; set; }
        public int P3Val { get; set; }

        [MaxLength(60)]
        public string P4Tipo { get; set; }
        public int P4Val { get; set; }

        [MaxLength(60)]
        public string P5Tipo { get; set; }
        public int P5Val { get; set; }


        public bool Fechado { get; set; }
        public bool Anulado { get; set; }


        public static readonly int Papel = 1;
        public static readonly int ColasTintasVern = 2;
        public static readonly int Diversos = 3;



        public static string InfoGramagem = "3 - Gramagem";
        public static string InfoDiametro = "4 - Diametro";
        public static string InfoPeso = "2 - Peso";
        public static string InfoComprimento = "5 - Comprimento";


        /// <summary>
        /// Methods
        /// </summary>
        /// <param name="entities"></param>
        /// <exception cref="System.NotImplementedException"></exception>

        public void AddToLocalDb(IEnumerable<FornecedoresCaiaca> entities)
        {
            _repo = new FornecedoresCaiacaRepository();
            _repo.AddToLocalDb(entities);
        }

        public void AddToLocalDb(FornecedoresCaiaca entity)
        {
            _repo = new FornecedoresCaiacaRepository();
            _repo.AddToLocalDb(entity);
        }

        public IEnumerable<FornecedoresCaiaca> GetAllLocalData()
        {
            _repo = new FornecedoresCaiacaRepository();
            return _repo.GetAllLocalData();
        }

        public void ResetLocalDb()
        {
            _repo = new FornecedoresCaiacaRepository();
            _repo.ResetLocalDb();
        }

        public void UpdateLocalDb()
        {
            _repo = new FornecedoresCaiacaRepository();
            _repo.UpdateLocalDb();

        }
    }
}
