using System.Net.WebSockets;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Exceptions;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NiteAdvServerCore.Entities;
using System.Reflection;
using NiteAdvServerCore.Util;
using Gremlin.Net.Driver.Remote;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Structure;

namespace NiteAdvServerCore.Managers;

internal class GremlinManager
{


    public static GremlinManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GremlinManager();
            }
            return instance;
        }
    }

    #region Private Methods
    private static GremlinManager instance = null;
    private static string Host => "nightevents.gremlin.cosmos.azure.com";
    private static string PrimaryKey => "H5UMlijKsYTI1swHzD55hS4WeSPA85KI9M1fA8vX2RerpGJiLVeOSJw3JA62B4FcsDSgiZQlnVVsb1OvuriPOA==";
    private static string Database => "nightevents";
    private static string Container => "g01";
    private static bool EnableSSL => true;
    private static int Port => 443;
    private GremlinClient Client;
    private ConnectionPoolSettings connectionPoolSettings => new ConnectionPoolSettings()
    {
        MaxInProcessPerConnection = 10,
        PoolSize = 30,
        ReconnectionAttempts = 3,
        ReconnectionBaseDelay = TimeSpan.FromMilliseconds(500)
    };
    private Action<ClientWebSocketOptions> webSocketConfiguration => new Action<ClientWebSocketOptions>(options =>
    {
        options.KeepAliveInterval = TimeSpan.FromSeconds(10);
    });
    private GremlinServer gremlinServer = null;
    private GremlinManager()
    {
        //inizializzazione della push notification
        string containerLink = "/dbs/" + Database + "/colls/" + Container;
        Console.WriteLine($"Connecting to: host: {Host}, port: {Port}, container: {containerLink}, ssl: {EnableSSL}");
        gremlinServer = new GremlinServer(Host, Port, enableSsl: EnableSSL,
                                                username: containerLink,
                                                password: PrimaryKey);

        Client = new GremlinClient(gremlinServer,
         new GraphSON2Reader(),
         new GraphSON2Writer(),
         GremlinClient.GraphSON2MimeType,
         connectionPoolSettings, webSocketConfiguration);

        // non funziona api 3.4.13
        // g = AnonymousTraversalSource.Traversal().WithRemote(new DriverRemoteConnection(Client));

    }



    private static Task<ResultSet<dynamic>> SubmitRequest(GremlinClient gremlinClient, string query)
    {
        try
        {
            return gremlinClient.SubmitAsync<dynamic>(query);
        }
        catch (ResponseException e)
        {
            Console.WriteLine("\tRequest Error!");

            // Print the Gremlin status code.
            Console.WriteLine($"\tStatusCode: {e.StatusCode}");

            // On error, ResponseException.StatusAttributes will include the common StatusAttributes for successful requests, as well as
            // additional attributes for retry handling and diagnostics.
            // These include:
            //  x-ms-retry-after-ms         : The number of milliseconds to wait to retry the operation after an initial operation was throttled. This will be populated when
            //                              : attribute 'x-ms-status-code' returns 429.
            //  x-ms-activity-id            : Represents a unique identifier for the operation. Commonly used for troubleshooting purposes.
            PrintStatusAttributes(e.StatusAttributes);
            Console.WriteLine($"\t[\"x-ms-retry-after-ms\"] : {GetValueAsString(e.StatusAttributes, "x-ms-retry-after-ms")}");
            Console.WriteLine($"\t[\"x-ms-activity-id\"] : {GetValueAsString(e.StatusAttributes, "x-ms-activity-id")}");

            throw;
        }

    }

    private static void PrintStatusAttributes(IReadOnlyDictionary<string, object> attributes)
    {
        Console.WriteLine($"\tStatusAttributes:");
        Console.WriteLine($"\t[\"x-ms-status-code\"] : {GetValueAsString(attributes, "x-ms-status-code")}");
        Console.WriteLine($"\t[\"x-ms-total-server-time-ms\"] : {GetValueAsString(attributes, "x-ms-total-server-time-ms")}");
        Console.WriteLine($"\t[\"x-ms-total-request-charge\"] : {GetValueAsString(attributes, "x-ms-total-request-charge")}");
    }

    private static string GetValueAsString(IReadOnlyDictionary<string, object> dictionary, string key)
    {
        return JsonConvert.SerializeObject(GetValueOrDefault(dictionary, key));
    }

    private static object GetValueOrDefault(IReadOnlyDictionary<string, object> dictionary, string key)
    {
        if (dictionary.ContainsKey(key))
        {
            return dictionary[key];
        }

        return null;
    }
    //@Marco: refactoring required
    public T CreateIstance<T>(IEnumerable<KeyValuePair<string, object>> input) where T : VertexEntity
    {
        try
        {
            T entity = Activator.CreateInstance<T>();
            var fields = typeof(T).GetProperties();
            entity.id = input.Where(kvp => kvp.Key.Equals("id")).FirstOrDefault().Value.ToString();
            dynamic properties = input.Where(kvp => kvp.Key.Equals("properties")).FirstOrDefault().Value;
            foreach (var property in properties)
            {

                foreach (var fieldInfo in fields)
                {
                    //se non ha attributi - per ora di nessun genere
                    object[] attrs = fieldInfo.GetCustomAttributes(true);
                    if (attrs == null || !attrs.Any())
                    {
                        if (fieldInfo.Name == property.Key && fieldInfo.Name != "partitionKey")
                        {

                            IEnumerable<object> val = (IEnumerable<object>)property.Value;
                            //recupero del valore 
                            var value = ((IEnumerable<KeyValuePair<string, object>>)val.ToList()[0]).Where(kvp => kvp.Key.Equals("value")).FirstOrDefault().Value;
                            if (!(value is null))
                            {

                                fieldInfo.SetValue(entity, SetTheRightObject(fieldInfo, value));
                            }


                            break;
                        }
                    }
                }

            }

            return entity;
        }
        catch (Exception ex)
        {
            return null;
        }

    }
    private T InsertVertex<T>(T vertex, GremlinClient gremlinClient) where T : VertexEntity
    {
        T result = default(T);
        string cmd = $"g.addV('{vertex.label}')";
        var properties = vertex.GetType().GetProperties();
        foreach (var property in properties)
        {
            var value = property.GetValue(vertex, null);
            if (value != null)
            {
                cmd += ComposeTheRightCommand(property.Name, value);
            }
        }
        var resultSet = SubmitRequest(gremlinClient, cmd).Result;
        if (resultSet.Count > 0)
        {
            result = CreateIstance<T>(resultSet.First());
        }
        return result;
    }
    private T UpdateVertex<T>(T vertex, GremlinClient gremlinClient) where T : VertexEntity
    {
        T result = default(T);
        string cmd = $"g.V('{vertex.label}', 'id', '{vertex.id}')";
        var properties = vertex.GetType().GetProperties();
        foreach (var property in properties)
        {
            var value = property.GetValue(vertex, null);
            if (value != null && property.Name != "id" && property.Name != "partitionKey") //
            {
                cmd += ComposeTheRightCommand(property.Name, value);
            }
        }
        var resultSet = SubmitRequest(gremlinClient, cmd).Result;
        if (resultSet.Count > 0)
        {

            result = CreateIstance<T>(resultSet.First());
        }

        return result;
    }

    private string ComposeTheRightCommand(string propertyName, object value)
    {
        string cmd = string.Empty;
        if (value is String || value is string)
            cmd += $".property('{propertyName}', '{value}')";
        else if (value is double || value is Double)
            cmd += $".property('{propertyName}', '{value.ToString().Replace(',', '.')}')";
        else if (value is bool || value is Boolean)
        {
            string val = ((bool)value) ? "true" : "false";
            cmd += $".property('{propertyName}', '{val}')";
        }
        else
            cmd += $".property('{propertyName}', {value})";
        return cmd;
    }
    private object SetTheRightObject(PropertyInfo fieldInfo, object value)
    {
        if (fieldInfo.PropertyType.Name.Contains("double") || fieldInfo.PropertyType.Name.Contains("Double"))
        {
            try { return Convert.ToDouble(value); } catch (Exception) { return 0; }
        }
        else if (fieldInfo.PropertyType.Name.Contains("bool") || fieldInfo.PropertyType.Name.Contains("Boolean"))
            return Convert.ToBoolean(value);
        else
            return value;
    }
    #endregion

    #region public Methods
    public ResultSet<dynamic> FreeQuery(string query)
    {

        return SubmitRequest(Client, query).Result;
    }
    public async Task<List<T>> RetreiveData<T>(string query, bool isEdge = false) where T : VertexEntity
    {

        List<T> result = new List<T>();
        var resultSet = await SubmitRequest(Client, query);
        if (resultSet.Count > 0)
        {
            foreach (var res in resultSet)
            {
                // The vertex results are formed as Dictionaries with a nested dictionary for their properties
                dynamic? cls = null;
                if (!isEdge)
                {
                    cls = CreateIstance<T>(res);

                }
                else
                {
                    cls = JsonConvert.DeserializeObject<T>(res);
                }
                if (cls != null)
                    result.Add(cls);
                //  Console.WriteLine($"\t{output}");
            }
        }
        return result.ToList();
    }
    public long Count(string query)
    {

        // using var gremlinClient = GetClient();
        var resultSet = SubmitRequest(Client, query).Result;
        return resultSet.FirstOrDefault();
    }
    public T SaveVertex<T>(T vertex, bool updateLastSyncDate = true) where T : VertexEntity
    {
        T result = default(T);
        if (updateLastSyncDate)
            vertex.LastSyncDate = ServerUtil.GetUnixFormatDateTime(DateTime.UtcNow);
        if (!String.IsNullOrWhiteSpace(vertex.id.ToString()))
        {
            var resultSet = SubmitRequest(Client, $"g.V('{vertex.id}').hasLabel('{vertex.label}')").Result;
            if (resultSet.Count > 0)
                result = UpdateVertex<T>(vertex, Client);
            else
                result = InsertVertex<T>(vertex, Client);

        }
        else
            result = InsertVertex<T>(vertex, Client);

        return result;
    }
    public bool AddEdge(string sourceID, string targetID, string relationship)
    {
        try
        {
            var cmd = "g.V('" + sourceID + "').as('v').V('" + targetID + "').coalesce(__.inE('" + relationship + "').where(outV().as('v')),addE('" + relationship + "').from('v'))";
            var resultSet = SubmitRequest(Client, cmd).Result;
            if (resultSet.Count == 0)
                throw new Exception("malformed request");

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    #endregion

}
