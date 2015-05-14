using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue.Dto
{
    public class User
    {
        public User(string name, string deviceType)
        {
            Name = name;
            DeviceType = deviceType;
        }

        [JsonProperty("username")]
        public string Name { get; private set; }

        [JsonProperty("devicetype")]
        public string DeviceType { get; private set; }
    }
}
