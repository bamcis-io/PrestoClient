using System;
using System.Collections.Generic;
using System.Text;

namespace BAMCIS.PrestoClient.Model.Statement
{
    public class DeleteLastUriV1Request
    {
        /// <summary>
        /// The API version being used for this request.
        /// </summary>
        public StatementApiVersion ApiVersion { get; }

        public Uri LastUri { get; }

        /// <summary>
        /// Creates a new request for next URI given the previous call's response
        /// </summary>
        /// <param name="query">The query statement to execute.</param>
        public DeleteLastUriV1Request(Uri lastUri)
        {
            if (lastUri == null)
            {
                throw new ArgumentNullException(nameof(lastUri), "The lastUri cannot be null.");
            }

            this.LastUri = lastUri;
            this.ApiVersion = StatementApiVersion.V1;
        }
    }
}
