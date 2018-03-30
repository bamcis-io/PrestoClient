using Newtonsoft.Json;
using System;
using System.Text;

namespace BAMCIS.PrestoClient.Model.Metadata
{
    /// <summary>
    /// From com.facebook.presto.metadata.TypeVariableConstraint.java
    /// </summary>
    public class TypeVariableConstraint
    {
        #region Public Properties

        public string Name { get; }

        public bool ComparableRequired { get; }

        public bool OrderableRequired { get; }

        public string VariadicBound { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public TypeVariableConstraint(string name, bool comparableRequired, bool orderableRequired, string variadicBound)
        {
            this.Name = name;
            this.ComparableRequired = comparableRequired;
            this.OrderableRequired = orderableRequired;
            this.VariadicBound = variadicBound;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            StringBuilder SB = new StringBuilder(this.Name);

            if (this.ComparableRequired)
            {
                SB.Append(":comparable");
            }

            if (this.OrderableRequired)
            {
                SB.Append(":orderable");
            }

            if (!String.IsNullOrEmpty(this.VariadicBound))
            {
                SB.Append($":{this.VariadicBound}<*>");
            }

            return SB.ToString();
        }

        #endregion
    }
}
