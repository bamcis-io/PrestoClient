using Newtonsoft.Json;

namespace BAMCIS.PrestoClient.Model.SPI.EventListener
{
    /// <summary>
    /// From com.facebook.presto.spi.eventlistener.StageGcStatistics.java
    /// </summary>
    public class StageGcStatistics
    {
        #region Public Properties

        public int StageId { get; }

        public int Tasks { get; }

        public int FullGcTasks { get; }

        public int MinFullGcSec { get; }

        public int MaxFullGcSec { get; }

        public int TotalFullGcSec { get; }

        public int AverageFullGcSec { get; }

        #endregion

        #region Constructors 

        [JsonConstructor]
        public StageGcStatistics(
            int stageId,
            int tasks,
            int fullGcTasks,
            int minFullGcSec,
            int maxFullGcSec,
            int totalFullGcSec,
            int averageFullGcSec
            )
        {
            this.StageId = stageId;
            this.Tasks = tasks;
            this.FullGcTasks = fullGcTasks;
            this.MinFullGcSec = minFullGcSec;
            this.MaxFullGcSec = maxFullGcSec;
            this.TotalFullGcSec = totalFullGcSec;
            this.AverageFullGcSec = averageFullGcSec;
        }

        #endregion
    }
}
