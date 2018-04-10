using System.ComponentModel;

namespace BAMCIS.PrestoClient.Model.Statement
{
    /// <summary>
    /// Indicates which version of the statement client API is being
    /// used
    /// </summary>
    public enum StatementApiVersion
    {
        /// <summary>
        /// The current version of the statement API
        /// </summary>
        [Description("v1")]
        V1,

        /// <summary>
        /// The experimental new version, not yet being used
        /// </summary>
        [Description("v2")]
        V2
    }
}