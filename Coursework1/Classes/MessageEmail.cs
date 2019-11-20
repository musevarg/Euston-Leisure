using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework1.Classes
{
    class MessageEmail
    {
        public String Type { get; private set; }
        public String ID { get; private set; }
        public String Sender { get; private set; }
        public String Subject { get; private set; }
        public String Body { get; private set; }

        public MessageEmail(string type, string iD, string sender, string subject, string body)
        {
            Type = type;
            ID = iD;
            Sender = sender;
            Subject = subject;
            Body = body;
        }
    }
}
