# bamcis.io PrestoClient - A Prestodb .NET Client

## Usage

### Basic Example 1

This demonstrates creating a new client config, initializing an IPrestoClient, and executing a simple query. The
returned data can be formatted in CSV or JSON. Additionally, all of the raw data is returned from the server
in case the deserialization process fails in .NET, the user can still access and manipulate the returned data.

```csharp
PrestoClientSessionConfig config = new PrestoClientSessionConfig("hive", "cars")
{
   Host = "localhost",
   Port = 8080
};

IPrestoClient client = new PrestodbClient(config);
ExecuteQueryV1Request request = new ExecuteQueryV1Request("select * from tracklets limit 5;");
ExecuteQueryV1Response queryResponse = await client.ExecuteQueryV1(request);

Console.WriteLine(String.Join("\n", queryResponse.DataToCsv()));
Console.WriteLine("-------------------------------------------------------------------");
Console.WriteLine(String.Join("\n", queryResponse.DataToJson()));
```

### Batched Example 1

This demonstrates how to execute a query and return the response in batches for "streaming" consumption, which is
useful when processing large result sets without having to keep the entire deserialized resultset in memory.

```csharp
PrestoClientSessionConfig config = new PrestoClientSessionConfig("hive", "cars")
{
   Host = "localhost",
   Port = 8080
};

IPrestoClient client = new PrestodbClient(config);
ExecuteQueryV1Request request = new ExecuteQueryV1Request("select * from tracklets limit 500000;");
ExecuteQueryV1BatchedResponse queryResponseBatched = await client.ExecuteQueryV1Batched(request);

IReadOnlyList<Column> columns = await queryResponse.GetColumnsAsync(); // Gets column information

// Iterate through the data row-by-row, fetching the next response batch only when needed
await foreach (List<dynamic> dataRow in queryResponse.GetDataAsync())
{
   Console.WriteLine(String.Join("\n", dataRow.ToString()));
}
```

## Local Testing

A suite of regression integration tests are located the `PrestoClient.DevTest` project.

1. Add required secrets to the `appsettings.TEST.json`.
2. Use a test runner to run the test suite in PrestoClientManualTest.

## Revision History

### 0.198.5
Converted `Thread.Sleep` to `Task.Delay`.

### 0.198.4-beta
Added `CancellationToken` support to all client methods.

### 0.198.3-beta
Added username/password auth to client.

### 0.198.2
Removed unused classes and allow null/empty values for `Catalog` and `Schema` in `PrestoClientSessionConfig`.

### 0.198.0
Initial release of the client compatible with Presto version 0.198.

### 0.197.0
Initial release of the client compatible with Presto version 0.197.
