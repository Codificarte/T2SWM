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
    public class ArtigosCaiaca : IArtigosCaiaca
    {

        ArtigosCaiacaRepository _repo;

        [PrimaryKey]
        public string Id { get; set; }

        [MaxLength(18)]
        public string Ref { get; set; }

        [MaxLength(60)]
        public string Design { get; set; }

        [MaxLength(20)]
        public string DesignLcb { get; set; }

        public bool Usalote { get; set; }

        [MaxLength(10)]
        public string UnidLcb { get; set; }

        [MaxLength(10)]
        public string Gramagem { get; set; }

        [MaxLength(10)]
        public string Largura { get; set; }

        [MaxLength(10)]
        public string TipoPapel { get; set; }

        [MaxLength(10)]
        public string TipoMP { get; set; }

        public int NumFornec { get; set; }

        [MaxLength(60)]
        public string NomeFornec { get; set; }

        public bool HasOpenEnc { get; set; }


        public static readonly string IsUnid = "UN";
        public static readonly string IsKg = "KG";
        public static readonly string IsTon = "TON";



        /// <summary>
        /// Methods
        /// </summary>
        /// <param name="entities"></param>
        /// <exception cref="System.NotImplementedException"></exception>



        public void AddToLocalDb(IEnumerable<ArtigosCaiaca> entities)
        {
            _repo = new ArtigosCaiacaRepository();
            _repo.AddToLocalDb(entities);
        }

        public void AddToLocalDb(ArtigosCaiaca entity)
        {
            _repo = new ArtigosCaiacaRepository();
            _repo.AddToLocalDb(entity);

        }

        public IEnumerable<ArtigosCaiaca> GetAllLocalData()
        {
            _repo = new ArtigosCaiacaRepository();
            return _repo.GetAllLocalData();
        }

        public void ResetLocalDb()
        {
            _repo = new ArtigosCaiacaRepository();
            _repo.ResetLocalDb();
        }

        public void UpdateLocalArtigosRecepcao()
        {

            _repo = new ArtigosCaiacaRepository();
            _repo.UpdateLocalArtigosRecepcao();

        }
    }
}
