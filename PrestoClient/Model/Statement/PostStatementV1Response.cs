using BAMCIS.PrestoClient.Model.Client;
using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAMCIS.PrestoClient.Model.Statement
{
    public class PostStatementV1Response : IQueryData, IQueryStatusInfo
    {
        #region Private Properties

        private QueryResultsV1 _queryResults = null;

        #endregion

        #region Public Properties

        public QueryResultsV1 QueryResults => _queryResults;

        #endregion

        #region Constructors

        public PostStatementV1Response(QueryResultsV1 queryResults)
        {
            _queryResults = queryResults;
        }

        #endregion

        #region Public Methods

        public IEnumerable<Column> GetColumns()
        {
            return _queryResults.GetColumns();
        }

        public IEnumerable<List<object>> GetData()
        {
            return _queryResults.Data;
        }

        public QueryError GetError()
        {
            return _queryResults.GetError();
        }

        public string GetId()
        {
            return _queryResults.GetId();
        }

        public Uri GetInfoUri()
        {
            return _queryResults.GetInfoUri();
        }

        public Uri GetNextUri()
        {
            return _queryResults.GetNextUri();
        }

        public Uri GetPartialCancelUri()
        {
            return _queryResults.GetPartialCancelUri();
        }

        public StatementStats GetStats()
        {
            return _queryResults.GetStats();
        }

        public long GetUpdateCount()
        {
            return _queryResults.GetUpdateCount();
        }

        public string GetUpdateType()
        {
            return _queryResults.GetUpdateType();
        }

        #endregion
    }
}
