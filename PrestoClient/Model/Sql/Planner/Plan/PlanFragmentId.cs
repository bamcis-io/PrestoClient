using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.PlanFragmentId.java
    /// </summary>
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

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Id);
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            PlanFragmentId That = (PlanFragmentId)obj;

            if (!this.Id.Equals(That.Id))
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
