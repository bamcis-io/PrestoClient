using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Jmx
{
    public class JmxMbeanV1Result
    {
        public string ObjectName { get; set; }

        public string ClassName { get; set; }

        public string Description { get; set; }

        public IEnumerable<JmxMbeanAttribute> Attributes { get; set; }

        public IEnumerable<JmxMbeanOperation> Operations { get; set; }
    }
}