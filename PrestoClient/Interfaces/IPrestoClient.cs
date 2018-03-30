using BAMCIS.PrestoClient.Model.Jmx;
using BAMCIS.PrestoClient.Model.NodeInfo;
using BAMCIS.PrestoClient.Model.Query;
using BAMCIS.PrestoClient.Model.Statement;
using BAMCIS.PrestoClient.Model.Thread;
using System.Threading.Tasks;

namespace BAMCIS.PrestoClient.Interfaces
{
    public interface IPrestoClient
    {
        #region Threads

        Task<ListThreadsV1Response> ListThreads();

        Task<string> GetThreadUIHtml();

        #endregion

        #region Nodes

        Task<ListNodesV1Response> ListNodes();

        Task<ListFailedNodesV1Response> ListFailedNodes();

        #endregion

        #region Query

        Task KillQuery(string queryId);

        Task<ListQueriesV1Response> GetQueries();

        Task<GetQueryV1Response> GetQuery(string queryId);

        #endregion

        #region Statements

        Task<ExecuteQueryV1Response> ExecuteQueryV1(ExecuteQueryV1Request request);

        // Not yet available as of Presto 0.197
        // Task<ExecuteQueryResponse<QueryResultsV2>> ExecuteQueryV2(ExecuteQueryV2Request request);

        #endregion

        #region Jmx Mbean

        Task<JmxMbeanV1Response> GetJmxMbean(JmxMbeanV1Request request);

        #endregion
    }
}