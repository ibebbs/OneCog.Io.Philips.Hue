using Newtonsoft.Json;

namespace OneCog.Io.Philips.Hue.Dto
{
    public class Command
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("body")]
        public CommandState Body { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }
    }
}
