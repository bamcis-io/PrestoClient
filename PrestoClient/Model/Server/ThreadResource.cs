using Newtonsoft.Json;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Server
{
    /// <summary>
    /// A Java thread resource.
    /// From com.facebook.presto.server.ThreadResource.java
    /// 
    /// This class is really just a data holder since the actual implementation
    /// creates and populates the thread info
    /// </summary>
    public class ThreadResource
    {
        #region Public Properties

        public IEnumerable<Info> ThreadInfo { get; private set; }

        #endregion

        #region Constructors

        private ThreadResource()
        { }

        #endregion

        #region Internal Classes

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

        #endregion
    }
}