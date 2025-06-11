using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Dtos;

namespace T2SLogistics.Models
{
    public abstract class RecepcaoMercadoriaCaiaca
    {


        public RecepcaoMercadoriaCaiaca()
        {

        }

        public string StampLeitura { get; set; }
        public DateTime DataMov { get; set; }

        public int NumDocInServer { get; set; }


        public abstract List<LeiturasCaiacaDto> AddOrCloseLeituras(object rec);


        public virtual ObservableCollection<ArtigosCaiaca> GetArtigosFornec(int numFornec)
        {

            ArtigosCaiaca _ref = new ArtigosCaiaca();
            var _listTmp = _ref.GetAllLocalData().Where(r => r.NumFornec == numFornec).ToList();

            var _list = _ref.GetAllLocalData().Where(r => r.NumFornec == numFornec && r.HasOpenEnc == true).ToList();

            var _artigos = new ObservableCollection<ArtigosCaiaca>();

            foreach (var item in _list)
            {
                _artigos.Add(new ArtigosCaiaca
                {
                    Design = item.Design,
                    DesignLcb = item.DesignLcb,
                    Gramagem = item.Gramagem,
                    Id = item.Id,
                    Largura = item.Largura,
                    Ref = item.Ref,
                    NomeFornec = item.NomeFornec,
                    NumFornec = item.NumFornec,
                    TipoMP = item.TipoMP,
                    TipoPapel = item.TipoPapel,
                    UnidLcb = item.UnidLcb,
                    Usalote = item.Usalote,
                    HasOpenEnc = item.HasOpenEnc
                });
            };

            return _artigos;

        }

        public virtual ObservableCollection<ArtigosCaiaca> GetArtigosFornec(int numFornec, string tipoPapel, string larguraPapel)
        {

            ArtigosCaiaca _ref = new ArtigosCaiaca();
            var _list = _ref.GetAllLocalData().Where(r => r.NumFornec == numFornec && r.TipoPapel == tipoPapel && r.Largura == larguraPapel);

            var _artigos = new ObservableCollection<ArtigosCaiaca>();

            foreach (var item in _list)
            {
                _artigos.Add(new ArtigosCaiaca
                {
                    Design = item.Design,
                    DesignLcb = item.DesignLcb,
                    Gramagem = item.Gramagem,
                    Id = item.Id,
                    Largura = item.Largura,
                    Ref = item.Ref,
                    NomeFornec = item.NomeFornec,
                    NumFornec = item.NumFornec,
                    TipoMP = item.TipoMP,
                    TipoPapel = item.TipoPapel,
                    UnidLcb = item.UnidLcb,
                    Usalote = item.Usalote
                });
            };

            return _artigos;

        }

    }
}
