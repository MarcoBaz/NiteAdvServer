using AutoMapper;
using Microsoft.IdentityModel.Tokens;
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

            var existingStatus = SqlServerManager.GetBaseEntity<Status>("IdConfig=" + config.Id + " and City='" + statusDTO.City + "'");
            if (existingStatus != null)
            {
                SqlServerManager.FreeQuery<Status>("update Status set IdStatusDescription=" + statusDTO.IdStatusDescription + " ,DeviceIP='" + statusDTO.DeviceIp + "',Message='" + statusDTO.Message + "',LastSyncDate=CONVERT(DATETIME, '" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff tt") + "',103) where Id=" + existingStatus.Id);
                id = existingStatus.Id;
                var EntityResult = SqlServerManager.GetBaseEntity<Status>("Id=" + id);
                if (EntityResult == null)
                    throw new Exception("error on insert or update");
                result.DockerId = statusDTO.DockerId;
                result.IdStatusDescription = EntityResult.IdStatusDescription;
                result.DeviceIp = EntityResult.DeviceIp;
                result.Message = EntityResult.Message;
                result.City = EntityResult.City;
                result.LastSyncDate = EntityResult.LastSyncDate;


                result.ActionResult = 200;
                return result;
            }
            else
                throw new Exception("Cannot find staus row in datatabase");


            //var EntityResult = SqlServerManager.GetBaseEntity<Status>("Id=" + statusDTO.Id);
           

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

            var status = SqlServerManager.GetBaseEntity<Status>("IdConfig=" + config.Id + " and IdStatusDescription=2");
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
    public static StatusDTO GetFirstAvailableStatus(int IdConfig, string city)
    {
        StatusDTO result = new StatusDTO();
        Config conf= SqlServerManager.GetBaseEntity<Config>($"Id={IdConfig}");
        try
        {
            var status = SqlServerManager.GetBaseEntity<Status>($"City='{city}' and IdConfig={IdConfig}");
            if (status == null || status.IdStatusDescription !=1)
                throw new Exception($"Docker for city={city} and IdConfig={IdConfig} is not available");
            result.DockerId = status.DockerId;
            result.DeviceIp = status.DeviceIp;
            result.IdStatusDescription = status.IdStatusDescription;
            result.Message = status.Message;
            result.LastSyncDate = status.LastSyncDate;
            result.ActionResult = 200;
        }
        catch (Exception ex)
        {
            Logger.Write(conf.DockerId, ex.Message,"Error");
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
    #region company
    public static async Task<CompanyResponse> GetCompaniesList(FilterCompany filter)
    {
        CompanyResponse result = new CompanyResponse();
        int startIndex = filter.PageSize * filter.Offset;
        int endIndex = startIndex + filter.PageSize;
        string query = $"g.V().hasLabel('company')";
        if (!String.IsNullOrWhiteSpace(filter.City))
        {
            if (filter.City != "World Wide")
               query += $".has('PlaceSearch','{filter.City}')";
        }
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

    public static CompanyResponse SaveCompanyFilter(FilterCompany filter)
    {

        var cmpResult = SaveCompany(filter.CompanyToSave);
        if (cmpResult == null)
            throw new Exception("Error on saving company");
        return GetCompaniesList(filter).Result;
    }
    private static Company SaveCompany(Company cmp)
    {
        //output.Replace("'", "\\'"))
        cmp.GoogleTypes = !String.IsNullOrWhiteSpace(cmp.GoogleTypes) ? cmp.GoogleTypes.Replace("\'", "\\'") : String.Empty;
        cmp.Reviews = !String.IsNullOrWhiteSpace(cmp.Reviews) ? cmp.Reviews.Replace("\'", "\\'") : String.Empty;
        var cmpResult = GremlinManager.Instance.SaveVertex<Company>(cmp);
        if (cmpResult == null)
            throw new Exception("Error on saving company");
        return cmpResult;
    }
    public static async Task<Company> SaveImageCompany(byte[] image, string IdCompany)
    {
        Company company = null;
        string query = $"g.V().hasLabel('company').has('id', '{IdCompany}')";
        var items = await GremlinManager.Instance.RetreiveData<Company>(query);
        if (items != null && items.Any())
        {
            company = items[0];
            var imageUrl = AzureBlobManager.SaveImage(image,AzureBlobManager.ImageContext.Company).Result;
            if (String.IsNullOrWhiteSpace(imageUrl))
                throw new Exception("Error saving in blob storage");

            company.ImageUrl = imageUrl;
            company = SaveCompany(company);
        }
        else
            throw new Exception("Undefined Company");


        return company;
    }
    public static async Task<CompanyResponse> CompanyBlackList(FilterCompany filter)
    {
        foreach (var company in filter.CheckedCompanies)
        {
            string query = $"g.V().hasLabel('company').has('id', '{company.id}')";
            var items = await GremlinManager.Instance.RetreiveData<Company>(query);
            if (items != null && items.Any())
            {
                var cmp = items[0];
                company.IsInBlackList = !cmp.IsInBlackList;
                SaveCompany(company);
            }
            else
                throw new Exception("error on find company");
        }
        return await GetCompaniesList(filter);
    }
    public static async Task<CompanyResponse> DeleteCompany(FilterCompany filter)
    {
        var company = filter.CompanyToSave;
        string query = $"g.V().hasLabel('company').has('id', '{company.id}')";
        var items = await GremlinManager.Instance.RetreiveData<Company>(query);
        if (items != null && items.Any())
        {
            var cmp = items[0];
            company.Deleted = !cmp.Deleted;
            company = SaveCompany(company);
        }
        else
            throw new Exception("error on find company");
        return await GetCompaniesList(filter);
    }
    #endregion
    #region events
    public static async Task<EventResponse> GetEventsList(FilterEvent filter)
    {
        EventResponse result = new EventResponse();
        int startIndex = filter.PageSize * filter.Offset;
        int endIndex = startIndex + filter.PageSize;
        string query = $"g.V().hasLabel('event')";
        if (!String.IsNullOrWhiteSpace(filter.City))
        {
            if (filter.City != "World Wide")
                query += $".out('organized_by').has('PlaceSearch', '{filter.City}').out('organizes')";
        }
        if (!String.IsNullOrWhiteSpace(filter.Where))
        {
            query += $".or(__.has('Name', containing('{filter.Where}')), __.has('Description', containing('{filter.Where}')))";
        }
        string queryCount = query + ".count()";
        query += $".range({startIndex},{endIndex})";
        var items = await GremlinManager.Instance.RetreiveData<Event>(query);
        if (items != null && items.Any())
        {
            result.EventList.AddRange(items);
            int count = (int)GremlinManager.Instance.Count(queryCount);
            result.ItemsCount = count;
        }
        return result;
    }

    public static EventResponse SaveEventFilter(FilterEvent filter)
    {

        var cmpResult = SaveEvent(filter.EventToSave);
        if (cmpResult == null)
            throw new Exception("Error on saving company");
        return GetEventsList(filter).Result;
    }
    private static Event SaveEvent(Event eve)
    {
        //output.Replace("'", "\\'"))
        // cmp.GoogleTypes = !String.IsNullOrWhiteSpace(cmp.GoogleTypes) ? cmp.GoogleTypes.Replace("\'", "\\'") : String.Empty;
        eve.Description = !String.IsNullOrWhiteSpace(eve.Description) ? eve.Description.Replace("\'", "\\'") : String.Empty;
        eve.Place = !String.IsNullOrWhiteSpace(eve.Place) ? eve.Place.Replace("\'", "\\'") : String.Empty;
        eve.Name = !String.IsNullOrWhiteSpace(eve.Name) ? eve.Name.Replace("\'", "\\'") : String.Empty;
        var cmpResult = GremlinManager.Instance.SaveVertex<Event>(eve);
        if (cmpResult == null)
            throw new Exception("Error on saving company");
        return cmpResult;
    }
    public static async Task<Event> SaveImageEvent(byte[] image, string IdEvent)
    {
        Event even = null;
        string query = $"g.V().hasLabel('event').has('id', '{IdEvent}')";
        var items = await GremlinManager.Instance.RetreiveData<Event>(query);
        if (items != null && items.Any())
        {
            even = items[0];
            var imageUrl = AzureBlobManager.SaveImage(image,AzureBlobManager.ImageContext.Event).Result;
            if (String.IsNullOrWhiteSpace(imageUrl))
                throw new Exception("Error saving in blob storage");

            even.Image = imageUrl;
            even = SaveEvent(even);
        }
        else
            throw new Exception("Undefined Company");


        return even;
    }
    public static async Task<EventResponse> EventBlackList(FilterEvent filter)
    {
        foreach (var eve in filter.CheckedEvents)
        {
            string query = $"g.V().hasLabel('event').has('id', '{eve.id}')";
            var items = await GremlinManager.Instance.RetreiveData<Event>(query);
            if (items != null && items.Any())
            {
                var cmp = items[0];
                eve.IsInBlackList = !cmp.IsInBlackList;
                SaveEvent(eve);
            }
            else
                throw new Exception("error on find company");
        }
        return await GetEventsList(filter);
    }
    public static async Task<EventResponse> DeleteEvent(FilterEvent filter)
    {
        var eve = filter.EventToSave;
        string query = $"g.V().hasLabel('event').has('id', '{eve.id}')";
        var items = await GremlinManager.Instance.RetreiveData<Event>(query);
        if (items != null && items.Any())
        {
            var cmp = items[0];
            eve.Deleted = !cmp.Deleted;
            eve = SaveEvent(eve);
        }
        else
            throw new Exception("error on find company");
        return await GetEventsList(filter);
    }
    #endregion
    #region users
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
        var companyResult = GremlinManager.Instance.RetreiveData<User>($"g.V().hasLabel('company').has('id','{userID.ToLower()}')").Result;
        if (companyResult == null || !companyResult.Any())
            throw new Exception("Sembra che non ci siano dati riguardanti il tuo locale nella base dati, contatta gli amministratori per la verifica");

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
            fbUser.RegistrationDate = ServerUtil.GetUnixFormatDateTime(DateTime.UtcNow, TimestampFormatter.Seconds);
            fbUser.UserActivated = true;
            fbUser.FromFacebook = true;
            result = GremlinManager.Instance.SaveVertex<User>(fbUser, true);
            // claim del locale

        }

        var navEdge = GremlinManager.Instance.RetreiveData<User>($"g.V('{result.id}').out('has_claimed')").Result;
        if (navEdge == null || !navEdge.Any())
        {
            var companyRes = GremlinManager.Instance.RetreiveData<User>($"g.V().hasLabel('company').has('id','{result.id.ToLower()}')").Result;
            if (companyRes != null && companyRes.Any())
            {
                // verifico che non siano state già inseriti gli edge


                var cmp = companyRes.First();
                GremlinManager.Instance.AddEdge(result.id, cmp.id, "has_claimed");
                GremlinManager.Instance.AddEdge(cmp.id, result.id, "claims");
            }
        }

        return result;
    }

   

    #endregion
    #region user events
    //https://developers.facebook.com/products/official-events-api/
    public async static Task<Event> SaveUserEvent(EventSaveViewModel evenVM)
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

    public static async Task<EventsResponse> GetUserEventsList(EventsViewModel viewModel)
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
    #endregion


    public static Boolean Contact(ContactViewModel contact)
    {
        


        return true;
    }
    #endregion
}


