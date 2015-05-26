using Colourful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue.Light
{
    public class Bulb : ISource
    {
        public uint Id { get; set; }
        public IColorVector Color { get; set; }
    }
}
