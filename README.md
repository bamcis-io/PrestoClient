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

Console.WriteLine(String.Join("\n", queryResponse.Response.DataToCsv()));
Console.WriteLine("-------------------------------------------------------------------");
Console.WriteLine(String.Join("\n", queryResponse.Response.DataToJson()));
```

## Revision History

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
