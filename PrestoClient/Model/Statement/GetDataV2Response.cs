using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Statement
{
    /// <summary>
    /// Represents the response from the server when requesting
    /// data from the V2 statement api through the dataUri response
    /// </summary>
    public class GetDataV2Response
    {
        #region Public Properties

        /// <summary>
        /// The array of data items returned from the query
        /// </summary>
        public IEnumerable<List<dynamic>> Data { get; set; }

        #endregion
    }
}