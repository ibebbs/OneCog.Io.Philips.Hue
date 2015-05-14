using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue.Dto
{
    public interface IState
    {

    }

    public class State : IState
    {
        public static State FromJson(string json)
        {
            return Serializer.Json.Deserialize<State>(json);
        }
    }
}
