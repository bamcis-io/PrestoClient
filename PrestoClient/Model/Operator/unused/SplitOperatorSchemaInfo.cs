using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Query.QueryDetails.Operators
{
    /// <summary>
    /// This class is not currently used because the Operator Summary Info property
    /// is very dynamic when serialized as json by presto
    /// </summary>
    public class SplitOperatorSchemaInfo : SplitOperatorInfo
    {
        public object TableHandle { get; set; }

        public IDictionary<string, IDictionary<string, dynamic>> Filters { get; set; }

        public IEnumerable<string> Addresses { get; set; }
    }
}
