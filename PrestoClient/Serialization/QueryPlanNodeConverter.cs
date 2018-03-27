using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using BAMCIS.PrestoClient.Model.Query.QueryDetails.Nodes;
using System;

namespace BAMCIS.PrestoClient.Serialization
{
    public class QueryPlanNodeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(QueryPlanNode);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken Token = JToken.Load(reader);
            JToken TypeToken = Token["@type"];
            if (TypeToken == null)
            {
                throw new InvalidOperationException("Invalid presto query plan node object.");
            }

            Type ActualType = QueryPlanNode.GetType(TypeToken.ToObject<QueryNodeType>(serializer));

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
