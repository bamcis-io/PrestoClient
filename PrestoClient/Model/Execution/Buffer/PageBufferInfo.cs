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
            return StringHelper.Build(this)
                .Add("partition", this.Partition)
                .Add("bufferedPages", this.BufferedPages)
                .Add("bufferedBytes", this.BufferedBytes)
                .Add("rowsAdded", this.RowsAdded)
                .Add("pagesAdded", this.PagesAdded)
                .ToString();
        }

        #endregion
    }
}
