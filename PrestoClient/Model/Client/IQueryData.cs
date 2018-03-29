using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Client
{
    /// <summary>
    /// Represents data returned by query
    /// </summary>
    public interface IQueryData
    {
        /// <summary>
        /// The array of data items returned from the query
        /// </summary>
        IEnumerable<List<dynamic>> GetData();
    }
}
