using BAMCIS.PrestoClient.Model.SPI;
using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.TaskId.java
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class TaskId
    {
        #region Public Properties

        public string FullId { get; }

        #endregion

        #region Constructors

        public TaskId(string queryId, int stageId, int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException("id", "The id cannot be less than 0.");
            }

            this.FullId = $"{queryId}.{stageId}.{id}";
        }

        public TaskId(StageId stageId, int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException("id", "The id cannot be less than 0.");
            }

            this.FullId = $"{stageId.QueryId.Id}.{stageId.Id}.{id}";
        }

        [JsonConstructor]
        public TaskId(string fullId)
        {
            this.FullId = fullId;
        }

        #endregion

        #region Public Methods

        public QueryId GetQueryId()
        {
            return new QueryId(this.FullId.Split('.')[0]);
        }

        public StageId GetStageId()
        {
            return new StageId(this.FullId.Split('.')[1]);
        }

        public int GetId()
        {
            return Int32.Parse(this.FullId.Split('.')[2]);
        }

        public override string ToString()
        {
            return this.FullId;
        }

        #endregion
    }
}
