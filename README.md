# BAMCIS PrestoClient - A Prestodb .NET Client

## Usage

### Basic Example 1

This demonstrates creating a new client config, initializing an IPrestoClient, and executing a simple query. The
returned data can be formatted in CSV or JSON. Additionally, all of the raw data is returned from the server
in case the deserialization process fails in .NET, the user can still access and manipulate the returned data.

     PrestoClientSessionConfig Config = new PrestoClientSessionConfig("hive", "cars")
     {
		Host = "localhost",
        Port = 8080
     )
	 ;
     IPrestoClient Client = new PrestodbClient(Config);

     ExecuteQueryV1Request Request = new ExecuteQueryV1Request("select * from tracklets limit 5;");

     ExecuteQueryV1Response QueryResponse = await Client.ExecuteQueryV1(Request);

     Console.WriteLine(String.Join("\n", QueryResponse.Response.DataToCsv()));
     Console.WriteLine("-------------------------------------------------------------------");
	 Console.WriteLine(String.Join("\n", QueryResponse.Response.DataToJson()));

## Revision History

### 0.198.2
Removed unused classes and allow null/empty values for `Catalog` and `Schema` in `PrestoClientSessionConfig`.

### 0.198.0
Initial release of the client compatible with Presto version 0.198.

### 0.197.0
Initial release of the client compatible with Presto version 0.197.
