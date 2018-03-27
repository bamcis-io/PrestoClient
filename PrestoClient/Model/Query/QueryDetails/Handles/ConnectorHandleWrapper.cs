namespace BAMCIS.PrestoClient.Model.Query.QueryDetails.Handles
{
    public class ConnectorHandleWrapper
    {
        /// <summary>
        /// This structure tends to be very dynamic in the json serialization
        /// </summary>
        public dynamic ConnectorHandle { get; set; }
    }
}