using Newtonsoft.Json;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Execution.Buffer
{
    /// <summary>
    /// From com.facebook.presto.execution.buffer.OutputBufferInfo.java
    /// </summary>
    public class OutputBufferInfo
    {
        #region Public Properties

        public string Type { get; }
        
        public BufferState State { get; }

        public bool CanAddBuffers { get; }

        public bool CanAddPages { get; }

        public long TotalBufferedBytes { get; }

        public long TotalBufferedPages { get; }

        public long TotalRowsSent { get; }

        public long TotalPagesSent { get; }

        public IEnumerable<BufferInfo> Buffers { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public OutputBufferInfo(
            string type, 
            BufferState state, 
            bool canAddBuffers, 
            bool canAddPages, 
            long totalBufferedBytes, 
            long totalBufferedPages,
            long totalRowsSent,
            long totalPagesSent,
            IEnumerable<BufferInfo> buffers
            )
        {
            this.Type = type;
            this.State = state;
            this.CanAddBuffers = canAddBuffers;
            this.CanAddPages = canAddPages;
            this.TotalBufferedBytes = totalBufferedBytes;
            this.TotalBufferedPages = totalBufferedPages;
            this.TotalRowsSent = totalRowsSent;
            this.TotalPagesSent = totalPagesSent;
            this.Buffers = buffers;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("type", this.Type)
                .Add("state", this.State)
                .Add("canAddBuffers", this.CanAddBuffers)
                .Add("canAddPages", this.CanAddPages)
                .Add("totalBufferedBytes", this.TotalBufferedBytes)
                .Add("totalBufferedPages", this.TotalBufferedPages)
                .Add("totalRowsSent", this.TotalRowsSent)
                .Add("totalPagesSent", this.TotalPagesSent)
                .Add("buffers", this.Buffers)
                .ToString();
        }

        #endregion
    }
}
