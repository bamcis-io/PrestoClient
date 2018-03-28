using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BAMCIS.PrestoClient.Model.Execution.Buffer
{
    /// <summary>
    /// From com.facebook.presto.execution.buffer.BufferState.java
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BufferState
    {
        /// <summary>
        /// Additional buffers can be added.
        /// Any next state is allowed.
        /// </summary>
        OPEN,

        /// <summary>
        /// No more buffers can be added.
        /// Next state is {@link #FLUSHING}.
        /// </summary>
        NO_MORE_BUFFERS,

        /// <summary>
        /// No more pages can be added.
        /// Next state is {@link #FLUSHING}.
        /// </summary>
        NO_MORE_PAGES,

        /// <summary>
        /// No more pages or buffers can be added, and buffer is waiting
        /// for the final pages to be consumed.
        /// Next state is {@link #FINISHED}.
        /// </summary>
        FLUSHING,

        /// <summary>
        /// No more buffers can be added and all pages have been consumed.
        /// This is the terminal state.
        /// </summary>
        FINISHED,

        /// <summary>
        /// Buffer has failed.  No more buffers or pages can be added.  Readers
        /// will be blocked, as to not communicate a finished state.  It is
        /// assumed that the reader will be cleaned up elsewhere.
        /// This is the terminal state.
        /// </summary>
        FAILED
    }
}
