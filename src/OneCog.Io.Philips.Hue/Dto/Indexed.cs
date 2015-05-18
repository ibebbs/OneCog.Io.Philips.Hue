using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue.Dto
{
    public class Indexed<T>
    {
        public int Index { get; set; }
        public T Item { get; set; }
    }
}
