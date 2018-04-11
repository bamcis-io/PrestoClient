using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Sql.Planner
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.Symbol.java
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class Symbol : IComparable<Symbol>
    {
        #region Public Properties

        public string Name { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public Symbol(string name)
        {
            ParameterCheck.NotNullOrEmpty(name, "name");

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
