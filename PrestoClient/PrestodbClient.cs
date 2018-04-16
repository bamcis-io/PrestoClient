using Newtonsoft.Json;
using BAMCIS.PrestoClient.Interfaces;
using BAMCIS.PrestoClient.Model;
using BAMCIS.PrestoClient.Model.Jmx;
using BAMCIS.PrestoClient.Model.Query;
using BAMCIS.PrestoClient.Model.Statement;
using BAMCIS.PrestoClient.Model.Thread;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Web;
using BAMCIS.PrestoClient.Model.NodeInfo;
using BAMCIS.PrestoClient.Model.SPI;

namespace BAMCIS.PrestoClient
{
    /// <summary>
    /// The client used to connect to a presto coordinator and execute statements, query nodes, get query info and summaries, and terminate running queries.
    /// </summary>
    public class PrestodbClient : IPrestoClient
    {
        #region Private Properties

        private static readonly string AssemblyVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        /// <summary>
        /// The cookie container all of the handlers will use so they all have access to the same cookies
        /// </summary>
        private CookieContainer Cookies = new CookieContainer();

        /// <summary>
        /// A normal http handler
        /// </summary>
        private HttpClientHandler NormalHandler;

        /// <summary>
        /// A handler that provides a log when SSL errors occur and then ignores those errors
        /// </summary>
        private HttpClientHandler IgnoreSslErrorHandler;

        /// <summary>
        /// The standard HTTP client using the Handler
        /// </summary>
        private HttpClient NormalClient;

        /// <summary>
        /// An HTTP client that ignores SSL errors
        /// </summary>
        private HttpClient IgnoreSslErrorClient;

        /// <summary>
        /// Initializes HTTP Handler and Client
        /// </summary>
        private void InitializeHttpClients()
        {
            this.NormalHandler = new HttpClientHandler()
            {
                CookieContainer = Cookies,
                ServerCertificateCustomValidationCallback = (request, cert, chain, sslPolicyErrors) =>
                {
                    //If there is an error with the SSL cert, log it
                    if (sslPolicyErrors != SslPolicyErrors.None)
                    {
                        // Log error
                    }

                    return sslPolicyErrors == SslPolicyErrors.None;
                }
            };

            this.IgnoreSslErrorHandler = new HttpClientHandler()
            {
                CookieContainer = Cookies,
                ServerCertificateCustomValidationCallback = (request, cert, chain, sslPolicyErrors) =>
                {
                    //If there is an error with the SSL cert, log it, but let the request continue
                    if (sslPolicyErrors != SslPolicyErrors.None)
                    {
                        // Log error
                    }

                    return true;
                }
            };

            this.NormalClient = new HttpClient(this.NormalHandler);
            this.IgnoreSslErrorClient = new HttpClient(this.IgnoreSslErrorHandler);
        }

        #endregion

        #region Public Properties

        public PrestoClientSessionConfig Configuration { get; private set; }

        #endregion

        #region Constructor

        public PrestodbClient()
        {
            // Initialize with defaults
            this.Configuration = new PrestoClientSessionConfig();

            this.InitializeHttpClients();
        }

        public PrestodbClient(PrestoClientSessionConfig config)
        {
            this.Configuration = config ?? throw new ArgumentNullException("config", "The presto client configuration cannot be null.");

            this.InitializeHttpClients();
        }

        #endregion

        #region Public Methods

        #region Threads

        /// <summary>
        /// Gets information about the in use threads in the cluster.
        /// </summary>
        /// <returns>
        /// Information about all of the threads and their state.
        /// </returns>
        public async Task<ListThreadsV1Response> ListThreads()
        {
            HttpClient LocalClient = (this.Configuration.IgnoreSslErrors) ? this.IgnoreSslErrorClient : this.NormalClient;

            Uri Path = this.BuildUri("/thread");

            HttpRequestMessage Request = this.BuildRequest(Path, HttpMethod.Get);

            HttpResponseMessage Response = await LocalClient.SendAsync(Request);

            string Json = await Response.Content.ReadAsStringAsync();

            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new PrestoWebException(await Response.Content.ReadAsStringAsync(), Response.StatusCode);
            }
            else
            {
                ListThreadsV1Response Result = new ListThreadsV1Response(Json);

                return Result;
            }
        }

        /// <summary>
        /// Gets the web ui html that displays information about the threads
        /// in the cluster and optionally opens that web page.
        /// </summary>
        /// <returns>The web page html/javascript/css.</returns>
        public async Task<string> GetThreadUIHtml()
        {
            HttpClient LocalClient = (this.Configuration.IgnoreSslErrors) ? this.IgnoreSslErrorClient : this.NormalClient;

            StringBuilder SB = new StringBuilder();

            string Scheme = (this.Configuration.UseSsl) ? "https" : "http";
            SB.Append($"{Scheme}://{this.Configuration.Host}");

            // Only add non-standard ports
            if ((Scheme == "http" && this.Configuration.Port != 80) || (Scheme == "https" && this.Configuration.Port != 443))
            {
                SB.Append($":{this.Configuration.Port}");
            }

            SB.Append($"/ui/thread");

            Uri Path = new Uri(SB.ToString());

            HttpResponseMessage Response = await LocalClient.GetAsync(Path);

            string Html = await Response.Content.ReadAsStringAsync();

            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new PrestoWebException(await Response.Content.ReadAsStringAsync(), Response.StatusCode);
            }
            else
            {
                return Html;
            }
        }

        #endregion

        #region Nodes

        /// <summary>
        /// Gets the worker nodes in a presto cluster
        /// </summary>
        /// <returns>
        /// Information about all of the worker nodes. If the request is unsuccessful, 
        /// a PrestoException is thrown.
        /// </returns>
        public async Task<ListNodesV1Response> ListNodes()
        {
            HttpClient LocalClient = (this.Configuration.IgnoreSslErrors) ? this.IgnoreSslErrorClient : this.NormalClient;

            Uri Path = this.BuildUri("/node");

            HttpRequestMessage Request = this.BuildRequest(Path, HttpMethod.Get);

            HttpResponseMessage Response = await LocalClient.SendAsync(Request);

            string Json = await Response.Content.ReadAsStringAsync();

            /*
             * Response without Failures
             * 
             *  [
             *      {
             *          "uri":"http://10.209.57.156:8080",
             *          "recentRequests":25.181940555111073,
             *          "recentFailures":0.0,
             *          "recentSuccesses":25.195472984170983,
             *          "lastRequestTime":"2013-12-22T13:32:44.673-05:00",
             *          "lastResponseTime":"2013-12-22T13:32:44.677-05:00",
             *          "age":"14155.28ms",
             *          "recentFailureRatio":0.0,
             *          "recentFailuresByType":{}
             *      }
             *  ]
             * 
             * Response with Failures
             *  [
             *      {
             *          "age": "4.45m",
             *          "lastFailureInfo": {
             *              "message": "Connect Timeout",
             *              "stack": [
             *                  "org.eclipse.jetty.io.ManagedSelector$ConnectTimeout.run(ManagedSelector.java:683)",
             *                  ....
             *                  "java.lang.Thread.run(Thread.java:745)"
             *              ],
             *              "suppressed": [],
             *              "type": "java.net.SocketTimeoutException"
             *          },
             *          "lastRequestTime": "2017-08-05T11:53:00.647Z",
             *          "lastResponseTime": "2017-08-05T11:53:00.647Z",
             *          "recentFailureRatio": 0.47263053472046446,
             *          "recentFailures": 2.8445543205610617,
             *          "recentFailuresByType": {
             *              "java.net.SocketTimeoutException": 2.8445543205610617
             *          },
             *          "recentRequests": 6.018558073577414,
             *          "recentSuccesses": 3.1746446343010297,
             *          "uri": "http://172.19.0.3:8080"
             *      }
             *  ]
             * 
            */

            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new PrestoWebException(await Response.Content.ReadAsStringAsync(), Response.StatusCode);
            }
            else
            {
                ListNodesV1Response Result = new ListNodesV1Response(Json);

                return Result;
            }
        }

        /// <summary>
        /// Gets any failed nodes in a presto cluster.
        /// </summary>
        /// <returns>
        /// Information about the failed nodes. If there are no failed nodes, 
        /// the FailedNodes property will be null.
        /// </returns>
        public async Task<ListFailedNodesV1Response> ListFailedNodes()
        {
            HttpClient LocalClient = (this.Configuration.IgnoreSslErrors) ? this.IgnoreSslErrorClient : this.NormalClient;

            StringBuilder SB = new StringBuilder();

            string Scheme = (this.Configuration.UseSsl) ? "https" : "http";
            SB.Append($"{Scheme}://{this.Configuration.Host}");

            // Only add non-standard ports
            if ((Scheme == "http" && this.Configuration.Port != 80) || (Scheme == "https" && this.Configuration.Port != 443))
            {
                SB.Append($":{this.Configuration.Port}");
            }

            SB.Append($"/{this.GetVersionString(this.Configuration.Version)}/node/failed");

            Uri Path = new Uri(SB.ToString());

            HttpResponseMessage Response = await LocalClient.GetAsync(Path);

            string Json = await Response.Content.ReadAsStringAsync();

            /*
             * Response with Failures
             *  [
             *      {
             *          "age": "4.45m",
             *          "lastFailureInfo": {
             *              "message": "Connect Timeout",
             *              "stack": [
             *                  "org.eclipse.jetty.io.ManagedSelector$ConnectTimeout.run(ManagedSelector.java:683)",
             *                  ....
             *                  "java.lang.Thread.run(Thread.java:745)"
             *              ],
             *              "suppressed": [],
             *              "type": "java.net.SocketTimeoutException"
             *          },
             *          "lastRequestTime": "2017-08-05T11:53:00.647Z",
             *          "lastResponseTime": "2017-08-05T11:53:00.647Z",
             *          "recentFailureRatio": 0.47263053472046446,
             *          "recentFailures": 2.8445543205610617,
             *          "recentFailuresByType": {
             *              "java.net.SocketTimeoutException": 2.8445543205610617
             *          },
             *          "recentRequests": 6.018558073577414,
             *          "recentSuccesses": 3.1746446343010297,
             *          "uri": "http://172.19.0.3:8080"
             *      }
             *  ]
             * 
            */

            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new PrestoWebException(await Response.Content.ReadAsStringAsync(), Response.StatusCode);
            }
            else
            {
                ListFailedNodesV1Response Result = new ListFailedNodesV1Response(Json);

                return Result;
            }
        }

        #endregion

        #region Query

        /// <summary>
        /// Kills an active query statement
        /// </summary>
        /// <param name="queryId">The Id of the query to kill</param>
        /// <param name="options">The header options to supply with the request to kill the query</param>
        /// <returns>No value is returned, but the method will throw an exception if it was not successful</returns>
        public async Task KillQuery(string queryId)
        {
            HttpClient localClient = (this.Configuration.IgnoreSslErrors) ? this.IgnoreSslErrorClient : this.NormalClient;

            Uri Path = this.BuildUri($"/query/{queryId}");

            HttpRequestMessage Request = this.BuildRequest(Path, HttpMethod.Delete);

            HttpResponseMessage Response = await localClient.SendAsync(Request);

            // Expect a 204 response
            if (Response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new PrestoWebException(await Response.Content.ReadAsStringAsync(), Response.StatusCode);
            }
        }

        /// <summary>
        /// This service returns information and statistics about queries that
        /// are currently being executed on a Presto coordinator.
        /// </summary>
        /// <returns></returns>
        public async Task<ListQueriesV1Response> GetQueries()
        {
            HttpClient LocalClient = (this.Configuration.IgnoreSslErrors) ? this.IgnoreSslErrorClient : this.NormalClient;

            Uri Path = this.BuildUri($"/query");

            HttpRequestMessage Request = this.BuildRequest(Path, HttpMethod.Get);

            HttpResponseMessage Response = await LocalClient.SendAsync(Request);

            // Expect a 200 response
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new PrestoWebException(await Response.Content.ReadAsStringAsync(), Response.StatusCode);
            }
            else
            {
                ListQueriesV1Response Results = new ListQueriesV1Response(await Response.Content.ReadAsStringAsync());
                return Results;
            }
        }

        /// <summary>
        /// If you are looking to gather very detailed statistics about a
        /// query, this is the service you would call.
        /// </summary>
        /// <param name="queryId">The id of the query to retrieve details about.</param>
        /// <returns>Information about the specified query.</returns>
        public async Task<GetQueryV1Response> GetQuery(string queryId)
        {
            HttpClient LocalClient = (this.Configuration.IgnoreSslErrors) ? this.IgnoreSslErrorClient : this.NormalClient;

            Uri Path = this.BuildUri($"/query/{queryId}");

            HttpRequestMessage Request = this.BuildRequest(Path, HttpMethod.Get);

            HttpResponseMessage Response = await LocalClient.GetAsync(Path);

            // Expect a 200 response
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new PrestoWebException(await Response.Content.ReadAsStringAsync(), Response.StatusCode);
            }
            else
            {
                GetQueryV1Response Result = new GetQueryV1Response(await Response.Content.ReadAsStringAsync());
                return Result;
            }
        }

        /// <summary>
        /// If you are looking to gather very detailed statistics about a
        /// query, this is the service you would call.
        /// </summary>
        /// <param name="queryId">The id of the query to retrieve details about.</param>
        /// <returns>Information about the specified query.</returns>
        public async Task<GetQueryV1Response> GetQuery(QueryId queryId)
        {
            return await this.GetQuery(queryId.ToString());
        }

        #endregion

        #region Statements

        /// <summary>
        /// Submits a statement to Presto for execution. The Presto client 
        /// executes queries on behalf of a user against a catalog and a schema.
        /// </summary>
        /// <param name="request">The query execution request.</param>
        /// <returns>The resulting response object from the query.</returns>
        public async Task<ExecuteQueryV1Response> ExecuteQueryV1(ExecuteQueryV1Request request)
        {
            // Check the required configuration items before running the query
            if (!CheckConfiguration(out Exception Ex))
            {
                throw Ex;
            }

            // Track all of the incremental results as they are returned
            List<QueryResultsV1> Results = new List<QueryResultsV1>();

            // Choose the correct client to use for ssl errors
            HttpClient LocalClient = (this.Configuration.IgnoreSslErrors) ? this.IgnoreSslErrorClient : this.NormalClient;

            // Build the url path
            Uri Path = this.BuildUri("/statement");

            // Create a new request to post with the query
            HttpRequestMessage Request = this.BuildRequest(Path, HttpMethod.Post, new StringContent(request.Query));

            // Add all of the configured headers to the request
            this.BuildQueryHeaders(ref Request, request.Options);

            // Use the stopwatch to measure if we've exceeded the specified timeout
            Stopwatch SW = new Stopwatch();

            if (this.Configuration.ClientRequestTimeout > 0)
            {
                SW.Start();
            }

            // This is the original submission result, will contain the nextUri
            // property to follow in order to get the results
            HttpResponseMessage ResponseMessage = await this.MakeHttpRequest(LocalClient, Request);

            // This doesn't really do anything but evaluate the headers right now
            this.ProcessResponseHeaders(ResponseMessage);

            string Content = await ResponseMessage.Content.ReadAsStringAsync();

            // If parsing the submission response fails, return and exit
            if (!QueryResultsV1.TryParse(Content, out QueryResultsV1 Response, out Exception ParseEx))
            {
                throw new PrestoException($"The query submission response could not be parsed.", Content, ParseEx);
            }
            else
            {
                // Check to make sure there wasn't an error provided
                if (Response.Error != null)
                {
                    throw new PrestoQueryException(Response.Error);
                }

                Results.Add(Response);

                // Keep track of the last, non-null uri so we can
                // send a delete request to it at the end
                Uri LastUri = Path;

                // Hold the new path to request against which will
                // be updated in the do/while loop
                Path = Response.NextUri;

                try
                {
                    // Keep following the nextUri until it is not sent
                    // When it is absent in the response, the query has
                    // finished, use  a while loop since the data may have 
                    // all been returned in the first request and no nextUri
                    // property was returned
                    while (Path != null && 
                        ((this.Configuration.ClientRequestTimeout > 0) ? 
                        SW.Elapsed.TotalSeconds <= this.Configuration.ClientRequestTimeout : 
                        true 
                        ))
                    {
                        // Put a pause in between each call to reduce CPU usage
                        Thread.Sleep(this.Configuration.CheckInterval);

                        // This is the last non-null uri
                        LastUri = Path;

                        // Make the request and get back a valid response, otherwise
                        // the MakeRequest method will throw an exception
                        Request = BuildRequest(Path, HttpMethod.Get);

                        ResponseMessage = await this.MakeHttpRequest(LocalClient, Request);

                        this.ProcessResponseHeaders(ResponseMessage);

                        Content = await ResponseMessage.Content.ReadAsStringAsync();

                        // Make sure deserialization succeeded
                        if (QueryResultsV1.TryParse(Content, out Response, out ParseEx))
                        {
                            Results.Add(Response);

                            // Check to make sure there wasn't an error provided
                            if (Response.Error != null)
                            {
                                throw new PrestoQueryException(Response.Error);
                            }

                            // Update the path to the returned nextUri (which could be null)
                            Path = Response.NextUri;
                        }
                        else
                        {
                            throw new PrestoException("The response from presto could not be deserialized.", Content, ParseEx);
                        }
                    }

                    bool Closed = false;

                    // Explicitly closes the query
                    ResponseMessage = await LocalClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, LastUri));

                    // If a 204 is not returned, the query was not successfully closed
                    if (ResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        Closed = true;
                    }

                    ExecuteQueryV1Response QueryResponse = new ExecuteQueryV1Response(Results, Closed);

                    return QueryResponse;
                }
                catch (PrestoException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw new PrestoException("Submitting the requested query failed.", e);
                }
            }
        }


        /// <summary>
        /// This API is not yet available in Presto as of version 0.197
        /// Submits a statement to Presto for execution. The Presto client
        /// executes queries on behalf of a user against a catalog and a schema.
        /// </summary>
        /// <param name="request">The query execution request</param>
        /// <returns>
        /// The final query response object where NextUri was null with all of the data
        /// retrieved from the data uris.
        /// </returns>
        private async Task<ExecuteQueryResponse<QueryResultsV2>> ExecuteQueryV2(ExecuteQueryV2Request request)
        {
            #region Standard Stuff

            // Check the required configuration items before running the query
            if (!CheckConfiguration(out Exception Ex))
            {
                throw Ex;
            }

            // Choose the correct client to use for ssl errors
            HttpClient LocalClient = (this.Configuration.IgnoreSslErrors) ? this.IgnoreSslErrorClient : this.NormalClient;

            // Build the url path
            Uri Path = this.BuildUri("/statement", PrestoApiVersion.V2);

            #endregion

            Uri NextUri = Path;
            Uri LastUri = NextUri;

            HashSet<Uri> DataUris = new HashSet<Uri>();
            List<Task> DataResponses = new List<Task>();

            // Each row is represented as a list of dynamic elements, then the 
            // wrapper list if the collection of those rows
            ConcurrentBag<List<dynamic>> DataRows = new ConcurrentBag<List<dynamic>>();
            ExecuteQueryResponse<QueryResultsV2> QueryResponse = null;

            bool FirstRun = true;

            // Each of these requests has to happen in serial because the 
            // next request depends on the results from the last
            while (NextUri != null)
            {
                // Save the last valid NextUri to send the DELETE 
                // request to in order to close the query
                LastUri = NextUri;

                HttpRequestMessage Request;

                if (FirstRun)
                {
                    // Create a new request to post with the query
                    Request = new HttpRequestMessage(HttpMethod.Post, NextUri)
                    {
                        Content = new StringContent(request.Query)
                    };

                    // Add all of the configured headers to the request
                    this.BuildQueryHeaders(ref Request, request.Options);

                    FirstRun = false;
                }
                else
                {
                    Request = new HttpRequestMessage(HttpMethod.Get, NextUri);
                }

                HttpResponseMessage Response = await this.MakeHttpRequest(LocalClient, Request);

                string raw = await Response.Content.ReadAsStringAsync();

                QueryResponse = new ExecuteQueryResponse<QueryResultsV2>(raw);

                if (QueryResponse.DeserializationSucceeded)
                {
                    if (QueryResponse.Response.DataUris != null)
                    {
                        foreach (Uri DataUri in QueryResponse.Response.DataUris)
                        {
                            // If we haven't seen the uri yet, add it and then
                            // kick off a request to grab the data
                            if (!DataUris.Contains(DataUri))
                            {
                                DataUris.Add(DataUri);

                                // Add the response to the concurrent bag collecting the data results
                                DataResponses.Add(Task.Run(async () =>
                                {
                                    Uri NextDataUri = DataUri;

                                    // Client must follow the next uri unti it becomes unavailable
                                    while (NextDataUri != null)
                                    {
                                        HttpRequestMessage DataRequest = new HttpRequestMessage(HttpMethod.Get, NextDataUri);
                                        HttpResponseMessage DataResponse = await LocalClient.SendAsync(DataRequest);
                                        GetDataV2Response DataItem = JsonConvert.DeserializeObject<GetDataV2Response>(await DataResponse.Content.ReadAsStringAsync());

                                        foreach (List<dynamic> Row in DataItem.Data)
                                        {
                                            DataRows.Add(Row);
                                        }

                                        if (DataResponse.Headers.Contains(PrestoHeader.PRESTO_DATA_NEXT_URI.Value))
                                        {
                                            NextDataUri = new Uri(DataResponse.Headers.GetValues(PrestoHeader.PRESTO_DATA_NEXT_URI.Value).First());
                                        }
                                        else
                                        {
                                            NextDataUri = null;
                                        }
                                    }
                                }));
                            }
                        }
                    }

                    NextUri = QueryResponse.Response.NextUri;
                }
                else
                {
                    throw new PrestoException("The response from presto could not be deserialized to follow the nextUri.", QueryResponse.LastError);
                }
            }

            // When these complete, all of the data row responses will have been
            // written to the concurrent bag
            await Task.WhenAll(DataResponses);

            // Write the data from the concurrent bag to a list so that 
            // the data property now holds all of the rows of our response
            QueryResponse.Response.Data = DataRows.ToList();

            // Explicitly closes the query
            HttpResponseMessage ClosureResponse = await LocalClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, LastUri));

            // If a 204 is not returned, the query was not successfully closed
            if (ClosureResponse.StatusCode == HttpStatusCode.NoContent)
            {
                QueryResponse.QueryClosed = true;
            }

            return QueryResponse;
        }

        #endregion

        #region JMX

        public async Task<JmxMbeanV1Response> GetJmxMbean(JmxMbeanV1Request request)
        {
            // Check the required configuration items before running the query
            if (!CheckConfiguration(out Exception Ex))
            {
                throw Ex;
            }

            // Build the url path
            Uri Path = this.BuildUri($"/jmx/mbean/{request.ObjectName}");

            // Create a new request to post with the query
            HttpRequestMessage Request = this.BuildRequest(Path, HttpMethod.Get);

            // Submit the request for details on the requested object name
            HttpResponseMessage Response = await this.MakeHttpRequest(Request);

            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new PrestoWebException(await Response.Content.ReadAsStringAsync(), Response.StatusCode);
            }
            else
            {
                // Generate a new query response object
                return new JmxMbeanV1Response(await Response.Content.ReadAsStringAsync());
            }
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Makes a request with the provided request message, choosing the appropriate http client from the
        /// configuration parameters and returns the response message
        /// </summary>
        /// <param name="client">The http client to use to make the request.</param>
        /// <param name="request">The request to send.</param>
        /// <param name="maxRetries">The maximum number of times the method will try to contact the server
        /// if a service unavailable response code is returned.</param>
        /// <returns>The http response message from the request.</returns>
        private async Task<HttpResponseMessage> MakeHttpRequest(HttpRequestMessage request, uint maxRetries = 5)
        {
            return await this.MakeHttpRequest(this.Configuration.IgnoreSslErrors ? this.IgnoreSslErrorClient : this.NormalClient, request, maxRetries);
        }

        /// <summary>
        /// Makes a request with the provided client and request message and
        /// returns the response message
        /// </summary>
        /// <param name="client">The http client to use to make the request.</param>
        /// <param name="request">The request to send.</param>
        /// <param name="maxRetries">The maximum number of times the method will try to contact the server
        /// if a service unavailable response code is returned.</param>
        /// <returns>The http response message from the request.</returns>
        private async Task<HttpResponseMessage> MakeHttpRequest(HttpClient client, HttpRequestMessage request, uint maxRetries = 5)
        {
            HttpResponseMessage Response = null;
            uint Counter = 0;

            while (Counter < maxRetries)
            {
                Response = await client.SendAsync(request);

                switch (Response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                            return Response;
                        }
                    case HttpStatusCode.ServiceUnavailable:
                        {
                            // Retry with an exponential backoff
                            int Milliseconds = (int)Math.Floor(Math.Pow(2, Counter) * 1000);
                            Milliseconds += new Random().Next(0, 1000);
                            Thread.Sleep(Milliseconds);

                            Counter++;

                            break;
                        }
                    default:
                        {
                            throw new PrestoWebException($"The request to {request.RequestUri} failed.", Response.StatusCode);
                        }
                }
            }

            // This will only be reached if the while loop exits
            throw new PrestoWebException($"The maximum number of retries, {maxRetries}, for path {request.RequestUri} was exceeded.", Response.StatusCode);
        }

        /// <summary>
        /// Builds the URI
        /// </summary>
        /// <param name="relativePath">The relative path to append to the base path, not including the version</param>
        /// <param name="version">The version of the API to use, will precede the relative path</param>
        /// <returns></returns>
        private Uri BuildUri(string relativePath, PrestoApiVersion version)
        {
            if (String.IsNullOrEmpty(relativePath))
            {
                throw new ArgumentNullException("relativePath", "The relative path in the url being constructed cannot be null or empty.");
            }

            StringBuilder SB = new StringBuilder();

            string Scheme = (this.Configuration.UseSsl) ? "https" : "http";
            SB.Append($"{Scheme}://{this.Configuration.Host}");

            // Only add non-standard ports
            if ((Scheme == "http" && this.Configuration.Port != 80) || (Scheme == "https" && this.Configuration.Port != 443))
            {
                SB.Append($":{this.Configuration.Port}");
            }

            if (!relativePath.StartsWith("/"))
            {
                relativePath = $"/{relativePath}";
            }

            SB.Append($"/{this.GetVersionString(version)}{relativePath}");

            Uri Path = new Uri(SB.ToString());

            return Path;
        }

        /// <summary>
        /// Builds the URI. The version in the path default to the client configuration
        /// </summary>
        /// <param name="relativePath">The relative path to append to the base path, not including the version.</param>
        /// <returns></returns>
        private Uri BuildUri(string relativePath)
        {
            return BuildUri(relativePath, this.Configuration.Version);
        }

        /// <summary>
        /// Checks the current configuration 
        /// </summary>
        /// <param name="ex">The exception to throw if a parameter is not configured correctly</param>
        /// <returns>True if the configuration is consistent, false if not</returns>
        private bool CheckConfiguration(out Exception ex)
        {
            if (String.IsNullOrEmpty(this.Configuration.User))
            {
                ex = new ArgumentNullException("user", "The user was not specified.");
                return false;
            }

            ex = null;
            return true;
        }

        /// <summary>
        /// Adds the session level and query specific options as headers to the HttpRequestMessage
        /// </summary>
        /// <param name="request">The HttpRequestMessage that will have headers added to it</param>
        /// <param name="options">The set of query specific options</param>
        private void BuildQueryHeaders(ref HttpRequestMessage request, QueryOptions options = null)
        {
            // Defaults that will remain static
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Timezone will always be set
            request.Headers.Add(PrestoHeader.PRESTO_TIME_ZONE.Value, this.Configuration.TimeZone.Id);

            // Catalog
            if (!String.IsNullOrEmpty(this.Configuration.Catalog))
            {
                request.Headers.Add(PrestoHeader.PRESTO_CATALOG.Value, this.Configuration.Catalog);
            }

            // Schema
            if (!String.IsNullOrEmpty(this.Configuration.Schema))
            {
                request.Headers.Add(PrestoHeader.PRESTO_SCHEMA.Value, this.Configuration.Schema);
            }

            // ClientInfo
            if (!String.IsNullOrEmpty(this.Configuration.ClientInfo))
            {
                request.Headers.Add(PrestoHeader.PRESTO_CLIENT_INFO.Value, this.Configuration.ClientInfo);
            }

            // Language
            if (this.Configuration.Locale != null)
            {
                request.Headers.Add(PrestoHeader.PRESTO_LANGUAGE.Value, this.Configuration.Locale.Name);
            }

            // Session properties
            if (this.Configuration.Properties != null)
            {
                foreach (KeyValuePair<string, string> Property in this.Configuration.Properties)
                {
                    request.Headers.Add(PrestoHeader.PRESTO_SESSION.Value, $"{Property.Key}={Property.Value}");
                }
            }

            // Build final set of prepared statements
            IDictionary<string, string> PreparedStatements = new Dictionary<string, string>();

            if (this.Configuration.PreparedStatements != null)
            {
                foreach (KeyValuePair<string, string> Item in this.Configuration.PreparedStatements)
                {
                    PreparedStatements.Add(Item);
                }
            }

            // Build final set of tags
            HashSet<string> Tags = new HashSet<string>();

            if (this.Configuration.ClientTags != null)
            {
                // Client tags are not allowed to have commas in them and have already been checked in the setter
                foreach (string Tag in this.Configuration.ClientTags)
                {
                    Tags.Add(Tag);
                }
            }

            if (options != null)
            {
                // Add query specific client tags
                if (options.ClientTags != null)
                {
                    foreach (string Tag in options.ClientTags)
                    {
                        if (!Tags.Contains(Tag))
                        {
                            Tags.Add(Tag);
                        }
                    }
                }

                // Add query specific prepared statements
                if (options.PreparedStatements != null)
                {
                    foreach (KeyValuePair<string, string> Statement in options.PreparedStatements)
                    {
                        if (!PreparedStatements.ContainsKey(Statement.Key))
                        {
                            PreparedStatements.Add(Statement);
                        }
                    }
                }

                // Add query specific session properties
                if (options.Properties != null)
                {
                    foreach (KeyValuePair<string, string> Property in options.Properties)
                    {
                        request.Headers.Add(PrestoHeader.PRESTO_SESSION.Value, $"{Property.Key}={Property.Value}");
                    }
                }

                if (!String.IsNullOrEmpty(options.TransactionId))
                {
                    request.Headers.Add(PrestoHeader.PRESTO_TRANSACTION_ID.Value, options.TransactionId);
                }
                else
                {
                    request.Headers.Add(PrestoHeader.PRESTO_TRANSACTION_ID.Value, "NONE");
                }
            }

            // If any session or query prepared statements were provided, add them
            if (PreparedStatements.Any())
            {
                foreach (KeyValuePair<string, string> Statement in PreparedStatements)
                {
                    request.Headers.Add(PrestoHeader.PRESTO_PREPARED_STATEMENT.Value, $"{HttpUtility.UrlDecode(Statement.Key)}={HttpUtility.UrlDecode(Statement.Value)}");
                }
            }

            // If any session or query client tags were provided, add tem
            if (Tags.Any())
            {
                request.Headers.Add(PrestoHeader.PRESTO_CLIENT_TAGS.Value, String.Join(",", Tags));
            }
        }

        /// <summary>
        /// Gets the string representation of the PrestoApiVersion
        /// </summary>
        /// <param name="version">The presto api version to get the string value of</param>
        /// <returns>The api version string</returns>
        private string GetVersionString(PrestoApiVersion version)
        {
            return version.GetType().GetMember(version.ToString()).FirstOrDefault().GetCustomAttribute<DescriptionAttribute>().Description;
        }

        /// <summary>
        /// Builds a new HttpRequestMessage with basic common headers,
        /// User-Agent, Accept, X-Presto-Source, and X-Presto-User
        /// </summary>
        /// <param name="url">The url for the request</param>
        /// <param name="method">The Http method</param>
        /// <param name="content">Any content included with the request</param>
        /// <returns></returns>
        private HttpRequestMessage BuildRequest(Uri url, HttpMethod method, HttpContent content = null)
        {
            HttpRequestMessage Request = new HttpRequestMessage(method, url);

            if (content != null)
            {
                Request.Content = content;
            }

            Request.Headers.Add("Accept", "application/json");
            Request.Headers.Add("User-Agent", $"bamcis_presto_dotnet_core_sdk/{AssemblyVersion}");
            Request.Headers.Add(PrestoHeader.PRESTO_SOURCE.Value, "bamcis_presto_dotnet_core_sdk");

            if (!String.IsNullOrEmpty(this.Configuration.User))
            {
                Request.Headers.Add(PrestoHeader.PRESTO_USER.Value, this.Configuration.User);
            }

            return Request;
        }

        /// <summary>
        /// Processes the http response headers for values supplied from presto
        /// </summary>
        /// <param name="response">The http response message from presto</param>
        /// <returns>The collection of values that were set by presto in the http headers</returns>
        private ResponseHeaderCollection ProcessResponseHeaders(HttpResponseMessage response)
        {
            return new ResponseHeaderCollection(response.Headers);
        }

        #endregion
    }
}