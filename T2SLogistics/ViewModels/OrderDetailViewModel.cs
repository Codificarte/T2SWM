using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.ViewModels
{
    public class OrderDetailViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }

        public string Ref { get; set; }

        public string Description { get; set; }

        public bool UseSugestAlv { get; set; }


        private bool _useBatch;
        public bool UseBatch
        {
            get => _useBatch; set
            {
                _useBatch = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UseBatch)));
            }
        }


        private bool _isService;
        public bool IsService
        {
            get => _isService; set
            {
                _isService = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsService)));
            }
        }


        public bool NotUseBatch
        { get => UseBatch ? false : true; }

        public string IconUseBatch { get; set; }
        public string IconUseBatchColor { get => "Green"; }

        public string BatchId { get; set; }

        public DateTime BatchExp { get; set; }
        public string StrBatchExp
        {
            get => BatchExp.ToString("dd-MM-yyyy");
        }

        public string CodeReader { get; set; }

        public int Quanty { get; set; }


        private int _qttLido;
        public int QttLido
        {
            get => _qttLido; set
            {
                _qttLido = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QttLido)));
            }
        }



        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted; set
            {
                _isCompleted = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCompleted)));
            }
        }


        public bool Cativo { get; set; }

        private string _alveolo;
        public string Alveolo
        {
            get => _alveolo; set
            {
                _alveolo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Alveolo)));
            }
        }

        public string StampAlv { get; set; }

        public int OrderId { get; set; }

        public string IdBackOffice { get; set; }

        public string IdBackOfficeCab { get; set; }


        public bool IsButtonVisible { get; set; }

        //public Color ItemBgColor
        //{
        //    get => Id == 1 ? Color.WhiteSmoke : (Id == 2 ? Color.White : Id % 2 == 0 ? Color.White : Color.WhiteSmoke);
        //}
    }
}
