using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Exceptions
{
    public class LeituraException : Exception
    {

        public LeituraException(string message, string header)
            : base(message)
        {
            Header = header;
        }

        public string Header { get; set; }


    }

    public class LeituraQttException : Exception
    {

        public LeituraQttException(string message, string header)
            : base(message)
        {
            Header = header;
        }

        public string Header { get; set; }


    }

    public class LeituraAlveoloException : Exception
    {

        public LeituraAlveoloException(string message, string header)
            : base(message)
        {
            Header = header;
        }

        public string Header { get; set; }


    }

    public class LeituraLotesException : Exception
    {

        public LeituraLotesException(string message, string header)
            : base(message)
        {
            Header = header;
        }

        public string Header { get; set; }


    }


    public class LeituraRefException : Exception
    {

        public LeituraRefException(string message, string header)
            : base(message)
        {
            Header = header;
        }

        public string Header { get; set; }


    }
}
