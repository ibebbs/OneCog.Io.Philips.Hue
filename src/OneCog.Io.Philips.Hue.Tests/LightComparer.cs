using Colourful.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue.Tests
{
    public class LightComparer : IEqualityComparer<Light.ISource>
    {
        public static readonly IEqualityComparer<Light.ISource> Default = new LightComparer();

        private static readonly ColourfulConverter Converter = new ColourfulConverter();

        public bool Equals(Light.ISource x, Light.ISource y)
        {
            if (x.GetType().Equals(y.GetType()) && x.Id == y.Id)
            {
                var xColor = Converter.ToRGB(x.Color);
                var yColor = Converter.ToRGB(y.Color);

                return xColor.Equals(yColor);
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(Light.ISource obj)
        {
            var color = Converter.ToRGB(obj.Color);

            return obj.GetType().GetHashCode()
                ^ obj.Id.GetHashCode()
                ^ color.GetHashCode();
        }
    }
}
