using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework1.Classes
{
    class SIR
    {
        public string Date { get; set; }
        public string CentreCode { get; set; }
        public string Incident { get; set; }

        public SIR(string date, string centreCode, string incident)
        {
            Date = date;
            CentreCode = centreCode;
            Incident = incident;
        }

        public override string ToString()
        {
            return this.Date + "\t" + this.CentreCode + "\t" + this.Incident;
        }
    }
}
