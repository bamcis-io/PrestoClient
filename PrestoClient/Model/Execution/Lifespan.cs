using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.Lifespan.java
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class Lifespan
    {
        #region Private Fields

        private static readonly Lifespan TASK_WIDE = new Lifespan(false, 0);
        private static readonly string TASK_WIDE_STR = "TaskWide";

        #endregion

        #region Public Properties

        public bool Grouped { get; }

        public int GroupId { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public Lifespan(bool grouped, int groupId)
        {
            this.Grouped = grouped;
            this.GroupId = groupId;
        }

        public Lifespan(string value)
        {
            if (value.Equals(TASK_WIDE_STR))
            {
                this.GroupId = 0;
                this.Grouped = false;
            }
            else
            {
                if (value.StartsWith("Group"))
                {
                    this.Grouped = true;
                    this.GroupId = Int32.Parse(value.Substring(5));
                }
            }
        }

        #endregion

        #region Public Methods

        public static Lifespan TaskWide()
        {
            return TASK_WIDE;
        }

        public static Lifespan DriverGroup(int id)
        {
            return new Lifespan(true, id);
        }

        public bool IsTaskWide()
        {
            return !this.Grouped;
        }

        public override string ToString()
        {
            return (Grouped) ? $"Group{this.GroupId}" : TASK_WIDE_STR;
        }

        #endregion
    }
}
