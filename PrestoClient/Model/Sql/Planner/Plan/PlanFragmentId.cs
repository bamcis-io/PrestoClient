using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class PlanFragmentId
    {
        #region Public Properties

        public string Id { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public PlanFragmentId(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id", "The id cannot be null or empty");
            }

            this.Id = id;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return this.Id;
        }

        #endregion
    }
}
