using Newtonsoft.Json;

namespace OneCog.Io.Philips.Hue.Dto
{
    public class Whitelist
    {
        [JsonProperty("newdeveloper")]
        public Developer Developer { get; set; }
    }
}
