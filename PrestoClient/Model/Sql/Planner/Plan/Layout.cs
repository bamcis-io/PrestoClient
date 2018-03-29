namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    public class Layout
    {
        public string ConnectorId { get; set; }

        /// <summary>
        /// This property is dynamic in the json serialization
        /// 
        /// "transactionHandle": {
        ///     "@type": "$info_schema",
        ///     "transactionId": "54d294a9-02c8-4ff6-9712-a14d9146a46e"
        /// }
        /// 
        /// "transactionHandle": {
        ///     "@type": "hive-hadoop2",
        ///     "uuid": "bdca2d49-c2e6-4090-87b2-c903a847adf5"
        /// }
        /// 
        /// </summary>
        public dynamic TransactionHandle { get; set; }

        /// <summary>
        /// This property is dynamic in the json serialization
        /// 
        /// "connectorHandle": {
        ///     "@type": "hive-hadoop2",
        ///     "schemaTableName": {
        ///       "schema": "sch",
        ///       "table": "test"
        ///     },
        ///     "partitionColumns": [],
        ///     "compactEffectivePredicate": { "columnDomains": [] },
        ///     "promisedPredicate": {}
        /// }
        /// 
        /// "connectorHandle": {
        ///     "@type": "$info_schema",
        ///     "table": {
        ///       "@type": "$info_schema",
        ///       "catalogName": "hive",
        ///       "schemaName": "information_schema",
        ///       "tableName": "tables"
        ///     },
        ///     "constraint": {
        ///       "columnDomains": [
        ///         {
        ///           "column": {
        ///             "@type": "$info_schema",
        ///             "columnName": "table_schema"
        ///           },
        ///           "domain": {
        ///             "values": {
        ///               "@type": "sortable",
        ///               "type": "varchar",
        ///               "ranges": [
        ///                 {
        ///                   "low": {
        ///                     "type": "varchar",
        ///                     "valueBlock": "DgAAAFZBUklBQkxFX1dJRFRIAQAAAAUAAAAABQAAAG1hdmVu",
        ///                     "bound": "EXACTLY"
        ///                   },
        ///                   "high": {
        ///                     "type": "varchar",
        ///                     "valueBlock": "DgAAAFZBUklBQkxFX1dJRFRIAQAAAAUAAAAABQAAAG1hdmVu",
        ///                     "bound": "EXACTLY"
        ///                   }
        ///                 }
        ///               ]
        ///             },
        ///             "nullAllowed": false
        ///           }
        ///         }
        ///       ]
        ///     }
        ///   }
        /// </summary>
        public dynamic ConnectorHandle { get; set; }
    }
}
