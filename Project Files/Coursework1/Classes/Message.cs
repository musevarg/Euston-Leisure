using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Coursework1.Classes
{
    [DataContract]
    public class Message
    {
        [DataMember(Name = "id", IsRequired = true, Order = 1)]
        public String ID;

        [DataMember(Name = "type", IsRequired = true, Order = 2)]
        public String Type;

        [DataMember(Name = "sender", IsRequired = true, Order = 3)]
        public String Sender;

        [DataMember(Name = "subject", IsRequired = false, EmitDefaultValue = false, Order = 4)]
        public String Subject;

        [DataMember(Name = "body", IsRequired = false, Order = 5)]
        public String Body;
    }
}
