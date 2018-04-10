using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.Column.java
    /// </summary>
    public class Column
    {
        #region Public Properties

        public string Name { get; }

        public string Type { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public Column(string name, string type)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "The name cannot be null or empty.");
            }

            if (String.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException("type", "The type cannot be null or empty.");
            }

            this.Name = name;
            this.Type = type;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("name", this.Name)
                .Add("type", this.Type)
                .ToString();
        }

        #endregion
    }
}