using BAMCIS.PrestoClient.Model.Sql.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.Assignments.java
    /// </summary>
    public class Assignments
    {
        #region Public Properties

        /// <summary>
        /// TODO: Key should be <Symbol, Expression>
        /// </summary>
        public IDictionary<string, dynamic> assignments { get; }

        #endregion

        #region Constructors

        public Assignments(IDictionary<string, dynamic> assignments)
        {
            this.assignments = assignments ?? throw new ArgumentNullException("assignments");
        }

        #endregion

        #region Public Methods

        public IEnumerable<Symbol> GetOutputs()
        {
            return this.assignments.Keys.Select(x => new Symbol(x));
        }

        public IEnumerable<dynamic> GetExpressions()
        {
            return this.assignments.Values;
        }

        public HashSet<Symbol> GetSymbols()
        {
            return new HashSet<Symbol>(this.assignments.Keys.Select(x => new Symbol(x)));
        }

        public int Size()
        {
            return this.assignments.Count;
        }

        #endregion
    }
}
