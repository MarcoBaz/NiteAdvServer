using AutoMapper;
using Facebook;
using NiteAdvServerCore.DTO;
using NiteAdvServerCore.DTO.Token;
using NiteAdvServerCore.Entities;
using NiteAdvServerCore.Generic;
using NiteAdvServerCore.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Jwt.AccessToken;

namespace NiteAdvServerCore.Managers;

public static class BusinessLogic
{
    public static Mapper MapperManager = new Mapper(MapperConfigurator.Configure);

    #region crawler

    public static StatusDTO SaveStatus(StatusDTO statusDTO)
    {
        StatusDTO result = new StatusDTO();
        try
        {
            int id = 0;
            var config = SqlServerManager.GetBaseEntity<Config>("DockerId='" + statusDTO.DockerId + "'");
            if (config == null)
                throw new Exception("Docker not present in configuration");

            var existingStatus = SqlServerManager.GetBaseEntity<Status>("IdConfig=" + config.Id);
            if (existingStatus == null)
            {
                int? count = SqlServerManager.FreeQueryCount("Select MAX(Id) from Status");
                id = count == null ? 1 : count.Value + 1;
                SqlServerManager.FreeQuery<Status>("insert into Status (Id,IdConfig,IdStatusDescription,DeviceIP,Message,City,LastSyncDate) values (" + id + "," + config.Id + "," + statusDTO.IdStatusDescription + ",'" + statusDTO.DeviceIp + "','" + statusDTO.Message + "','" + statusDTO.City + "', CONVERT(DATETIME, '" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff tt") + "',103) )");

            }
            else
            {
                SqlServerManager.FreeQuery<Status>("update Status set IdStatusDescription=" + statusDTO.IdStatusDescription + " ,DeviceIP='" + statusDTO.DeviceIp + "',Message='" + statusDTO.Message + "',City='" + statusDTO.City + "',LastSyncDate=CONVERT(DATETIME, '" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff tt") + "',103) where Id=" + existingStatus.Id);
                id = existingStatus.Id;
            }
            var EntityResult = SqlServerManager.GetBaseEntity<Status>("Id=" + id);
            if (EntityResult == null)
                throw new Exception("error on inssert or update");

            //var EntityResult = SqlServerManager.GetBaseEntity<Status>("Id=" + statusDTO.Id);
            result.DockerId = statusDTO.DockerId;
            result.IdStatusDescription = EntityResult.IdStatusDescription;
            result.DeviceIp = EntityResult.DeviceIp;
            result.Message = EntityResult.Message;
            result.City = EntityResult.City;
            result.LastSyncDate = EntityResult.LastSyncDate;


            result.ActionResult = 200;
            return result;

        }

        catch (Exception ex)
        {
            result.ActionResult = 400;
            result.ActionError = ex.Message;
        }
        return result;
    }
    public static StatusDTO GetStatus(string DockerId)
    {
        StatusDTO result = new StatusDTO();
        try
        {
            var config = SqlServerManager.GetBaseEntity<Config>("DockerId='" + DockerId + "'");
            if (config == null)
                throw new Exception("No configuration found");

            var status = SqlServerManager.GetBaseEntity<Status>("IdConfig=" + config.Id);
            if (status == null)
                throw new Exception("No status found");

            result.DeviceIp = status.DeviceIp;
            result.DockerId = DockerId;
            result.IdStatusDescription = status.IdStatusDescription;
            result.Message = status.Message;
            result.City = status.City;
            result.LastSyncDate = status.LastSyncDate;
            result.ActionResult = 200;
        }

        catch (Exception ex)
        {
            result.ActionResult = 400;
            result.ActionError = ex.Message;
        }
        return result;


    }
    public static List<Status> GetAllStatus()
    {
        return SqlServerManager.GetBaseCollection<Status>("Id>0");
    }
    public static ConfigDTO GetConfig(string DockerId)
    {

        ConfigDTO result = new ConfigDTO();
        try
        {
            var config = SqlServerManager.GetBaseEntity<Config>("DockerId='" + DockerId + "'");
            if (config == null)
                throw new Exception("No configuration found");


            result.Port = config.Port;
            result.DockerId = config.DockerId;
            result.MongooseUrl = config.MongooseUrl;
            result.LastSyncDate = config.LastSyncDate;
            result.TickMinutes = config.TickMinutes;
        }

        catch (Exception ex)
        {
            result.ActionResult = 400;
            result.ActionError = ex.Message;
        }
        return result;

    }
    public static List<City> GetCities()
    {
        return SqlServerManager.GetBaseCollection<City>("Id>0");
    }
    public static List<City> GetCities(DateTime LastSyncDate)
    {
        if (LastSyncDate == DateTime.MinValue)
            LastSyncDate = new DateTime(1900, 1, 1);

        return SqlServerManager.GetBaseCollection<City>("LastSyncDate > CONVERT(DATETIME, '" + LastSyncDate.ToString("dd/MM/yyyy HH:mm:ss.fff tt") + "',103)");
    }
    public static StatusDTO GetFirstAvailableStaus()
    {
        StatusDTO result = new StatusDTO();
        try
        {
            var status = SqlServerManager.GetBaseCollection<Status>("IdStatusDescription=1").OrderByDescending(x => x.LastSyncDate).FirstOrDefault();
            if (status == null)
                throw new Exception("None Docker is avalable");
            result.DockerId = status.DockerId;
            result.DeviceIp = status.DeviceIp;
            result.IdStatusDescription = status.IdStatusDescription;
            result.Message = status.Message;
            result.LastSyncDate = status.LastSyncDate;
            result.ActionResult = 200;
        }
        catch (Exception ex)
        {
            result.ActionResult = 400;
            result.ActionError = ex.Message;
        }
        return result;
    }
    #endregion

    #region app
    public static List<Nation> GetNations()
    {
        return SqlServerManager.GetNations();
    }
    public static SyncCompanyResponseDTO SyncCompanies(SyncCompanyTokenDTO token)
    {
        SyncCompanyResponseDTO result = new SyncCompanyResponseDTO();
        try
        {

            //DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0);
            var milliseconds = ServerUtil.GetUnixFormatDateTime(token.LastSyncDate);
            var syncResult = GremlinManager.Instance.RetreiveData<Company>($"g.V().hasLabel('company').has('LastSyncDate',gt({milliseconds}))").Result;
            if (syncResult != null && syncResult.Any())
            {
                syncResult.ForEach(x =>
                {
                    //var dto = MapperManager.Map<Company, CompanyDTO>(x);
                    var dto = MapperConfigurator.ConvertCompany(x);
                    result.CompanyDTOList.Add(dto);
                });

            }
            result.ActionResult = 200;
        }
        catch (Exception ex)
        {
            result.ActionResult = 400;
            result.ActionError = ex.Message;
        }
        return result;
    }

    public static SyncEventResponseDTO SyncEvents(SyncEventTokenDTO token)
    {
        SyncEventResponseDTO result = new SyncEventResponseDTO();
        try
        {

            //if (token.CompanyID)
            var milliseconds = ServerUtil.GetUnixFormatDateTime(token.LastSyncDate);
            var syncResult = GremlinManager.Instance.FreeQuery($"g.V().hasLabel('event').has('LastSyncDate',gt({milliseconds})).as('Event').out('organized_by').as('Company').select('Event','Company')");
            if (syncResult.Count > 0)
            {
                foreach (var res in syncResult)
                {
                    // The vertex results are formed as Dictionaries with a nested dictionary for their properties
                    Dictionary<string, object> list = (Dictionary<string, object>)res;
                    //IEnumerable<KeyValuePair<string, object>> valueList = (IEnumerable<KeyValuePair<string, object>>)list.Values.ToList()[0];
                    //var Event = GremlinManager.Instance.CreateIstance<Event>((IEnumerable<KeyValuePair<string, object>>)list.Values.ToList()[0]);
                    //var Company = GremlinManager.Instance.CreateIstance<Company>((IEnumerable<KeyValuePair<string, object>>)list.Values.ToList()[1]);
                    //var dto = MapperConfigurator.ConvertEvent(Event, Company.id);
                    //result.EventDTOList.Add(dto);

                }
            }
            result.ActionResult = 200;
        }
        catch (Exception ex)
        {
            result.ActionResult = 400;
            result.ActionError = ex.Message;
        }
        return result;
    }

    public static UserDTO RegisterUser(UserDTO usDTO)
    {
        UserDTO result = new UserDTO();
        try
        {
            var syncResult = GremlinManager.Instance.RetreiveData<User>($"g.V().hasLabel('user').has('Email','{usDTO.Email}')").Result;
            if (syncResult != null && syncResult.Any())
                throw new Exception("User already exists");
            else
            {
                var us = MapperConfigurator.ConvertUserDTO(usDTO);
                var usResult = GremlinManager.Instance.SaveVertex<User>(us);
                if (usResult == null)
                    throw new Exception("Error on saving to graphdb");
                else
                    result = MapperConfigurator.ConvertUser(usResult);
            }
            result.ActionResult = 200;
        }
        catch (Exception ex)
        {
            result.ActionResult = 400;
            result.ActionError = ex.Message;
        }
        return result;
    }
    public static User LoginUser(string email, string password)
    {
        User result = null;
          var syncResult = GremlinManager.Instance.RetreiveData<User>($"g.V().hasLabel('user').has('Email','{email.ToLower()}').has('Password','{password}')").Result;
            if (syncResult != null && syncResult.Any())
            {
            result = syncResult.First();
             
            }
            else
                throw new Exception("User null");

        
        return result;
    }
    #endregion

    #region Web

    public static async Task<CompanyResponse> GetCompaniesList(FilterCompany filter)
    {
        CompanyResponse result = new CompanyResponse();
        int startIndex = filter.PageSize * filter.Offset;
        int endIndex = startIndex + filter.PageSize;
        string query = $"g.V().hasLabel('company')";
        if (!String.IsNullOrWhiteSpace(filter.Where))
        {
            query += $".or(__.has('Name', containing('{filter.Where}')), __.has('Street', containing('{filter.Where}')))";
        }
        string queryCount = query + ".count()";
        query += $".range({startIndex},{endIndex})";
        var items = await GremlinManager.Instance.RetreiveData<Company>(query);
        if (items != null && items.Any())
        {
            result.CompanyList.AddRange(items);
            int count = (int)GremlinManager.Instance.Count(queryCount);
            result.ItemsCount = count;
        }
        return result;
    }
    /*  public static async Task<CompanyResponse> GetCompaniesList(FilterCompany filter)
      {
          CompanyResponse result = new CompanyResponse();
          int startIndex = filter.PageSize * filter.Offset;

          string query = String.Empty;
          if (!String.IsNullOrWhiteSpace(filter.Where))
          {
              query = $"Name Like '%{filter.Where}%'";
          }
          var items = await CosmosManager.Instance.RetreiveData<Company>(query,startIndex,filter.PageSize);
          if (items != null && items.Any())
          {
              result.CompanyList.AddRange(items);
              int count = (int) await CosmosManager.Instance.Count<Company>(query);
              result.ItemsCount = count;
          }
          return result;
      }*/

    public static Company SaveCompany(Company company)
    {
        return GremlinManager.Instance.SaveVertex<Company>(company);
    }

    public static async Task<UserResponse> GetUsersList(FilterUser filter)
    {
        UserResponse result = new UserResponse();
        int startIndex = filter.PageSize * filter.Offset;
        int endIndex = startIndex + filter.PageSize;
        string query = $"g.V().hasLabel('user').range({startIndex},{endIndex})";
        if (!String.IsNullOrWhiteSpace(filter.Where))
        {

        }
        var items = await GremlinManager.Instance.RetreiveData<User>(query);
        if (items != null && items.Any())
        {
            result.UserList.AddRange(items);
            int count = (int)GremlinManager.Instance.Count("g.V().hasLabel('user').count()");
            result.ItemsCount = count;
        }
        return result;
    }

    public static User SaveUser(User user)
    {
        return GremlinManager.Instance.SaveVertex<User>(user);
    }

    public static User LoginFacebook(string accessToken, string userID)
    {
        User result = null;

        var fbUser = FacebookManager.Instance.GetUserData(accessToken, userID);
        if (fbUser == null)
            throw new Exception("User Facebook null");
        //  dynamic parameters = new ExpandoObject();
        //parameters.message = txtSearch;
        // dynamic resultCall = fb.Get("search?q=Milan&fields=name,location,link", parameters);
        var userResult = GremlinManager.Instance.RetreiveData<User>($"g.V().hasLabel('user').has('id','{fbUser.id.ToLower()}')").Result;
        if (userResult != null && userResult.Any())
        {
            result = userResult.First();
        }
        else
        {
            //insert user
            fbUser.Disabled = false;
            fbUser.RegistrationDate = ServerUtil.GetUnixFormatDateTime(DateTime.UtcNow,TimestampFormatter.Seconds);
            fbUser.UserActivated = true;
            fbUser.FromFacebook = true;
            result = GremlinManager.Instance.SaveVertex<User>(fbUser, true);
            // claim del locale
          
        }

        var navEdge = GremlinManager.Instance.RetreiveData<User>($"g.V('{result.id}').out('has_claimed')").Result;
        if (navEdge == null || !navEdge.Any())
        {
            var companyResult = GremlinManager.Instance.RetreiveData<User>($"g.V().hasLabel('company').has('id','{result.id.ToLower()}')").Result;
            if (companyResult != null && companyResult.Any())
            {
                // verifico che non siano state già inseriti gli edge


                var cmp = companyResult.First();
                GremlinManager.Instance.AddEdge(result.id, cmp.id, "has_claimed");
                GremlinManager.Instance.AddEdge(cmp.id, result.id, "claims");
            }
        }

        return result;
    }

    public static async Task<EventsResponse> GetEventsList(EventsViewModel viewModel)
    {
        EventsResponse result = new EventsResponse();
        string query = $"g.V().hasLabel('user').has('id', '{viewModel.IdUser}').out('has_claimed').out('organizes')";
        var items = await GremlinManager.Instance.RetreiveData<Event>(query);
        if (items != null && items.Any())
        {
            result.EventsList.AddRange(items);
            result.ItemsCount = items.Count;
        }
        return result;
    }
    //https://developers.facebook.com/products/official-events-api/
    public async static Task<Event> SaveEvent(EventSaveViewModel evenVM)
    {
        //return FacebookManager.Instance.SaveEvent(even);
        //return GremlinManager.Instance.SaveVertex<Event>(even);
        Event result = null;
        string query = $"g.V().hasLabel('user').has('id', '{evenVM.IdUser}').out('has_claimed')";
        var companies = await GremlinManager.Instance.RetreiveData<Company>(query);
        if (companies != null && companies.Any())
        {
            var company = companies[0];
            if (string.IsNullOrWhiteSpace(evenVM.Event.id))
                evenVM.Event.id = Guid.NewGuid().ToString();

            evenVM.Event.LastSyncDate = ServerUtil.GetUnixFormatDateTime(DateTime.UtcNow);
            result = GremlinManager.Instance.SaveVertex<Event>(evenVM.Event);
            GremlinManager.Instance.AddEdge(result.id, company.id, "organized_by");
            GremlinManager.Instance.AddEdge(company.id, result.id, "organizes");
        }
        else throw new Exception("Cannot find the company");

         return result;
    }

    #endregion
}


