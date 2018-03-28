# BAMCIS PrestoClient - A Prestodb .NET Client

## Usage

### Basic Example 1

This demonstrates creating a new client config, initializing an IPrestoClient, and executing a simple query. The
returned data can be formatted in CSV or JSON. Additionally, all of the raw data is returned from the server
in case the deserialization process fails in .NET, the user can still access and manipulate the returned data.

     PrestoClientConfig Opts = new PrestoClientConfig("hive", "cars");
     Opts.Host = "localhost";
     Opts.Port = 8080;
     IPrestoClient Client = new PrestoClient.PrestoClient(Opts);

     ExecuteQueryV1Request Req = new ExecuteQueryV1Request("select * from tracklets limit 5");
     ExecuteQueryV1Response Res = await Client.ExecuteQueryV1(Req);

     Console.WriteLine(String.Join("\n", Res.Response.DataToCsv()));
     Console.WriteLine("-------------------------------------------------------------------");
	 Console.WriteLine(String.Join("\n", Res.Response.DataToJson()));

## Revision History

### 0.0.0.0
This is currently a work in progress