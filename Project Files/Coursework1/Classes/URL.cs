using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework1.Classes
{
    class URL
    {
        public string Date { get; private set; }
        public string Sender { get; private set; }
        public string Url { get; private set; }

        public URL(string date, string sender, string url)
        {
            this.Date = date;
            this.Sender = sender;
            this.Url = url;
        }
    }
}
