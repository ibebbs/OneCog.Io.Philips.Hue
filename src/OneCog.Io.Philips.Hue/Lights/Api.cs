using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue.Lights
{
    public interface IApi
    {
        Task Set(ILight light);
    }

    public class Api : IApi
    {

        public Task Set(ILight light)
        {
            throw new NotImplementedException();
        }
    }
}
