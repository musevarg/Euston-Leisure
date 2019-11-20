using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework1
{
    class TwitterID
    {
        public String id { get; private set; }
        public int count { get; set; }

        public TwitterID(string id, int count)
        {
            this.id = id;
            this.count = count;
        }
    }
}
