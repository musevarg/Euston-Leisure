using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework1.Classes
{
    class Hashtag
    {
        public String Tag { get; private set; }
        public int Count { get; set; }

        public Hashtag(string tag, int count)
        {
            this.Tag = tag;
            this.Count = count;
        }
    }
}
