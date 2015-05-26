using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace OneCog.Io.Philips.Hue.Dto
{
    public class StateChangeResponseConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(StateChangeResponse).Equals(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JArray actions = JArray.Load(reader);

            Change[] changes = actions
                    .OfType<JObject>()
                    .SelectMany(action => action.Properties())
                    .Where(prop => prop.Value is JObject)
                    .Select(prop => new { Success = prop.Name.Equals("success"), Change = ((JObject)prop.Value).Properties().First() })
                    .Where(tuple => tuple.Change != null)
                    .Select(tuple => new Change { Key = tuple.Change.Name, Value = tuple.Change.Value.ToObject<object>(), Success = tuple.Success })
                    .ToArray();

            return new StateChangeResponse { Changes = changes };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}
