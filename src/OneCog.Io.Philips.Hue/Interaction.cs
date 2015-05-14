using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue
{
    public interface IInteraction
    {
        Task Completion { get; }
        CancellationToken Cancel { get; }
    }

    public class Interaction
    {
    }
}
