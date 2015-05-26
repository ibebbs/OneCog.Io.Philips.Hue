using Colourful;

namespace OneCog.Io.Philips.Hue.Light
{
    public interface ISource
    {
        uint Id { get; }
        IColorVector Color { get; }
    }
}
