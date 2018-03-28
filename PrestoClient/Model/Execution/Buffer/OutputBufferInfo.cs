using System;
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
            return $"OutputBufferInfo {{type={this.Type}, state={this.State}, canAddBuffers={this.CanAddBuffers}, canAddPages={this.CanAddPages}, totalBufferedBytes={this.TotalBufferedBytes}, totalBufferedPages={this.TotalBufferedPages}, totalRowsSent={this.TotalRowsSent}, totalPagesSent={this.TotalPagesSent}, buffers=[{String.Join(",", this.Buffers)}]}}";
        }

        #endregion
    }
}
