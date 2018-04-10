using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.Partitioning.java
    /// </summary>
    public class Partitioning
    {
        #region Public Properties

        public PartitioningHandle Handle { get; }

        public IEnumerable<ArgumentBinding> Arguments { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public Partitioning(PartitioningHandle handle, IEnumerable<ArgumentBinding> arguments)
        {
            this.Handle = handle ?? throw new ArgumentNullException("handle");
            this.Arguments = arguments ?? throw new ArgumentNullException("arguments");
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("handle", this.Handle)
                .Add("arguments", this.Arguments)
                .ToString();
        }

        #endregion
    }
}