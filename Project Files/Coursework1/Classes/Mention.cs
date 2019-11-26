using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework1
{
    class Mention
    {
        public String Id { get; private set; }
        public int Count { get; set; }

        public Mention(string id, int count)
        {
            this.Id = id;
            this.Count = count;
        }
    }
}
