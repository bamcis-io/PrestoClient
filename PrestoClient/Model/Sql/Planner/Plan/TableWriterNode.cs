using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.TableWriterNode.java
    /// </summary>
    public class TableWriterNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }
        
        public WriterTarget Target { get; }

        public IEnumerable<Symbol> Outputs { get; }

        public IEnumerable<Symbol> Columns { get; }

        public IEnumerable<string> ColumnNames { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public PartitioningScheme PartitioningScheme { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public TableWriterNode(PlanNodeId id, PlanNode source, WriterTarget target, IEnumerable<Symbol> outputs, IEnumerable<Symbol> columns, IEnumerable<string> columnNames, PartitioningScheme partitioningScheme) : base(id)
        {
            this.Source = source ?? throw new ArgumentNullException("source");
            this.Target = target ?? throw new ArgumentNullException("target");
            this.Columns = columns ?? throw new ArgumentNullException("columns");
            this.ColumnNames = columnNames ?? throw new ArgumentNullException("columnNames");

            if (this.Columns.Count() != this.ColumnNames.Count())
            {
                throw new ArgumentException("Columns and column names sizes don't match.");
            }

            this.Outputs = outputs ?? throw new ArgumentNullException("outputs");
            this.PartitioningScheme = partitioningScheme;
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return this.Outputs;
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return this.Source;
        }

        #endregion
    }
}
