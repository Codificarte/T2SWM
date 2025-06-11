using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Dtos;
using T2SLogistics.Models;
using T2SLogistics.Services;

namespace T2SLogistics.ViewModels.Recepcao
{
    public class RecepcaoCaiacaHomeViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public RecepcaoCaiacaHomeViewModel()
        {

            BtnPapelBackColor = BtnBackColorNotSelected;
            BtnPapelTextColor = BtnTextColorNotSelected;

            BtnCTVBackColor = BtnBackColorNotSelected;
            BtnCTVTextColor = BtnTextColorNotSelected;

            BtnDiversosBackColor = BtnBackColorNotSelected;
            BtnDiversosTextColor = BtnTextColorNotSelected;

        }


        public string Nome { get; set; }
        public int Num { get; set; }
        public int TipoMP { get; set; } //Tipo Matéria Prima
        public bool Usa2CodBar { get; set; }

        public ObservableCollection<FornecedoresCaiaca> Fornecedores { get; set; }



        public string ItemSelected { get => "\ue039"; }
        public string BtnBackColorNotSelected { get => "white"; }
        public string BtnTextColorNotSelected { get => "DarkBlue"; }

        public string BtnBackColorSelected { get => "darkGreen"; }
        public string BtnTextColorSelected { get => "white"; }


        // Botão Papel
        private string _btnPapelBackColor;
        public string BtnPapelBackColor
        {
            get => _btnPapelBackColor; set
            {
                _btnPapelBackColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BtnPapelBackColor)));
            }
        }

        private string _btnPapelTextColor;
        public string BtnPapelTextColor
        {
            get => _btnPapelTextColor; set
            {
                _btnPapelTextColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BtnPapelTextColor)));
            }
        }

        // Botão Colas Tintas e Vernizes 
        private string _btnCTVBackColor;
        public string BtnCTVBackColor
        {
            get => _btnCTVBackColor; set
            {
                _btnCTVBackColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BtnCTVBackColor)));
            }
        }

        private string _btnCTVTextColor;
        public string BtnCTVTextColor
        {
            get => _btnCTVTextColor; set
            {
                _btnCTVTextColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BtnCTVTextColor)));
            }
        }

        // Botão Diversos
        private string _btnDiversosBackColor;
        public string BtnDiversosBackColor
        {
            get => _btnDiversosBackColor; set
            {
                _btnDiversosBackColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BtnDiversosBackColor)));
            }
        }

        private string _btnDiversosTextColor;
        public string BtnDiversosTextColor
        {
            get => _btnDiversosTextColor; set
            {
                _btnDiversosTextColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BtnDiversosTextColor)));
            }
        }


        //Texto p/ Tipo de Fornecedor
        private string _textTipoFornecedores;
        public string TextTipoFornecedores
        {
            get => _textTipoFornecedores; set
            {
                _textTipoFornecedores = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextTipoFornecedores)));
            }
        }

        /// <summary>
        /// Methods
        /// </summary>
        /// <returns></returns>


        public async Task<ObservableCollection<FornecedoresCaiaca>> GetFornecedoresFromApi()
        {
            try
            {

                var _api = new CaiacaApi();
                var _fornecTask = _api.GetRemoteFornecedoresAsync();

                var _fornecedoresDto = await _fornecTask;

                var _fornecedores = new List<FornecedoresCaiaca>();
                var _listObservableColFornec = new ObservableCollection<FornecedoresCaiaca>();

                if (_fornecedoresDto.Count() > 0)
                {
                    _listObservableColFornec = GetFornecedorObservableCol(_fornecedoresDto.ToList());
                    _fornecedores = _listObservableColFornec.ToList();

                    AddToLocalDb(_fornecedores);

                    AddArtigosToLocalDb(_fornecedoresDto);

                }

                return _listObservableColFornec;
            }
            catch (Exception)
            {
                return new ObservableCollection<FornecedoresCaiaca>();
            }

        }

        private ObservableCollection<FornecedoresCaiaca> GetFornecedorObservableCol(List<FornecedoresCaiacaDto> _fornecedoresDto)
        {

            var _fornecedores = new ObservableCollection<FornecedoresCaiaca>();

            foreach (var f in _fornecedoresDto)
            {
                _fornecedores.Add(new FornecedoresCaiaca
                {
                    Id = f.Id,
                    NCodBarBobina = f.NCodBarBobina,
                    NCodBarInfo = f.NCodBarInfo,
                    Nome = f.Nome,
                    Nome2 = f.Nome2,
                    Num = f.Num,
                    P1Tipo = f.P1Tipo,
                    P1Val = f.P1Val,
                    P2Tipo = f.P2Tipo,
                    P2Val = f.P2Val,
                    P3Tipo = f.P3Tipo,
                    P3Val = f.P3Val,
                    P4Tipo = f.P4Tipo,
                    P4Val = f.P4Val,
                    P5Tipo = f.P5Tipo,
                    P5Val = f.P5Val,
                    PosInicio = f.PosInicio,
                    TipoMP = f.TipoMP,
                    Use2CodBar = f.Use2CodBar,
                    Fechado = f.Fechado,
                    Anulado = f.Anulado,
                });

            }

            return _fornecedores;
        }

        private void AddArtigosToLocalDb(IEnumerable<FornecedoresCaiacaDto> _fornecs)
        {


            var _artigos = new ArtigosCaiaca();

            foreach (var l in _fornecs)
                _artigos.AddToLocalDb(l.Artigos);

        }

        private void AddToLocalDb(IEnumerable<FornecedoresCaiaca> fornec)
        {

            if (fornec == null)
                return;

            if (fornec.Count() <= 0)
                return;


            var f = new FornecedoresCaiaca();
            f.AddToLocalDb(fornec);

        }

        public ObservableCollection<FornecedoresCaiaca> LoadFornecedores(int tipoMP)
        {

            var fornec = new FornecedoresCaiaca();

            var _list = fornec.GetAllLocalData().Where(i => i.TipoMP == tipoMP && i.Anulado == false && i.Fechado == false);
            var _fornecedores = new ObservableCollection<FornecedoresCaiaca>();

            foreach (var f in _list)
            {
                _fornecedores.Add(new FornecedoresCaiaca
                {
                    Id = f.Id,
                    NCodBarBobina = f.NCodBarBobina,
                    NCodBarInfo = f.NCodBarInfo,
                    Nome = f.Nome,
                    Nome2 = f.Nome2,
                    Num = f.Num,
                    P1Tipo = f.P1Tipo,
                    P1Val = f.P1Val,
                    P2Tipo = f.P2Tipo,
                    P2Val = f.P2Val,
                    P3Tipo = f.P3Tipo,
                    P3Val = f.P3Val,
                    P4Tipo = f.P4Tipo,
                    P4Val = f.P4Val,
                    P5Tipo = f.P5Tipo,
                    P5Val = f.P5Val,
                    PosInicio = f.PosInicio,
                    TipoMP = f.TipoMP,
                    Use2CodBar = f.Use2CodBar
                });

            }

            return _fornecedores;

        }
    }
}
