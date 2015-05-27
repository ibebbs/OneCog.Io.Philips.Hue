using Colourful;

namespace OneCog.Io.Philips.Hue
{
    public static class LightExtensions
    {
        public static Light.ISource AsLightSource(this Dto.Indexed<Dto.Light> light)
        {
            return new Light.Bulb
            {
                Id = (uint)light.Index,
                Color = new xyYColor(light.Item.State.Xy[0], light.Item.State.Xy[1], 255.0 / light.Item.State.Brightness)
            };
        }
    }
}
