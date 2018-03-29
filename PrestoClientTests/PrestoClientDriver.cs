using BAMCIS.PrestoClient;
using BAMCIS.PrestoClient.Interfaces;
using BAMCIS.PrestoClient.Model.Execution;
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
        public PrestoClientDriver()
        { }

        [Fact]
        public async Task TestExecuteStatement()
        {
            //ARRANGE
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig("hive", "maven") {
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
        public async Task TestQueryResultDataToJson()
        {
            //ARRANGE
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig("hive", "maven")
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
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig("hive", "maven")
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
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig("hive", "maven")
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
            PrestoClientSessionConfig Config = new PrestoClientSessionConfig("hive", "maven")
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
