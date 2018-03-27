using Newtonsoft.Json;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Server
{
    /// <summary>
    /// A Java thread resource.
    /// From com.facebook.presto.server.ThreadResource.java
    /// </summary>
    public class ThreadResource
    {
        public IEnumerable<Info> ThreadInfo { get; private set; }

        public class Info
        {
            #region Public Properties

            public long Id { get; }

            public string Name { get; }

            public ThreadState State { get; }

            public long LockOnwer { get; }

            public IEnumerable<StackLine> StackTrace { get; }

            #endregion

            #region Constructors

            [JsonConstructor]
            public Info(long id, string name, ThreadState state, long lockOwner, IEnumerable<StackLine> stackTrace)
            {
                this.Id = id;
                this.Name = name;
                this.State = state;
                this.LockOnwer = lockOwner;
                this.StackTrace = stackTrace;
            }

            #endregion
        }
    }
}