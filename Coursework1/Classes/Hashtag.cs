using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework1.Classes
{
    class Hashtag
    {
        public String tag { get; private set; }
        public int count { get; set; }

        public Hashtag(string tag, int count)
        {
            this.tag = tag;
            this.count = count;
        }
    }
}
