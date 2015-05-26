using Newtonsoft.Json;

namespace OneCog.Io.Philips.Hue.Dto
{
    [JsonConverter(typeof(StateChangeResponseConverter))]
    public class StateChangeResponse
    {
        public Change[] Changes { get; set; }
    }

    public class Change
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public bool Success { get; set; }
    }
}
