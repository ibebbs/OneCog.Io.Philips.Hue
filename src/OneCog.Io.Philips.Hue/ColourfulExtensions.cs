using Colourful;
using Colourful.Conversion;
using System;

namespace OneCog.Io.Philips.Hue
{
    public static class ColourfulExtensions
    {
        private static readonly ColourfulConverter Converter = new ColourfulConverter();

        public static Dto.LightState ToLightState(this ColourfulConverter converter, IColorVector color)
        {
            var xy = converter.ToxyY(color);

            return new Dto.LightState
            {
                Xy = new[] { xy.x, xy.y },
                Brightness = Convert.ToInt32(255.0 * xy.Luminance)
            };
        }

        public static Dto.LightState ToLightState(this IColorVector color)
        {
            var xy = Converter.ToxyY(color);

            return new Dto.LightState
            {
                Xy = new[] { xy.x, xy.y },
                Brightness = Convert.ToInt32(255.0 * xy.Luminance)
            };
        }
    }
}
