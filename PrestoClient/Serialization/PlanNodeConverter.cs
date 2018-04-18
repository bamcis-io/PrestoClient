using BAMCIS.PrestoClient.Model.Sql.Planner.Plan;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            JObject Token = JObject.Load(reader);

            if (!Token.TryGetValue("@type", StringComparison.OrdinalIgnoreCase, out JToken TypeToken))
            {
                throw new InvalidOperationException("Invalid presto query plan node object.");
            }

            Type ActualType = PlanNode.GetType(TypeToken.ToObject<PlanNodeType>(serializer));

            if (existingValue == null || existingValue.GetType() != ActualType)
            {
                // Don't use this approach, since there are no default constructors
                // JsonContract Contract = serializer.ContractResolver.ResolveContract(ActualType);
                // existingValue = Contract.DefaultCreator();
                return Token.ToObject(ActualType, serializer);
            }
            else
            {
                using (JsonReader DerivedTypeReader = Token.CreateReader())
                {
                    serializer.Populate(DerivedTypeReader, existingValue);
                }

                return existingValue;
            }
        }

        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}