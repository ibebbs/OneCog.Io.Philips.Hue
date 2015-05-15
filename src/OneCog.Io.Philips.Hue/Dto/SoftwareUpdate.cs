using Newtonsoft.Json;

namespace OneCog.Io.Philips.Hue.Dto
{
    public class SoftwareUpdate
    {
        [JsonProperty("updatestate")]
        public int UpdateState { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("notify")]
        public bool Notify { get; set; }
    }
}
