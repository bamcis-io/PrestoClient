using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.SchemaTableName.java
    /// </summary>
    public class SchemaTableName
    {
        #region Public Properties

        public string SchemaName { get; }

        public string TableName { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public SchemaTableName(string schemaName, string tableName)
        {
            ParameterCheck.NotNullOrEmpty(schemaName, "schemaName");
            ParameterCheck.NotNullOrEmpty(tableName, "tableName");

            this.SchemaName = schemaName.ToLower();
            this.TableName = tableName.ToLower();
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{this.SchemaName}.{this.TableName}";
        }

        #endregion
    }
}
