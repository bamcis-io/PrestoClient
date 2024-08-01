using AutoFixture;
using BAMCIS.PrestoClient;
using BAMCIS.PrestoClient.Model;
using BAMCIS.PrestoClient.Model.Client;
using BAMCIS.PrestoClient.Model.Statement;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Xunit.Abstractions;

namespace PrestoClient.DevTest
{
    public class PrestoClientManualTest
    {
        private static readonly string TRINO_HOST = "TRINO_HOST";
        private static readonly string TRINO_PORT = "TRINO_PORT";
        private static readonly string TRINO_USER = "TRINO_USER";
        private static readonly string TRINO_PASSWORD = "TRINO_PASSWORD";
        private const int FULL_RESULT_SET_SIZE = 100000;

        private readonly Fixture _fixture;
        private readonly IConfiguration _configuration;
        private readonly ITestOutputHelper _testOutputHelper;

        public PrestoClientManualTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _fixture = new Fixture();
            _configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.TEST.json", optional: false, reloadOnChange: true)
                        .Build();
        }

        [Fact]
        public async Task TestFullResultQuery()
        {
            // Given
            var trinoHost = _configuration.GetValue<string>(TRINO_HOST);
            var trinoPort = _configuration.GetValue<int>(TRINO_PORT);
            var trinoUser = _configuration.GetValue<string>(TRINO_USER);
            var trinoPass = _configuration.GetValue<string>(TRINO_PASSWORD);
            var client = new PrestodbClient(new PrestoClientSessionConfig()
            {
                User = trinoUser,
                Password = trinoPass,
                Host = trinoHost,
                Port = trinoPort,
                UseSsl = true,
            });
            var request = new ExecuteQueryV1Request($"select * from hive.testschema.testdata limit {FULL_RESULT_SET_SIZE}");

            // When
            _testOutputHelper.WriteLine("start");

            var response = await client.ExecuteQueryV1(request);
            _testOutputHelper.WriteLine(response.DataToJson());

            _testOutputHelper.WriteLine("end");

            // Then
            Assert.True(true);
        }

        [Fact]
        public async Task TestBatchedQuery()
        {
            // Given
            var trinoHost = _configuration.GetValue<string>(TRINO_HOST);
            var trinoPort = _configuration.GetValue<int>(TRINO_PORT);
            var trinoUser = _configuration.GetValue<string>(TRINO_USER);
            var trinoPass = _configuration.GetValue<string>(TRINO_PASSWORD);
            var client = new PrestodbClient(new PrestoClientSessionConfig()
            {
                User = trinoUser,
                Password = trinoPass,
                Host = trinoHost,
                Port = trinoPort,
                UseSsl = true,
            });
            var request = new ExecuteQueryV1Request($"select * from hive.testschema.testdata limit {FULL_RESULT_SET_SIZE}");

            // When
            _testOutputHelper.WriteLine("start");

            using var response = await client.ExecuteQueryV1Batched(request);
            int i = 0;
            int resultsCount = 0;
            await foreach (var resultsBatch in response.GetBatchesAsync())
            {
                _testOutputHelper.WriteLine($"response batch number {i}");
                _testOutputHelper.WriteLine(resultsBatch.DataToJson() ?? "");
                resultsCount += resultsBatch.Data.Count();
                ++i;
            }

            _testOutputHelper.WriteLine("end");

            // Then
            Assert.Equal(FULL_RESULT_SET_SIZE, resultsCount);
            Assert.True(response.QueryClosed);
        }

        [Fact]
        public async Task TestFullResultQuery_SilentMemoryTest()
        {
            // Given
            var trinoHost = _configuration.GetValue<string>(TRINO_HOST);
            var trinoPort = _configuration.GetValue<int>(TRINO_PORT);
            var trinoUser = _configuration.GetValue<string>(TRINO_USER);
            var trinoPass = _configuration.GetValue<string>(TRINO_PASSWORD);
            var client = new PrestodbClient(new PrestoClientSessionConfig()
            {
                User = trinoUser,
                Password = trinoPass,
                Host = trinoHost,
                Port = trinoPort,
                UseSsl = true,
            });
            var request = new ExecuteQueryV1Request($"select * from hive.testschema.testdata limit {FULL_RESULT_SET_SIZE}");

            // When
            _testOutputHelper.WriteLine("start");

            var response = await client.ExecuteQueryV1(request);
            response.DataToJson(); // Run a ToJson but don't output into the "test helper" buffer to help assess memory.

            _testOutputHelper.WriteLine("end");

            // Then
            Assert.True(true);
        }

        [Fact]
        public async Task TestBatchedQuery_SilentMemoryTest()
        {
            // Given
            var trinoHost = _configuration.GetValue<string>(TRINO_HOST);
            var trinoPort = _configuration.GetValue<int>(TRINO_PORT);
            var trinoUser = _configuration.GetValue<string>(TRINO_USER);
            var trinoPass = _configuration.GetValue<string>(TRINO_PASSWORD);
            var client = new PrestodbClient(new PrestoClientSessionConfig()
            {
                User = trinoUser,
                Password = trinoPass,
                Host = trinoHost,
                Port = trinoPort,
                UseSsl = true,
            });
            var request = new ExecuteQueryV1Request($"select * from hive.testschema.testdata limit {FULL_RESULT_SET_SIZE}");

            // When
            _testOutputHelper.WriteLine("start");

            using var response = await client.ExecuteQueryV1Batched(request);
            int i = 0;
            await foreach (var resultsBatch in response.GetBatchesAsync())
            {
                resultsBatch.DataToJson(); // Run a ToJson but don't output into the "test helper" buffer to help assess memory.
                ++i;
            }

            _testOutputHelper.WriteLine("end");

            // Then
            Assert.True(true);
        }

        [Fact]
        public async Task TestBatchedQuery_ZeroRowsReturned()
        {
            // Given
            var trinoHost = _configuration.GetValue<string>(TRINO_HOST);
            var trinoPort = _configuration.GetValue<int>(TRINO_PORT);
            var trinoUser = _configuration.GetValue<string>(TRINO_USER);
            var trinoPass = _configuration.GetValue<string>(TRINO_PASSWORD);
            var client = new PrestodbClient(new PrestoClientSessionConfig()
            {
                User = trinoUser,
                Password = trinoPass,
                Host = trinoHost,
                Port = trinoPort,
                UseSsl = true,
            });
            var request = new ExecuteQueryV1Request($"select * from hive.testschema.testdata where 1 = 0 limit {FULL_RESULT_SET_SIZE}");

            // When
            _testOutputHelper.WriteLine("start");

            using var response = await client.ExecuteQueryV1Batched(request);
            int i = 0;
            int resultsCount = 0;
            await foreach (var resultsBatch in response.GetBatchesAsync())
            {
                _testOutputHelper.WriteLine($"response batch number {i}");
                _testOutputHelper.WriteLine(resultsBatch.DataToJson() ?? "");
                resultsCount += resultsBatch.Data.Count();
                ++i;
            }

            _testOutputHelper.WriteLine("end");

            // Then
            Assert.Equal(0, i);
            Assert.Equal(0, resultsCount);
        }

        [Fact]
        public async Task TestBatchedQuery_SqlSyntaxErrorThrows()
        {
            // Given
            var trinoHost = _configuration.GetValue<string>(TRINO_HOST);
            var trinoPort = _configuration.GetValue<int>(TRINO_PORT);
            var trinoUser = _configuration.GetValue<string>(TRINO_USER);
            var trinoPass = _configuration.GetValue<string>(TRINO_PASSWORD);
            var client = new PrestodbClient(new PrestoClientSessionConfig()
            {
                User = trinoUser,
                Password = trinoPass,
                Host = trinoHost,
                Port = trinoPort,
                UseSsl = true,
            });
            var request = new ExecuteQueryV1Request($"select * from hive.testschema.testdata where 1 ( 0 limit {FULL_RESULT_SET_SIZE}");

            // .. Then
            await Assert.ThrowsAsync<PrestoQueryException>(async () =>
            {
                // When ..
                _testOutputHelper.WriteLine("start");

                using var response = await client.ExecuteQueryV1Batched(request);
                int i = 0;
                await foreach (var resultsBatch in response.GetBatchesAsync())
                {
                    _testOutputHelper.WriteLine($"response batch number {i}");
                    _testOutputHelper.WriteLine(resultsBatch.DataToJson() ?? "");
                    ++i;
                }

                _testOutputHelper.WriteLine("end");
            });

            // Then
            Assert.True(true);
        }

        [Fact]
        public async Task TestBatchedQuery_ColumnNameErrorThrows()
        {
            // Given
            var trinoHost = _configuration.GetValue<string>(TRINO_HOST);
            var trinoPort = _configuration.GetValue<int>(TRINO_PORT);
            var trinoUser = _configuration.GetValue<string>(TRINO_USER);
            var trinoPass = _configuration.GetValue<string>(TRINO_PASSWORD);
            var client = new PrestodbClient(new PrestoClientSessionConfig()
            {
                User = trinoUser,
                Password = trinoPass,
                Host = trinoHost,
                Port = trinoPort,
                UseSsl = true,
            });
            var request = new ExecuteQueryV1Request($"select asdfg from hive.testschema.testdata limit {FULL_RESULT_SET_SIZE}");

            // .. Then
            await Assert.ThrowsAsync<PrestoQueryException>(async () =>
            {
                // When ..
                _testOutputHelper.WriteLine("start");

                using var response = await client.ExecuteQueryV1Batched(request);
                int i = 0;
                await foreach (var resultsBatch in response.GetBatchesAsync())
                {
                    _testOutputHelper.WriteLine($"response batch number {i}");
                    _testOutputHelper.WriteLine(resultsBatch.DataToJson() ?? "");
                    ++i;
                }

                _testOutputHelper.WriteLine("end");
            });
        }

        [Fact]
        public async Task TestBatchedQuery_GetColumns()
        {
            // Given
            var trinoHost = _configuration.GetValue<string>(TRINO_HOST);
            var trinoPort = _configuration.GetValue<int>(TRINO_PORT);
            var trinoUser = _configuration.GetValue<string>(TRINO_USER);
            var trinoPass = _configuration.GetValue<string>(TRINO_PASSWORD);
            var client = new PrestodbClient(new PrestoClientSessionConfig()
            {
                User = trinoUser,
                Password = trinoPass,
                Host = trinoHost,
                Port = trinoPort,
                UseSsl = true,
            });
            var request = new ExecuteQueryV1Request($"select testdataid, fooid, footype, barnumber, foobarnumber from hive.testschema.testdata limit {FULL_RESULT_SET_SIZE}");

            // When
            _testOutputHelper.WriteLine("start");

            using var response = await client.ExecuteQueryV1Batched(request);
            var columns = await response.GetColumnsAsync();

            _testOutputHelper.WriteLine("end");

            // Then
            Assert.Equal(5, columns.Count);
            Assert.Equal("testdataid", columns[0].Name);
            Assert.Equal("fooid", columns[1].Name);
            Assert.Equal("footype", columns[2].Name);
            Assert.Equal("barnumber", columns[3].Name);
            Assert.Equal("foobarnumber", columns[4].Name);
        }

        [Fact]
        public async Task TestBatchedQuery_GetColumnsThenDataSameAsGetDataThenColumns()
        {
            // Given
            var trinoHost = _configuration.GetValue<string>(TRINO_HOST);
            var trinoPort = _configuration.GetValue<int>(TRINO_PORT);
            var trinoUser = _configuration.GetValue<string>(TRINO_USER);
            var trinoPass = _configuration.GetValue<string>(TRINO_PASSWORD);
            var client = new PrestodbClient(new PrestoClientSessionConfig()
            {
                User = trinoUser,
                Password = trinoPass,
                Host = trinoHost,
                Port = trinoPort,
                UseSsl = true,
            });
            var request = new ExecuteQueryV1Request($"select testdataid, fooid, footype, barnumber, foobarnumber from hive.testschema.testdata limit {FULL_RESULT_SET_SIZE}");

            // When
            // Get Columns Then Data
            _testOutputHelper.WriteLine("start");

            using var c_response = await client.ExecuteQueryV1Batched(request);
            int c_i = 0;
            int c_resultsCount = 0;
            var c_columns = await c_response.GetColumnsAsync();
            await foreach (var resultsBatch in c_response.GetBatchesAsync())
            {
                _testOutputHelper.WriteLine($"response batch number {c_i}");
                _testOutputHelper.WriteLine(resultsBatch.DataToJson() ?? "");
                c_resultsCount += resultsBatch.Data.Count();
                ++c_i;
            }

            _testOutputHelper.WriteLine("end");

            // Get Data Then Columns
            _testOutputHelper.WriteLine("start");

            using var d_response = await client.ExecuteQueryV1Batched(request);
            int d_i = 0;
            int d_resultsCount = 0;
            await foreach (var resultsBatch in d_response.GetBatchesAsync())
            {
                _testOutputHelper.WriteLine($"response batch number {d_i}");
                _testOutputHelper.WriteLine(resultsBatch.DataToJson() ?? "");
                d_resultsCount += resultsBatch.Data.Count();
                ++d_i;
            }
            var d_columns = await d_response.GetColumnsAsync();

            _testOutputHelper.WriteLine("end");


            // Then
            Assert.Equal(c_columns.Count, d_columns.Count);
            Assert.Equal(c_columns[0].Name, d_columns[0].Name);
            Assert.Equal(c_columns[1].Name, d_columns[1].Name);
            Assert.Equal(c_columns[2].Name, d_columns[2].Name);
            Assert.Equal(c_columns[3].Name, d_columns[3].Name);
            Assert.Equal(c_columns[4].Name, d_columns[4].Name);
            Assert.Equal(c_resultsCount, d_resultsCount);
        }

        [Fact]
        public async Task TestBatchedQuery_GetColumnsMultipleTimesAllowed()
        {
            // Given
            var trinoHost = _configuration.GetValue<string>(TRINO_HOST);
            var trinoPort = _configuration.GetValue<int>(TRINO_PORT);
            var trinoUser = _configuration.GetValue<string>(TRINO_USER);
            var trinoPass = _configuration.GetValue<string>(TRINO_PASSWORD);
            var client = new PrestodbClient(new PrestoClientSessionConfig()
            {
                User = trinoUser,
                Password = trinoPass,
                Host = trinoHost,
                Port = trinoPort,
                UseSsl = true
            });
            var request = new ExecuteQueryV1Request($"select testdataid, fooid, footype, barnumber, foobarnumber from hive.testschema.testdata limit {FULL_RESULT_SET_SIZE}");

            // When
            _testOutputHelper.WriteLine("start");

            using var response = await client.ExecuteQueryV1Batched(request);
            var columns1 = await response.GetColumnsAsync();
            var columns2 = await response.GetColumnsAsync();

            Assert.False(response.QueryClosed);

            _testOutputHelper.WriteLine("end");

            // Then
            Assert.Equal(columns1.Count, columns2.Count);
            Assert.Equal(columns1[0].Name, columns2[0].Name);
            Assert.Equal(columns1[0].Type, columns2[0].Type);
            Assert.Equal(columns1[1].Name, columns2[1].Name);
            Assert.Equal(columns1[1].Type, columns2[1].Type);
            Assert.Equal(columns1[2].Name, columns2[2].Name);
            Assert.Equal(columns1[2].Type, columns2[2].Type);
            Assert.Equal(columns1[3].Name, columns2[3].Name);
            Assert.Equal(columns1[3].Type, columns2[3].Type);
            Assert.Equal(columns1[4].Name, columns2[4].Name);
            Assert.Equal(columns1[4].Type, columns2[4].Type);
        }

        [Fact]
        public async Task TestBatchedQuery_TimeoutBeforeFullResults()
        {
            // Given
            var trinoHost = _configuration.GetValue<string>(TRINO_HOST);
            var trinoPort = _configuration.GetValue<int>(TRINO_PORT);
            var trinoUser = _configuration.GetValue<string>(TRINO_USER);
            var trinoPass = _configuration.GetValue<string>(TRINO_PASSWORD);
            var client = new PrestodbClient(new PrestoClientSessionConfig()
            {
                User = trinoUser,
                Password = trinoPass,
                Host = trinoHost,
                Port = trinoPort,
                UseSsl = true,
                ClientRequestTimeout = 1, // Test 1 seconds, we should hit timeout and cancel the request without seeing the full result set
            });
            var request = new ExecuteQueryV1Request($"select * from hive.testschema.testdata limit {FULL_RESULT_SET_SIZE}");

            // When
            _testOutputHelper.WriteLine("start");

            using var response = await client.ExecuteQueryV1Batched(request);
            int i = 0;
            int resultsCount = 0;
            await foreach (var resultsBatch in response.GetBatchesAsync())
            {
                _testOutputHelper.WriteLine($"response batch number {i}");
                _testOutputHelper.WriteLine(resultsBatch.DataToJson() ?? "");
                resultsCount += resultsBatch.Data.Count();
                ++i;
            }

            _testOutputHelper.WriteLine("end");


            // Then
            Assert.True(resultsCount < FULL_RESULT_SET_SIZE);
        }

        [Fact]
        public async Task TestBatchedQuery_TimeoutTimerIsPausedWhenCallerProcessing()
        {
            // Given
            var trinoHost = _configuration.GetValue<string>(TRINO_HOST);
            var trinoPort = _configuration.GetValue<int>(TRINO_PORT);
            var trinoUser = _configuration.GetValue<string>(TRINO_USER);
            var trinoPass = _configuration.GetValue<string>(TRINO_PASSWORD);
            var client = new PrestodbClient(new PrestoClientSessionConfig()
            {
                User = trinoUser,
                Password = trinoPass,
                Host = trinoHost,
                Port = trinoPort,
                UseSsl = true,
                ClientRequestTimeout = 30, // Test 30 seconds, we should not hit the timeout even if we process for 40 seconds
            });
            var request = new ExecuteQueryV1Request($"select * from hive.testschema.testdata limit {FULL_RESULT_SET_SIZE}");

            // When
            _testOutputHelper.WriteLine("start");

            using var response = await client.ExecuteQueryV1Batched(request);
            int i = 0;
            int resultsCount = 0;
            await foreach (var resultsBatch in response.GetBatchesAsync())
            {
                _testOutputHelper.WriteLine($"response batch number {i}");
                _testOutputHelper.WriteLine(resultsBatch.DataToJson() ?? "");
                resultsCount += resultsBatch.Data.Count();
                ++i;

                if (i == 1)
                {
                    await Task.Delay(40000); // Pretend to process for 40 seconds, this should NOT cause timeout of request.
                }
            }

            _testOutputHelper.WriteLine("end");


            // Then
            Assert.Equal(FULL_RESULT_SET_SIZE, resultsCount);
        }

        [Fact]
        public async Task TestBatchedQuery_ReiteratingOverResponseThrows()
        {
            // Given
            var trinoHost = _configuration.GetValue<string>(TRINO_HOST);
            var trinoPort = _configuration.GetValue<int>(TRINO_PORT);
            var trinoUser = _configuration.GetValue<string>(TRINO_USER);
            var trinoPass = _configuration.GetValue<string>(TRINO_PASSWORD);
            var client = new PrestodbClient(new PrestoClientSessionConfig()
            {
                User = trinoUser,
                Password = trinoPass,
                Host = trinoHost,
                Port = trinoPort,
                UseSsl = true
            });
            var request = new ExecuteQueryV1Request($"select * from hive.testschema.testdata limit {FULL_RESULT_SET_SIZE}");

            // When
            _testOutputHelper.WriteLine("start");

            using var response = await client.ExecuteQueryV1Batched(request);
            int i = 0;
            int resultsCount = 0;
            await foreach (var resultsBatch in response.GetBatchesAsync())
            {
                _testOutputHelper.WriteLine($"response batch number {i}");
                _testOutputHelper.WriteLine(resultsBatch.DataToJson() ?? "");
                resultsCount += resultsBatch.Data.Count();
                ++i;
            }

            _testOutputHelper.WriteLine("end");
            _testOutputHelper.WriteLine("start reit");

            int reit_i = 0;
            int reit_resultsCount = 0;
            var exception = await Assert.ThrowsAsync<PrestoException>(async () =>
            {
                await foreach (var resultsBatch in response.GetBatchesAsync())
                {
                    _testOutputHelper.WriteLine($"response reit batch number {reit_i}");
                    _testOutputHelper.WriteLine(resultsBatch.DataToJson() ?? "");
                    reit_resultsCount += resultsBatch.Data.Count();
                    ++reit_i;
                }
            });

            _testOutputHelper.WriteLine("end reit");

            // Then
            Assert.NotEqual(0, i);
            Assert.NotEqual(0, resultsCount);
            Assert.Equal(0, reit_i);
            Assert.Equal(0, reit_resultsCount);
        }

        [Fact]
        public async Task TestBatchedQuery_DisposeClosesQuery()
        {
            // Given
            var trinoHost = _configuration.GetValue<string>(TRINO_HOST);
            var trinoPort = _configuration.GetValue<int>(TRINO_PORT);
            var trinoUser = _configuration.GetValue<string>(TRINO_USER);
            var trinoPass = _configuration.GetValue<string>(TRINO_PASSWORD);
            var client = new PrestodbClient(new PrestoClientSessionConfig()
            {
                User = trinoUser,
                Password = trinoPass,
                Host = trinoHost,
                Port = trinoPort,
                UseSsl = true
            });
            var request = new ExecuteQueryV1Request($"select * from hive.testschema.testdata limit {FULL_RESULT_SET_SIZE}");

            // When
            _testOutputHelper.WriteLine("start");

            using var response = await client.ExecuteQueryV1Batched(request);
            int i = 0;
            int resultsCount = 0;
            var enumerator = response.GetBatchesAsync().GetAsyncEnumerator();
            await enumerator.MoveNextAsync();
            _testOutputHelper.WriteLine($"response batch number {i}");
            _testOutputHelper.WriteLine(enumerator.Current.DataToJson() ?? "");
            resultsCount += enumerator.Current.Data.Count();
            ++i;

            _testOutputHelper.WriteLine("end after 1 batch");

            Assert.False(response.QueryClosed);
            response.Dispose();
            // Then
            Assert.True(response.QueryClosed);
        }

        [Fact]
        public async Task TestBatchedQuery_DisposeAsyncClosesQuery()
        {
            // Given
            var trinoHost = _configuration.GetValue<string>(TRINO_HOST);
            var trinoPort = _configuration.GetValue<int>(TRINO_PORT);
            var trinoUser = _configuration.GetValue<string>(TRINO_USER);
            var trinoPass = _configuration.GetValue<string>(TRINO_PASSWORD);
            var client = new PrestodbClient(new PrestoClientSessionConfig()
            {
                User = trinoUser,
                Password = trinoPass,
                Host = trinoHost,
                Port = trinoPort,
                UseSsl = true
            });
            var request = new ExecuteQueryV1Request($"select * from hive.testschema.testdata limit {FULL_RESULT_SET_SIZE}");

            // When
            _testOutputHelper.WriteLine("start");

            using var response = await client.ExecuteQueryV1Batched(request);
            int i = 0;
            int resultsCount = 0;
            var enumerator = response.GetBatchesAsync().GetAsyncEnumerator();
            await enumerator.MoveNextAsync();
            _testOutputHelper.WriteLine($"response batch number {i}");
            _testOutputHelper.WriteLine(enumerator.Current.DataToJson() ?? "");
            resultsCount += enumerator.Current.Data.Count();
            ++i;

            _testOutputHelper.WriteLine("end after 1 batch");

            Assert.False(response.QueryClosed);
            await response.DisposeAsync();
            // Then
            Assert.True(response.QueryClosed);
        }

        [Fact]
        public async Task TestBatchedQuery_CancelClosesQuery()
        {
            // Given
            var trinoHost = _configuration.GetValue<string>(TRINO_HOST);
            var trinoPort = _configuration.GetValue<int>(TRINO_PORT);
            var trinoUser = _configuration.GetValue<string>(TRINO_USER);
            var trinoPass = _configuration.GetValue<string>(TRINO_PASSWORD);
            var client = new PrestodbClient(new PrestoClientSessionConfig()
            {
                User = trinoUser,
                Password = trinoPass,
                Host = trinoHost,
                Port = trinoPort,
                UseSsl = true
            });
            var request = new ExecuteQueryV1Request($"select * from hive.testschema.testdata limit {FULL_RESULT_SET_SIZE}");
            var cancellationTokenSource = new CancellationTokenSource();

            // When
            _testOutputHelper.WriteLine("start");

            using var response = await client.ExecuteQueryV1Batched(request, cancellationTokenSource.Token);
            int i = 0;
            int resultsCount = 0;

            var it = response.GetBatchesAsync().GetAsyncEnumerator();
            if (await it.MoveNextAsync())
            {
                var resultsBatch = it.Current;
                _testOutputHelper.WriteLine($"response batch number {i}");
                _testOutputHelper.WriteLine(resultsBatch.DataToJson() ?? "");
                resultsCount += resultsBatch.Data.Count();
                ++i;
            }

            Assert.False(response.QueryClosed);

            cancellationTokenSource.Cancel();

            if (await it.MoveNextAsync())
            {
                var resultsBatch = it.Current;
                _testOutputHelper.WriteLine($"response batch number {i}");
                _testOutputHelper.WriteLine(resultsBatch.DataToJson() ?? "");
                resultsCount += resultsBatch.Data.Count();
                ++i;
            }

            // Then
            Assert.True(response.QueryClosed);

            _testOutputHelper.WriteLine("end");

            // Check that only the first iteration happened
            Assert.Equal(1, i);
        }
    }
}
