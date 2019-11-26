using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework1.Classes
{
    class SIR
    {
        public string Date { get; private set; }
        public string CentreCode { get; private set; }
        public string Incident { get; private set; }

        public SIR(string date, string centreCode, string incident)
        {
            Date = date;
            CentreCode = centreCode;
            Incident = incident;
        }
    }
}
