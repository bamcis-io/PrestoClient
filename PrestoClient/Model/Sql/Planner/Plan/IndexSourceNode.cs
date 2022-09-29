﻿using BAMCIS.PrestoClient.Model.Metadata;
using BAMCIS.PrestoClient.Model.SPI;
using BAMCIS.PrestoClient.Model.SPI.Predicate;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.IndexSourceNode.java
    /// </summary>
    public class IndexSourceNode : PlanNode
    {
        #region Public Properties

        public IndexHandle IndexHandle { get; }

        public TableHandle TableHandle { get; }

        public HashSet<Symbol> LookupSymbols { get; }

        public IEnumerable<Symbol> OutputSymbols { get; }

        /// <summary>
        /// TODO: Key is supposed to be Symbol, Key is IColumnHandle
        /// </summary>
        public IDictionary<string, dynamic> Assignments { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public IndexSourceNode(
            PlanNodeId id,
            IndexHandle indexHandle,
            TableHandle tableHandle,
            HashSet<Symbol> lookupSymbols,
            IEnumerable<Symbol> outputSymbols,
            IDictionary<string, dynamic> assignments,
            TupleDomainPlaceHolder<dynamic> currentConstraint
            ) : base(id)
        {
            this.IndexHandle = indexHandle ?? throw new ArgumentNullException("indexHandle");
            this.TableHandle = tableHandle ?? throw new ArgumentNullException("tableHanlde");
            this.LookupSymbols = lookupSymbols ?? throw new ArgumentNullException("lookupSymbols");
            this.OutputSymbols = outputSymbols ?? throw new ArgumentNullException("outputSymbols");
            this.Assignments = assignments ?? throw new ArgumentNullException("assignments");
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return this.OutputSymbols;
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            return new PlanNode[0];
        }

        #endregion
    }
}
