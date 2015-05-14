using Colourful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue
{
    public interface ILight
    {
        uint Id { get; }
        IColorVector Color { get; }
    }

    public class Light : ILight
    {
        public uint Id { get; set; }
        public IColorVector Color { get; set; }
    }
}
