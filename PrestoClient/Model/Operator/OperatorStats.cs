using Newtonsoft.Json;
using BAMCIS.PrestoClient.Serialization;
using System;
using BAMCIS.PrestoClient.Model.Sql.Planner.Plan;
using BAMCIS.PrestoClient.Model.Execution.Scheduler;

namespace BAMCIS.PrestoClient.Model.Operator
{
    /// <summary>
    /// From com.facebook.presto.operator.OperatorStats.java
    /// </summary>
    public class OperatorStats
    {
        #region Public Properties

        public int PipelineId { get; set; }

        public int OperatorId { get; set; }

        public PlanNodeId PlanNodeId { get; set; }

        public string OperatorType { get; set; }

        public long TotalDrivers { get; set; }

        public long AddInputCalls { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan AddInputWall { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan AddInputCpu { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan AddInputUser { get; set; }

        public DataSize InputDataSize { get; set; }

        public long InputPositions { get; set; }

        public double SumSquaredInputPositions { get; set; }

        public long GetOutputCalls { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan GetOutputWall { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan GetOutputCpu { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan GetOutputUser { get; set; }

        public DataSize OutputDataSize { get; set; }

        public long OutputPositions { get; set; }

        public DataSize PhysicalWrittenDataSize { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan BlockedWall { get; set; }

        public long FinishCalls { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan FinishWall { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan FinishCpu { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan FinishUser { get; set; }

        public string UserMemoryReservation { get; set; }

        public string RevocableMemoryReservation { get; set; }

        public string SystemMemoryReservation { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public BlockedReason BlockedReason { get; set; }

        /// <summary>
        /// This property is only present with certain types of
        /// operator summaries, like a TableScanOperator. The value takes on many different
        /// forms in the serialized Json, for example:
        /// 
        /// "info": {
        ///   "@type": "splitOperator",
        ///   "splitInfo": {
        ///     "path": "s3a://presto-test-haken/test/test.avro",
        ///     "start": 0,
        ///     "length": 218,
        ///     "fileSize": 218,
        ///     "hosts": [ "localhost" ],
        ///     "database": "db",
        ///     "table": "test",
        ///     "forceLocalScheduling": false,
        ///     "partitionName": "<UNPARTITIONED>"
        ///   }
        /// }
        /// 
        /// As well as
        /// "info": {
        ///   "@type": "splitOperator",
        ///   "splitInfo": {
        ///     "@type": "$info_schema",
        ///     "tableHandle": {
        ///       "@type": "$info_schema",
        ///       "catalogName": "hive",
        ///       "schemaName": "information_schema",
        ///       "tableName": "tables"
        ///     },
        ///     "filters": {
        ///       "table_schema": {
        ///         "serializable": {
        ///           "type": "varchar",
        ///           "block": "DgAAAFZBUklBQkxFX1dJRFRIAQAAAAUAAAAABQAAAG1hdmVu"
        ///         }
        ///       }
        ///     },
        ///     "addresses": [ "172.25.0.3:8080" ]
        ///   }
        /// }
        /// 
        /// Thus, this property is deserialized as dynamic.
        /// 
        /// The actual property type is OperatorInfo, which is a wrapper for
        /// the JSON subtypes in Java
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic Info { get; set; }

        #endregion

        #region Constructors


        #endregion
    }
}