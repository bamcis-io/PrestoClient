using System;
using System.Collections.Generic;
using System.Text;

namespace BAMCIS.PrestoClient.Model.Client
{
    public interface IQueryStatusInfo
    {
        string GetId();

        Uri GetInfoUri();

        Uri GetPartialCancelUri();

        IEnumerable<Column> GetColumns();

        StatementStats GetStats();

        QueryError GetError();

        string GetUpdateType();

        long GetUpdateCount();
    }
}
