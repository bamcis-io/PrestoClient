using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Client
{
    /// <summary>
    /// From com.facebook.presto.client.QueryStatusInfo.java
    /// </summary>
    public interface IQueryStatusInfo
    {
        string GetId();

        Uri GetInfoUri();

        Uri GetPartialCancelUri();

        Uri GetNextUri();

        IEnumerable<Column> GetColumns();

        StatementStats GetStats();

        QueryError GetError();

        string GetUpdateType();

        long GetUpdateCount();
    }
}
