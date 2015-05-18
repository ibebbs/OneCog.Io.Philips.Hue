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

        [JsonProperty("lights")]
        [JsonConverter(typeof(IndexConverter<Light>))]
        public Index<Light> Lights { get; set; }

        [JsonProperty("groups")]
        [JsonConverter(typeof(IndexConverter<Group>))]
        public Index<Group> Groups { get; set; }

        [JsonProperty("config")]
        public Configuration Configuration { get; set; }

        [JsonProperty("schedules")]
        [JsonConverter(typeof(IndexConverter<Schedule>))]
        public Index<Schedule> Schedules { get; set; }
    }
}
