using Newtonsoft.Json;

namespace BAMCIS.PrestoClient.Model.Metadata
{
    /// <summary>
    /// From com.facebook.presto.metadata.LongVariableConstraint
    /// </summary>
    public class LongVariableConstraint
    {
        #region Public Properties

        public string Name { get; }

        public string Expression { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public LongVariableConstraint(string name, string expression)
        {
            this.Name = name;
            this.Expression = expression;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{this.Name}:{this.Expression}";
        }

        #endregion
    }
}
