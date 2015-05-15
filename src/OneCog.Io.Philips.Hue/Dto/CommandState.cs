using Newtonsoft.Json;

namespace OneCog.Io.Philips.Hue.Dto
{
    public class CommandState
    {
        [JsonProperty("on")]
        public bool On { get; set; }
    }
}
