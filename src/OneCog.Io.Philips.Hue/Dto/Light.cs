using Newtonsoft.Json;

namespace OneCog.Io.Philips.Hue.Dto
{
    public class Light
    {
        [JsonProperty("state")]
        public LightState State { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("modelid")]
        public string ModelId { get; set; }

        [JsonProperty("swversion")]
        public string FirmwareVersion { get; set; }

        [JsonProperty("pointsymbol")]
        public PointSymbol pointsymbol { get; set; }
    }
}
