using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Sql.Planner
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.Symbol.java
    /// </summary>
    public class Symbol : IComparable<Symbol>
    {
        #region Public Properties

        public string Name { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public Symbol(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "The name cannot be null or empty.");
            }

            this.Name = name;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return this.Name;
        }

        public int CompareTo(Symbol other)
        {
            return this.Name.CompareTo(other.Name);
        }

        #endregion
    }
}
