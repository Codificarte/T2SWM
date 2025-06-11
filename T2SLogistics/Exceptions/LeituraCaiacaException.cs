using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Exceptions
{
    public class LeituraCaiacaException : Exception
    {
        public LeituraCaiacaException(string message, string header)
            : base(message)
        {
            Header = header;
        }

        public string Header { get; set; }
    }


    public class LeituraCaiacaRefException : Exception
    {

        public LeituraCaiacaRefException(string message, string header)
            : base(message)
        {
            Header = header;
        }

        public string Header { get; set; }

    }

    public class LeituraCaiacaLoteException : Exception
    {

        public LeituraCaiacaLoteException(string message, string header)
            : base(message)
        {
            Header = header;
        }

        public string Header { get; set; }

    }


    public class LeituraCaiacaQttException : Exception
    {

        public LeituraCaiacaQttException(string message, string header)
            : base(message)
        {
            Header = header;
        }

        public string Header { get; set; }

    }

    public class LeituraCaiacaPesoException : Exception
    {

        public LeituraCaiacaPesoException(string message, string header)
            : base(message)
        {
            Header = header;
        }

        public string Header { get; set; }

    }


    public class LeituraCaiacaLoteJaLidoException : Exception
    {

        public LeituraCaiacaLoteJaLidoException(string message, string header)
            : base(message)
        {
            Header = header;
        }

        public string Header { get; set; }

    }
}
