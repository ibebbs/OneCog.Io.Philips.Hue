using Colourful;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue.Tests
{
    public static class Helper
    {
        public static Light.ISource ConstructBulb(uint index, byte r, byte g, byte b)
        {
            return new Light.Bulb { Id = index, Color = new RGBColor(r, g, b) };
        }

        public static Light.ISource ConstructBulb(uint index, double x, double y, double c)
        {
            return new Light.Bulb { Id = index, Color = new Colourful.xyYColor(x, y, c) };
        }
    }
}
