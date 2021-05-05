using BAMCIS.PrestoClient.Model.Jmx;
using BAMCIS.PrestoClient.Model.NodeInfo;
using BAMCIS.PrestoClient.Model.Query;
using BAMCIS.PrestoClient.Model.SPI;
using BAMCIS.PrestoClient.Model.Statement;
using BAMCIS.PrestoClient.Model.Thread;
using System.Threading;
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
        /// Gets information about the in use threads in the cluster.
        /// </summary>
        /// <returns>
        /// Information about all of the threads and their state.
        /// </returns>
        Task<ListThreadsV1Response> ListThreads();

        /// <summary>
        /// Gets information about the in use threads in the cluster.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>
        /// Information about all of the threads and their state.
        /// </returns>
        Task<ListThreadsV1Response> ListThreads(CancellationToken cancellationToken);

        /// <summary>
        /// Gets the web ui html that displays information about the threads
        /// in the cluster and optionally opens that web page.
        /// </summary>
        /// <returns>The web page html/javascript/css.</returns>
        Task<string> GetThreadUIHtml();

        /// <summary>
        /// Gets the web ui html that displays information about the threads
        /// in the cluster and optionally opens that web page.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>The web page html/javascript/css.</returns>
        Task<string> GetThreadUIHtml(CancellationToken cancellationToken);

        #endregion

        #region Nodes

        /// <summary>
        /// Gets the worker nodes in a presto cluster
        /// </summary>
        /// <returns>
        /// Information about all of the worker nodes. If the request is unsuccessful, 
        /// a PrestoException is thrown.
        /// </returns>
        Task<ListNodesV1Response> ListNodes();

        /// <summary>
        /// Gets the worker nodes in a presto cluster
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>
        /// Information about all of the worker nodes. If the request is unsuccessful, 
        /// a PrestoException is thrown.
        /// </returns>
        Task<ListNodesV1Response> ListNodes(CancellationToken cancellationToken);

        /// <summary>
        /// Gets any failed nodes in a presto cluster.
        /// </summary>
        /// <returns>
        /// Information about the failed nodes. If there are no failed nodes, 
        /// the FailedNodes property will be null.
        /// </returns>
        Task<ListFailedNodesV1Response> ListFailedNodes();

        /// <summary>
        /// Gets any failed nodes in a presto cluster.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>
        /// Information about the failed nodes. If there are no failed nodes, 
        /// the FailedNodes property will be null.
        /// </returns>
        Task<ListFailedNodesV1Response> ListFailedNodes(CancellationToken cancellationToken);

        #endregion

        #region Query

        /// <summary>
        /// Kills an active query statement
        /// </summary>
        /// <param name="queryId">The id of the query to kill</param>
        /// <returns>No value is returned, but the method will throw an exception if it was not successful</returns>
        Task KillQuery(string queryId);

        /// <summary>
        /// Kills an active query statement
        /// </summary>
        /// <param name="queryId">The Id of the query to kill</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>No value is returned, but the method will throw an exception if it was not successful</returns>
        Task KillQuery(string queryId, CancellationToken cancellationToken);

        /// <summary>
        /// This method returns information and statistics about queries that
        /// are currently being executed on a Presto coordinator that have not been purged
        /// </summary>
        /// <returns>Details on the queries</returns>
        Task<ListQueriesV1Response> GetQueries();

        /// <summary>
        /// This method returns information and statistics about queries that
        /// are currently being executed on a Presto coordinator that have not been purged
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>Details on the queries</returns>
        Task<ListQueriesV1Response> GetQueries(CancellationToken cancellationToken);

        /// <summary>
        /// Gets a detailed summary of the specified query
        /// </summary>
        /// <param name="queryId">The id of the query to retrieve details about.</param>
        /// <returns>Detailed summary of the query</returns>
        Task<GetQueryV1Response> GetQuery(string queryId);

        /// <summary>
        /// Gets a detailed summary of the specified query
        /// </summary>
        /// <param name="queryId">The id of the query to retrieve details about.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>Detailed summary of the query</returns>
        Task<GetQueryV1Response> GetQuery(string queryId, CancellationToken CancellationToken);

        /// <summary>
        /// Gets a detailed summary of the specified query
        /// </summary>
        /// <param name="queryId">The id of the query to retrieve details about.</param>
        /// <returns>Detailed summary of the query</returns>
        Task<GetQueryV1Response> GetQuery(QueryId queryId);

        /// <summary>
        /// Gets a detailed summary of the specified query
        /// </summary>
        /// <param name="queryId">The id of the query to retrieve details about.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>Detailed summary of the query</returns>
        Task<GetQueryV1Response> GetQuery(QueryId queryId, CancellationToken cancellationToken);

        #endregion

        #region Statements

        /// <summary>
        /// Submits a Presto SQL statement for execution. The Presto client 
        /// executes queries on behalf of a user against a catalog and a schema.
        /// </summary>
        /// <param name="request">The query execution request.</param>
        /// <returns>The resulting response object from the query.</returns>
        Task<ExecuteQueryV1Response> ExecuteQueryV1(ExecuteQueryV1Request request);

        /// <summary>
        /// Submits a Presto SQL statement for execution. The Presto client 
        /// executes queries on behalf of a user against a catalog and a schema.
        /// </summary>
        /// <param name="request">The query execution request.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>The resulting response object from the query.</returns>
        Task<ExecuteQueryV1Response> ExecuteQueryV1(ExecuteQueryV1Request request, CancellationToken cancellationToken);

        // Not yet available as of Presto 0.198
        // Task<ExecuteQueryResponse<QueryResultsV2>> ExecuteQueryV2(ExecuteQueryV2Request request);
        // Task<ExecuteQueryResponse<QueryResultsV2>> ExecuteQueryV2(ExecuteQueryV2Request request, CancellationToken cancellationToken);

        #endregion

        #region Jmx Mbean

        /// <summary>
        /// Gets details about a specified Jmx Mbean
        /// </summary>
        /// <param name="request">The request details</param>
        /// <returns>Details about the specified Jmx Mbean</returns>
        Task<JmxMbeanV1Response> GetJmxMbean(JmxMbeanV1Request request);

        /// <summary>
        /// Gets details about a specified Jmx Mbean
        /// </summary>
        /// <param name="request">The request details</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>Details about the specified Jmx Mbean</returns>
        Task<JmxMbeanV1Response> GetJmxMbean(JmxMbeanV1Request request, CancellationToken cancellationToken);

        #endregion
    }
}