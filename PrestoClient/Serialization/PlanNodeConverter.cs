using BAMCIS.PrestoClient.Model.Execution.PlanFlattener;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;

namespace BAMCIS.PrestoClient.Serialization
{
    public class PlanNodeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PlanNode);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken Token = JToken.Load(reader);
            JToken TypeToken = Token["@type"];
            if (TypeToken == null)
            {
                throw new InvalidOperationException("Invalid presto query plan node object.");
            }

            Type ActualType = PlanNode.GetType(TypeToken.ToObject<PlanNodeType>(serializer));

            if (existingValue == null || existingValue.GetType() != ActualType)
            {
                JsonContract Contract = serializer.ContractResolver.ResolveContract(ActualType);
                existingValue = Contract.DefaultCreator();
            }

            using (JsonReader DerivedTypeReader = Token.CreateReader())
            {
                serializer.Populate(DerivedTypeReader, existingValue);
            }

            return existingValue;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
