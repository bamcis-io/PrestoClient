using BAMCIS.PrestoClient.Model.Execution.PlanFlattener;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Serialization
{
    public class NodeTypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PlanNode);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // The node type strings produced by presto use hypens for 
            // things like hive-hadoop2 instead of underscores which we
            // have to use in an enum
            string Temp = reader.Value.ToString().ToUpper().Replace('-', '_');

            return Enum.Parse(typeof(PlanNodeType), Temp);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string Temp = value.ToString().ToLower().Replace('_', '-');

            writer.WriteValue(Temp);
        }
    }
}
