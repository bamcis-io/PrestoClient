namespace BAMCIS.PrestoClient.Model.Jmx
{
    public class JmxMbeanAttribute
    {
        #region Public Properties

        public string Name { get; set; }

        /// <summary>
        /// This is the type of the value, could be long, double, java.util.Map, java.util.concurrent.TimeUnit
        /// </summary>
        public string Type { get; set; }

        public string Description { get; set; }

        public bool Readable { get; set; }

        public bool Writable { get; set; }

        /// <summary>
        /// This will be the Java type defined in the "Type" property
        /// </summary>
        public dynamic Value { get; set; }

        #endregion
    }
}