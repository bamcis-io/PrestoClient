using BAMCIS.PrestoClient.Model.Connector;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.Output.java
    /// </summary>
    public class Output
    {
        #region Public Properties

        public CatalogHandle CatalogHandle { get; }

        public string Schema { get; }

        public string Table { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public Output(string catalogName, string schema, string table)
        {
            if (String.IsNullOrEmpty(catalogName))
            {
                throw new ArgumentNullException("catalogName", "catalogName cannot be null or empty.");
            }

            if (String.IsNullOrEmpty(schema))
            {
                throw new ArgumentNullException("schema", "Schema cannot be null or empty.");
            }

            if (String.IsNullOrEmpty(table))
            {
                throw new ArgumentNullException("table", "Table cannot be null or empty.");
            }

            this.CatalogHandle = new CatalogHandle(catalogName);
        }

        #endregion
    }
}
