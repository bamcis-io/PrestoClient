using BAMCIS.PrestoClient;
using BAMCIS.PrestoClient.Interfaces;
using BAMCIS.PrestoClient.Model.Query;
using BAMCIS.PrestoClient.Model.Server;
using BAMCIS.PrestoClient.Model.Statement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PrestoClient.Tests
{
    public class PrestoClientDriver
    {
        private static string Schema = "cars";
        private static string S3_Location = "";

        public PrestoClientDriver()
        { }

        [Fact]
        public async Task CreateSchema()
        {
            // ARRANGE
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig()
            {
                Host = "localhost",
                Port = 8080
            };

            IPrestoClient Client = new PrestodbClient(Config);

            ExecuteQueryV1Request Req = new ExecuteQueryV1Request($"CREATE SCHEMA IF NOT EXISTS hive.{Schema}");

            // ACT
            ExecuteQueryV1Response Res = await Client.ExecuteQueryV1(Req);

            // ASSERT
            Assert.True(Res.QueryClosed == true);
        }

        [Fact]
        public async Task CreateTable()
        {
            // ARRANGE
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig("hive", Schema)
            {
                Host = "localhost",
                Port = 8080
            };

            IPrestoClient Client = new PrestodbClient(Config);

            ExecuteQueryV1Request Req = new ExecuteQueryV1Request($"CREATE TABLE IF NOT EXISTS tracklets (id bigint, objectclass varchar, length double, trackdata array(varchar), platform varchar,spectrum varchar, timestamp bigint) WITH (format = 'AVRO', external_location = '{S3_Location}');");

            // ACT
            ExecuteQueryV1Response Res = await Client.ExecuteQueryV1(Req);

            // ASSERT
            Assert.True(Res.QueryClosed == true);
        }

        [Fact]
        public async Task TestExecuteStatement()
        { 
            //ARRANGE
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig("hive", Schema) {
                Host = "localhost",
                Port = 8080
            };
            IPrestoClient Client = new PrestodbClient(Config);

            ExecuteQueryV1Request Req = new ExecuteQueryV1Request("select * from tracklets limit 5;");

            // ACT
            ExecuteQueryV1Response Res = await Client.ExecuteQueryV1(Req);

            // ASSERT
            Assert.True(Res.QueryClosed == true);
        }

        [Fact]
        public async Task TestExecuteStatementOrderBy()
        {
            //ARRANGE
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig("hive", Schema)
            {
                Host = "localhost",
                Port = 8080
            };
            IPrestoClient Client = new PrestodbClient(Config);

            ExecuteQueryV1Request Req = new ExecuteQueryV1Request("select * from tracklets ORDER BY length limit 5;");

            // ACT
            ExecuteQueryV1Response Res = await Client.ExecuteQueryV1(Req);

            // ASSERT
            Assert.True(Res.QueryClosed == true);
        }

        [Fact]
        public async Task TestExecuteStatementWhere()
        {
            //ARRANGE
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig("hive", Schema)
            {
                Host = "localhost",
                Port = 8080
            };
            IPrestoClient Client = new PrestodbClient(Config);

            ExecuteQueryV1Request Req = new ExecuteQueryV1Request("select id,length,objectclass from tracklets WHERE length > 1000 LIMIT 5;");

            // ACT
            ExecuteQueryV1Response Res = await Client.ExecuteQueryV1(Req);

            // ASSERT
            Assert.True(Res.QueryClosed == true);
        }

        [Fact]
        public async Task TestQueryResultDataToJson()
        {
            //ARRANGE
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig("hive", Schema)
            {
                Host = "localhost",
                Port = 8080
            };
            IPrestoClient Client = new PrestodbClient(Config);

            ExecuteQueryV1Request Req = new ExecuteQueryV1Request("select * from tracklets limit 5;");

            // ACT
            ExecuteQueryV1Response Res = await Client.ExecuteQueryV1(Req);
          
            string Json = Res.DataToJson();

            // ASSERT
            Assert.True(Res.QueryClosed == true && !String.IsNullOrEmpty(Json));
        }

        [Fact]
        public async Task TestQueryResultDataToCsv()
        {
            //ARRANGE
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig("hive", Schema)
            {
                Host = "localhost",
                Port = 8080
            };
            IPrestoClient Client = new PrestodbClient(Config);

            ExecuteQueryV1Request Req = new ExecuteQueryV1Request("select * from tracklets limit 5;");

            // ACT
            ExecuteQueryV1Response Res = await Client.ExecuteQueryV1(Req);

            string Csv = String.Join("\n", Res.DataToCsv());

            // ASSERT
            Assert.True(Res.QueryClosed == true && !String.IsNullOrEmpty(Csv));
        }

        [Fact]
        public async Task TestListQueries()
        {
            //ARRANGE
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig("hive", Schema)
            {
                Host = "localhost",
                Port = 8080
            };
            IPrestoClient Client = new PrestodbClient(Config);

            // ACT
            ListQueriesV1Response Res = await Client.GetQueries();

            // ASSERT
            Assert.True(Res != null && Res.DeserializationSucceeded);
        }

        [Fact]
        public async Task TestGetQuery()
        {
            //ARRANGE
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig("hive", Schema)
            {
                Host = "localhost",
                Port = 8080
            };
            IPrestoClient Client = new PrestodbClient(Config);

            // ACT
            ListQueriesV1Response Res = await Client.GetQueries();

            List<GetQueryV1Response> Info = new List<GetQueryV1Response>();

            foreach (BasicQueryInfo Item in Res.QueryInfo)
            {
                GetQueryV1Response QRes = await Client.GetQuery(Item.QueryId);
                Info.Add(QRes);
            }

            // ASSERT
            Assert.True(Res != null && Info.All(x => x.DeserializationSucceeded == true));
        }
    }
}
