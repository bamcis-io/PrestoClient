using BAMCIS.PrestoClient.Model.Jmx;
using BAMCIS.PrestoClient.Model.NodeInfo;
using BAMCIS.PrestoClient.Model.Query;
using BAMCIS.PrestoClient.Model.Statement;
using BAMCIS.PrestoClient.Model.Thread;
using System.Threading.Tasks;

namespace BAMCIS.PrestoClient.Interfaces
{
    /// <summary>
    /// Provides an interface to interact with the Presto REST API
    /// </summary>
    public interface IPrestoClient
    {
        #region Threads

        /// <summary>
        /// Lists thread details
        /// </summary>
        /// <returns>Details about each thread</returns>
        Task<ListThreadsV1Response> ListThreads();

        /// <summary>
        /// Retrieves the web ui HTML page for thread stats
        /// </summary>
        /// <returns></returns>
        Task<string> GetThreadUIHtml();

        #endregion

        #region Nodes

        /// <summary>
        /// Gets a list of worker nodes
        /// </summary>
        /// <returns>The worker nodes</returns>
        Task<ListNodesV1Response> ListNodes();

        /// <summary>
        /// Gets a list of failed worker nodes
        /// </summary>
        /// <returns>The list of failed worker nodes</returns>
        Task<ListFailedNodesV1Response> ListFailedNodes();

        #endregion

        #region Query

        /// <summary>
        /// Kills a specified query
        /// </summary>
        /// <param name="queryId">The id of the query</param>
        /// <returns></returns>
        Task KillQuery(string queryId);

        /// <summary>
        /// Gets basic query details about all queries that have not been purged
        /// </summary>
        /// <returns>Details on the queries</returns>
        Task<ListQueriesV1Response> GetQueries();

        /// <summary>
        /// Gets a detailed summary of the specified query
        /// </summary>
        /// <param name="queryId">The id of the query</param>
        /// <returns>Detailed summary of the query</returns>
        Task<GetQueryV1Response> GetQuery(string queryId);

        #endregion

        #region Statements

        /// <summary>
        /// Executes a Presto SQL statement
        /// </summary>
        /// <param name="request">The request details</param>
        /// <returns>The response from the statement</returns>
        Task<ExecuteQueryV1Response> ExecuteQueryV1(ExecuteQueryV1Request request);

        // Not yet available as of Presto 0.197
        // Task<ExecuteQueryResponse<QueryResultsV2>> ExecuteQueryV2(ExecuteQueryV2Request request);

        #endregion

        #region Jmx Mbean

        /// <summary>
        /// Gets details about a specified Jmx Mbean
        /// </summary>
        /// <param name="request">The request details</param>
        /// <returns>Details about the specified Jmx Mbean</returns>
        Task<JmxMbeanV1Response> GetJmxMbean(JmxMbeanV1Request request);

        #endregion
    }
}