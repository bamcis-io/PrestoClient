using Newtonsoft.Json;

namespace BAMCIS.PrestoClient.Model.Execution.Buffer
{
    /// <summary>
    /// From com.facebook.presto.execution.buffer.PageBufferInfo.java
    /// </summary>
    public class PageBufferInfo
    {
        #region Public Properties

        public int Partition { get; }

        public long BufferedPages { get; }

        public long BufferedBytes { get; }

        public long RowsAdded { get; }

        public long PagesAdded { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public PageBufferInfo(int partition, long bufferedPages, long bufferedBytes, long rowsAdded, long pagesAdded)
        {
            this.Partition = partition;
            this.BufferedPages = bufferedPages;
            this.BufferedBytes = bufferedBytes;
            this.RowsAdded = rowsAdded;
            this.PagesAdded = pagesAdded;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"PageBufferInfo {{partition={this.Partition}, bufferedPages={this.BufferedPages}, bufferedBytes={this.BufferedBytes}, rowsAdded={this.RowsAdded}, pagesAdded={this.PagesAdded}}}";
        }

        #endregion
    }
}
