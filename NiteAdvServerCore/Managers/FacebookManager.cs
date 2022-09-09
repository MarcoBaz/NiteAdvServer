using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using Google.Apis.Auth.OAuth2;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using NiteAdvServerCore.Entities;

namespace NiteAdvServerCore.Managers;

public class FacebookManager
{
    private HttpClient httpClient;
    private static FacebookManager instance = null;
    private string baseAddress = "https://graph.facebook.com/";
    private FacebookManager()
    {
        httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(90);
    }

    internal static FacebookManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FacebookManager();
            }
            return instance;
        }
    }
    //https://developers.facebook.com/docs/pages/official-events
    public User GetUserData(string accessToken, string userID)
    {
        User result = null;
        //var fb = new FacebookClient(accessToken);
        //dynamic resultAccess = fb.Get($"{userID}?metadatada=1");
        List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
        param.Add(new KeyValuePair<string, string>("fields", "id,name,email"));
        //param.Add(new KeyValuePair<string, string>("scope", "email"));
        param.Add(new KeyValuePair<string, string>("access_token", accessToken));
        dynamic resultAccess = Get($"{userID}",param).Result;
        if (resultAccess != null)
        {
            var name = ((string)resultAccess.name).Split(" ");
            result = new User(userID);
            result.Name = name[0];
            result.Surename = name[1];
            result.Email = resultAccess.email??"";
        }
        return result;



    }

    public Event SaveEvent(Event even)
    {
        return new Event();
    }
    private async Task<dynamic> Get(string api, List<KeyValuePair<string, string>> parameters)
    {
        dynamic result = null;
        var builder = new UriBuilder(baseAddress + api);
        if (parameters != null && parameters.Any())
        {
            string resultQuery = "";
            int counter = 0;
            foreach (var param in parameters)
            {
                resultQuery = resultQuery + $"{param.Key}={System.Net.WebUtility.UrlDecode(param.Value)}";
                if (counter != parameters.Count - 1)
                    resultQuery += "&";
                counter += 1;
            }
            builder.Query = resultQuery;
        }

        string url = builder.ToString();
        var response = await httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            string resultString = await response.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject<dynamic>(resultString);
        }
        else
            throw new Exception("Error Facebook request");

        return result;
    }
}

//public class FacebookUser
//{
//    public String UserId { get; set; }
//    public String Name { get; set; }
//    public String Surename { get; set; }
//    public String Email { get; set; }
//}

