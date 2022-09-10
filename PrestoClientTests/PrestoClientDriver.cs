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

        public PrestoClientDriver()
        { }

        private string S3Location(string table){
            string bucket = Environment.GetEnvironmentVariable("S3_BUCKET") ?? "bucket";
            string prefix = Environment.GetEnvironmentVariable("S3_PREFIX") ?? "prefix";
            return $"s3a://{bucket}/{prefix}/{table}";
        }

        private PrestoClientSessionConfig GetConfigBasic()
        {
            return new PrestoClientSessionConfig()
            {
                Host = Environment.GetEnvironmentVariable("HOST") ?? "localhost",
                Port = 8080,
            };
        }

        private PrestoClientSessionConfig GetConfig()
        {
            PrestoClientSessionConfig basic = GetConfigBasic();
            basic.Catalog = "hive";
            basic.Schema = Schema;

            return basic;
        }

        [Fact]
        public async Task TestPassword()
        {
            // ARRANGE
            PrestoClientSessionConfig config = GetConfigBasic();
            config.Password = "password1!2@3#4$AA";

            IPrestoClient client = new PrestodbClient(config);

            // ACT
            ExecuteQueryV1Request req = new ExecuteQueryV1Request($"CREATE SCHEMA IF NOT EXISTS hive.{Schema}");

            //mock.Verify(x => x.ExecuteQueryV1(captor.Capture()));
            //req.
            
            ExecuteQueryV1Response res = await client.ExecuteQueryV1(req);

            // ASSERT
            Assert.NotNull(res);
        }

        [Fact]
        public async Task CreateSchema()
        {
            // ARRANGE
            PrestoClientSessionConfig Config = GetConfigBasic();

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
            PrestoClientSessionConfig Config = GetConfig();

            IPrestoClient Client = new PrestodbClient(Config);

            ExecuteQueryV1Request Req = new ExecuteQueryV1Request($"CREATE TABLE IF NOT EXISTS tracklets (id bigint, objectclass varchar, length double, trackdata array(varchar), platform varchar,spectrum varchar, timestamp bigint) WITH (format = 'AVRO', external_location = '{S3Location("tracklets")}');");

            // ACT
            ExecuteQueryV1Response Res = await Client.ExecuteQueryV1(Req);

            // ASSERT
            Assert.True(Res.QueryClosed == true);
        }

        [Fact]
        public async Task TestExecuteStatement()
        { 
            //ARRANGE
            PrestoClientSessionConfig Config = GetConfig();
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
            PrestoClientSessionConfig Config = GetConfig();
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
            PrestoClientSessionConfig Config = GetConfig();
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
            PrestoClientSessionConfig Config = GetConfig();
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
            PrestoClientSessionConfig Config = GetConfig();
            IPrestoClient Client = new PrestodbClient(Config);

            ExecuteQueryV1Request Req = new ExecuteQueryV1Request("select * from tracklets limit 5;");

            // ACT
            ExecuteQueryV1Response Res = await Client.ExecuteQueryV1(Req);

            string Csv = String.Join("\n", Res.DataToCsv());
            Console.WriteLine(Csv);

            // ASSERT
            Assert.True(Res.QueryClosed == true && !String.IsNullOrEmpty(Csv));
        }

        [Fact]
        public async Task TestListQueries()
        {
            //ARRANGE
            PrestoClientSessionConfig Config = GetConfig();
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
            PrestoClientSessionConfig Config = GetConfig();
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
