using System;
using Microsoft.VisualBasic;
using System.Net.Http.Headers;
using System.Text;
using NiteAdvServerCore.Entities;
using Microsoft.Azure.Cosmos;
using Google.Apis.Http;
using System.Net;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using NiteAdvServerCore.Util;
using Gremlin.Net.Process.Traversal;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Newtonsoft.Json;

namespace NiteAdvServerCore.Managers;

internal class CosmosManager
{
    //https://docs.microsoft.com/it-it/azure/cosmos-db/sql/sql-api-get-started
    private static CosmosManager instance = null;
    public static CosmosManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CosmosManager();

            }
            return instance;
        }
    }
    private static string endpointUri => "https://nightevents.documents.azure.com:443/";
    private static string primaryKey => "H5UMlijKsYTI1swHzD55hS4WeSPA85KI9M1fA8vX2RerpGJiLVeOSJw3JA62B4FcsDSgiZQlnVVsb1OvuriPOA==";
    private static string databaseId => "nightevents";
    private static string containerId => "g01";


    // The Cosmos client instance
    private CosmosClient cosmosClient;
    //private Database database;
    private Container container;

    private CosmosManager()
    {
        cosmosClient = new CosmosClient(endpointUri, primaryKey, new CosmosClientOptions()
        {
            ConnectionMode = ConnectionMode.Gateway
        });
        Database database = this.cosmosClient.GetDatabase(databaseId);

        this.container = database.GetContainer(containerId);
    }
    #region private methods

    private string GetVertexLabelName<T>() where T : VertexEntity
    {
        var entity = Activator.CreateInstance<T>();
        string result = entity.label;
       
        return result;
    }
    private  Tuple<T,string> GetTableName<T>() where T : VertexEntity
    {
        var entity = Activator.CreateInstance<T>();
        string result = typeof(T).Name;
        object[] attrs = entity.GetType().GetCustomAttributes(true);
        if (attrs != null && attrs.Any())
        {
            foreach (var attr in attrs)
            {
                if (attr.GetType() == typeof(TableNameAttribute))
                {
                    result = ((TableNameAttribute)attr).TableName;
                }
            }
        }
        return new Tuple<T,string>(entity,result);
    }
    private List<T> FromJsonStream<T>(Stream stream) where T : VertexEntity
    {
        List<T> result = new List<T>();
        using (StreamReader sr = new StreamReader(stream))
        {
            //This allows you to do one Read operation.
            var jsonString = sr.ReadToEnd();
            if (!String.IsNullOrWhiteSpace(jsonString))
            {
                var utf8 = Encoding.UTF8;
                byte[] utfBytes = utf8.GetBytes(jsonString);
                string stringResult = utf8.GetString(utfBytes, 0, utfBytes.Length);
                var deserializedJson = JsonConvert.DeserializeObject<ResponseObject<T>>(stringResult);
                if (deserializedJson != null && deserializedJson.Documents != null && deserializedJson.Documents.Any())
                    result.AddRange(deserializedJson.Documents);
            }
            
        }
        return result;
    }
    #endregion
    #region public methods
 /*   public async Task<List<T>> RetreiveData<T>(string WhereCondition, int offset = -1, int limit = -1) where T : VertexEntity
    {


        var label = GetVertexLabelName<T>();
        string where = where = String.IsNullOrWhiteSpace(WhereCondition) ? String.Empty : String.Format(" where {0}", WhereCondition);
        string sqlString = $"SELECT * FROM c  WHERE CONTAINS(c.label, '{label}')";
        if (offset > -1)
            sqlString += $" offset {offset}";
        if (limit > -1)
            sqlString += $" limit {limit}";
        //string testQuery = $"SELECT * FROM c  WHERE CONTAINS(c.label, '{label}') offset 0 limit 10";
        QueryDefinition queryDefinition = new QueryDefinition(sqlString);
        using FeedIterator queryResultSetIterator = this.container.GetItemQueryStreamIterator(queryDefinition);

        List<T> result = new List<T>();

        while (queryResultSetIterator.HasMoreResults)
        {
            using (ResponseMessage response = await queryResultSetIterator.ReadNextAsync())
            {
                if (response.IsSuccessStatusCode)
                {
                    var deserializeResult = FromJsonStream<T>(response.Content);
                    if (deserializeResult != null && deserializeResult.Any())
                        result.AddRange(deserializeResult);
                }
                else
                    throw new Exception();
            }
          

        }
        return result;


    }*/

   public async Task<List<T>> RetreiveData<T>(string WhereCondition, int offset = -1, int limit = -1) where T : VertexEntity
    {


        var label = GetVertexLabelName<T>();
        string where = where = String.IsNullOrWhiteSpace(WhereCondition) ? String.Empty : String.Format(" where {0}", WhereCondition);
        string sqlString = $"SELECT * FROM c  WHERE CONTAINS(c.label, '{label}')";
        if (offset > -1)
            sqlString += $" offset {offset}";
        if (limit > -1)
            sqlString += $" limit {limit}";
        //string testQuery = $"SELECT * FROM c  WHERE CONTAINS(c.label, '{label}') offset 0 limit 10";
        QueryDefinition queryDefinition = new QueryDefinition(sqlString);
        using FeedIterator<T> queryResultSetIterator = this.container.GetItemQueryIterator<T>(queryDefinition);

        List<T> result = new List<T>();

        while (queryResultSetIterator.HasMoreResults)
        {
            FeedResponse<T> response = await queryResultSetIterator.ReadNextAsync();
            if (response != null && response.Any())
                   result.AddRange(response);
                
            


        }
        return result;


    }
    public async Task<T> SaveVertex<T>(T vertex, bool updateLastSyncDate = true) where T : VertexEntity
    {
        T result = null;
        try
        {
         //   if (updateLastSyncDate)
               // vertex.LastSyncDate = ServerUtil.GetUnixFormatDateTime(DateTime.UtcNow);
            // Read the item to see if it exists
          //  ItemResponse<T> wakefieldResponse = await this.container.ReadItemAsync<T>(vertex.id, new Microsoft.Azure.Cosmos.PartitionKey(vertex.partitionKey));
            //UPDATE
          //  var oldItem = wakefieldResponse.Resource;
           // ItemResponse<T> updateResponse = await this.container.ReplaceItemAsync<T>(vertex, oldItem.id, new Microsoft.Azure.Cosmos.PartitionKey(vertex.partitionKey));
            //result = updateResponse.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            // Create an item in the container representing the Wakefield family. Note we provide the value of the partition key for this item, which is "Wakefield"
            //INSERT
            ItemResponse<T> insertResponse = await this.container.CreateItemAsync<T>(vertex, new Microsoft.Azure.Cosmos.PartitionKey(vertex.partitionKey));
            result = insertResponse.Resource;
        }
        return result;
    }
    public async Task<long> Count<T>(string WhereCondition) where T : VertexEntity
    {
        long result = 0;
        var className = GetTableName<T>();
        string where = String.Empty;
        where = String.IsNullOrWhiteSpace(WhereCondition) ? String.Empty : String.Format(" where {0}", WhereCondition);
        string sqlString = String.Format("Select COUNT(*) from {1} {2}", className, where);
        using FeedIterator<T> queryResultSetIterator = this.container.GetItemQueryIterator<T>(sqlString);
        while (queryResultSetIterator.HasMoreResults)
        {
            FeedResponse<T> currentResultSet = await queryResultSetIterator.ReadNextAsync();
            result += currentResultSet.LongCount();

        }
        return result;
    }
    #endregion

}

public class ResponseObject<T> 
{
    public string _rid { get; set; }
    public List<T> Documents { get; set; }
    public int _count { get; set; }
}
