namespace BAMCIS.PrestoClient.Model.Query.QueryDetails.Handles
{
    public class RemoteHandle : Handle
    {
        public string Partitioning { get; set; }

        public string Function { get; set; }
    }
}