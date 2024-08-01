using System;
using System.Collections.Generic;
using System.Text;

namespace BAMCIS.PrestoClient.Model.Statement
{
    public class GetNextUriV1Request
    {
        /// <summary>
        /// The API version being used for this request.
        /// </summary>
        public StatementApiVersion ApiVersion { get; }

        public Uri NextUri { get; }

        /// <summary>
        /// Creates a new request for next URI given the previous call's response
        /// </summary>
        /// <param name="query">The query statement to execute.</param>
        public GetNextUriV1Request(QueryResultsV1 previousResults)
        {
            if (previousResults == null)
            {
                throw new ArgumentNullException(nameof(previousResults), "The previousResults cannot be null.");
            }

            this.NextUri = previousResults.NextUri;
            this.ApiVersion = StatementApiVersion.V1;
        }
    }
}
