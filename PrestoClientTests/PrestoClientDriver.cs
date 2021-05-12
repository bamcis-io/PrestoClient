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

        [Fact]
        public async Task TestPassword()
        {
            // ARRANGE
            PrestoClientSessionConfig config = new PrestoClientSessionConfig()
            {
                Host = "localhost",
                Port = 8080,
                Password = "password1!2@3#4$AA"
            };

            IPrestoClient client = new PrestodbClient(config);

            // ACT
            ExecuteQueryV1Request req = new ExecuteQueryV1Request($"CREATE SCHEMA IF NOT EXISTS memory.{Schema}");

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
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig()
            {
                Host = "localhost",
                Port = 8080,
            };

            IPrestoClient Client = new PrestodbClient(Config);

            ExecuteQueryV1Request Req = new ExecuteQueryV1Request($"CREATE SCHEMA IF NOT EXISTS memory.{Schema}");

            // ACT
            ExecuteQueryV1Response Res = await Client.ExecuteQueryV1(Req);

            // ASSERT
            Assert.True(Res.QueryClosed == true);
        }

        [Fact]
        public async Task CreateTable()
        {
            // ARRANGE
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig("memory", Schema)
            {
                Host = "localhost",
                Port = 8080
            };

            IPrestoClient Client = new PrestodbClient(Config);

            ExecuteQueryV1Request Req = new ExecuteQueryV1Request($"CREATE TABLE IF NOT EXISTS tracklets (id bigint, objectclass varchar, length double, trackdata array(varchar), platform varchar,spectrum varchar, timestamp bigint);");

            // ACT
            ExecuteQueryV1Response Res = await Client.ExecuteQueryV1(Req);

            // ASSERT
            Assert.True(Res.QueryClosed == true);
        }

        [Fact]
        public async Task TestExecuteStatement()
        { 
            //ARRANGE
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig("tpch", Schema) {
                Host = "localhost",
                Port = 8080,
            };
            IPrestoClient Client = new PrestodbClient(Config);

            ExecuteQueryV1Request Req = new ExecuteQueryV1Request("SELECT * FROM tiny.region");

            // ACT
            ExecuteQueryV1Response Res = await Client.ExecuteQueryV1(Req);

            // ASSERT
            Assert.True(Res.QueryClosed == true);
        }

        [Fact]
        public async Task TestExecuteStatementOrderBy()
        {
            //ARRANGE
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig("tpch", Schema)
            {
                Host = "localhost",
                Port = 8080
            };
            IPrestoClient Client = new PrestodbClient(Config);

            ExecuteQueryV1Request Req = new ExecuteQueryV1Request("SELECT * FROM tiny.customer ORDER BY name limit 5");

            // ACT
            ExecuteQueryV1Response Res = await Client.ExecuteQueryV1(Req);

            // ASSERT
            Assert.True(Res.QueryClosed == true);
        }

        [Fact]
        public async Task TestExecuteStatementWhere()
        {
            //ARRANGE
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig("tpch", Schema)
            {
                Host = "localhost",
                Port = 8080
            };
            IPrestoClient Client = new PrestodbClient(Config);

            ExecuteQueryV1Request Req = new ExecuteQueryV1Request("select custkey, name, address from tiny.customer WHERE custkey > 500 LIMIT 5;");

            // ACT
            ExecuteQueryV1Response Res = await Client.ExecuteQueryV1(Req);

            // ASSERT
            Assert.True(Res.QueryClosed == true);
        }

        [Fact]
        public async Task TestQueryResultDataToJson()
        {
            //ARRANGE
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig("tpch", Schema)
            {
                Host = "localhost",
                Port = 8080
            };
            IPrestoClient Client = new PrestodbClient(Config);

            ExecuteQueryV1Request Req = new ExecuteQueryV1Request("SELECT * FROM tiny.region");

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
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig("tpch", Schema)
            {
                Host = "localhost",
                Port = 8080
            };
            IPrestoClient Client = new PrestodbClient(Config);

            ExecuteQueryV1Request Req = new ExecuteQueryV1Request("SELECT * FROM tiny.region");

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
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig("tpch", Schema)
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
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig("tpch", Schema)
            {
                Host = "localhost",
                Port = 8080,
                User = "test"
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
