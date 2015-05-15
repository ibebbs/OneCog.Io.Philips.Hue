using Newtonsoft.Json;

namespace OneCog.Io.Philips.Hue.Dto
{
    public class Group
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("lights")]
        public string[] Lights { get; set; }

        [JsonProperty("action")]
        public GroupState State { get; set; }
    }
}
