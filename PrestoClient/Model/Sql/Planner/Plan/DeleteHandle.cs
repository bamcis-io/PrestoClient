using BAMCIS.PrestoClient.Model.Metadata;
using BAMCIS.PrestoClient.Model.SPI;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.TableWriterNode.java (internal class DeleteHandle)
    /// </summary>
    public class DeleteHandle : WriterTarget
    {
        #region Public Properties

        public TableHandle Handle { get; }

        public SchemaTableName SchemaTableName { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public DeleteHandle(TableHandle handle, SchemaTableName schemaTableName)
        {
            this.Handle = handle ?? throw new ArgumentNullException("handle");
            this.SchemaTableName = schemaTableName ?? throw new ArgumentNullException("schemaTableName");
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return this.Handle.ToString();
        }

        #endregion
    }
}
