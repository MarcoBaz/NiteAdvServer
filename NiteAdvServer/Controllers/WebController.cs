using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NiteAdvServer.ViewModel;
using NiteAdvServerCore.DTO;
using NiteAdvServerCore.Entities;
using NiteAdvServerCore.Generic;
using NiteAdvServerCore.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NiteAdvServer.Controllers;

[ApiController]
[Route("Web")]
public class WebController : ControllerBase
{
    [HttpPost]
    [Route("getlogin")]
    //[ValidateAntiForgeryToken]
    public string GetLogin(LoginViewModel vmLogin)
    {
        try
        {
            var us = BusinessLogic.LoginUser(vmLogin.Login, vmLogin.Password);
            if (us == null)
                throw new Exception("Utente non autorizzato");
         
            return JsonConvert.SerializeObject(new TokenWebResponse(us, "GetLogin"));
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "GetLogin"));
        }

    }
    [HttpPost]
    [Route("authFacebook")]
    //[ValidateAntiForgeryToken]
    public string AuthFacebook(FacebookLoginViewModel login)
    {
        try
        {
       
            var us = BusinessLogic.LoginFacebook(login.AccessToken,login.UserId);
            if (us == null)
                throw new Exception("Utente non autorizzato");
          
            return JsonConvert.SerializeObject(new TokenWebResponse(us, "GetLogin"));
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "GetLogin"));
        }

    }
    

    [HttpPost]
    [Route("getUsersList")]
    public async Task<string> GetUsersList(FilterUser filter)
    {
        //ViewBag.Message = "Your contact page.";
        try
        {
            var resList = await BusinessLogic.GetUsersList(filter);
            var resp = JsonConvert.SerializeObject(new TokenWebResponse(resList, "GetUsersList"));
            return resp;




        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "GetUsersList"));
        }

    }
    [HttpPost]
    [Route("saveUser")]
    public string SaveUser(User user)
    {
        //ViewBag.Message = "Your contact page.";
        try
        {
            var cmp = BusinessLogic.SaveUser(user);
            var resp = JsonConvert.SerializeObject(new TokenWebResponse(cmp, "SaveUser"));
            return resp;
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "SaveUser"));
        }

    }
    [HttpPost]
    [Route("deleteUser")]
    public string DeleteUser(User user)
    {
        //ViewBag.Message = "Your contact page.";
        try
        {
            user.Disabled = true;
            var cmp = BusinessLogic.SaveUser(user);
            var resp = JsonConvert.SerializeObject(new TokenWebResponse(cmp, "DeleteUser"));
            return resp;
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "DeleteUser"));
        }

    }

    [HttpPost]
    [Route("getUserEventsList")]
    public async Task<string> getUserEventsList(EventsViewModel viewmodel)
    {
        //ViewBag.Message = "Your contact page.";
        try
        {
            var resList = await BusinessLogic.GetUserEventsList(viewmodel);
            var resp = JsonConvert.SerializeObject(new TokenWebResponse(resList, "GetEventsList"));
            return resp;




        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "GetEventsList"));
        }

    }

    [HttpPost]
    [Route("saveUserEvent")]
    public async Task<string> SaveUserEvent(EventSaveViewModel evenVm)
    {
        //ViewBag.Message = "Your contact page.";
        try
        {
            var cmp = await BusinessLogic.SaveUserEvent(evenVm);
            var resp = JsonConvert.SerializeObject(new TokenWebResponse(cmp, "saveEvent"));
            return resp;
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "saveEvent"));
        }

    }

    [HttpGet]
    [Route("Test")]
    public string Test()
    {
        //ViewBag.Message = "Your contact page.";
        try
        {
            FilterCompany filter = new FilterCompany();
            filter.Offset = 0;
            filter.PageSize = 10;
            var resList = BusinessLogic.GetCompaniesList(filter).Result;
            var resp = JsonConvert.SerializeObject(new TokenWebResponse(resList, "GetCompanyList"));
            return resp;

        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "GetCompanyList"));
        }

    }
    [HttpPost, DisableRequestSizeLimit]
    [Route("SaveCompanyImage")]
    public async Task<string> SaveCompanyImage() //string vectorViewModel
    {
        //ViewBag.Message = "Your contact page.";

        try
        {

            MultipartParser multiformParsed;

            multiformParsed = new MultipartParser(HttpContext.Request.Body, "IdCompany");
            //using (stream = new MemoryStream())
            //{
            //    await Request.Body.CopyToAsync(stream);

            //}
            Company result = null;
            string IdCompany = multiformParsed.AdditionalObject.ToString();
            //var file = new System.IO.MemoryStream(multiformParsed.FileContents);
            result = await BusinessLogic.SaveImageCompany(multiformParsed.FileContents, IdCompany);

            if (result == null)
                throw new Exception("Error on saving file");

            return JsonConvert.SerializeObject(new TokenWebResponse(result, "SaveCompanyImage"));
        }
        catch (Exception ex)
        {

            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "SaveCompanyImage")); ;
        }
    }
    [HttpPost]
    [Route("getCompanyList")]
    public async Task<string> GetCompanyList(FilterCompany filter)
    {
        //ViewBag.Message = "Your contact page.";
        try
        {
            var resList = await BusinessLogic.GetCompaniesList(filter);
            var resp = JsonConvert.SerializeObject(new TokenWebResponse(resList, "GetCompanyList"));
            return resp;




        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "GetCompanyList"));
        }

    }
    [HttpPost]
    [Route("saveCompany")]
    public string saveCompany(FilterCompany filter)
    {
        //ViewBag.Message = "Your contact page.";
        try
        {
            var cmp = BusinessLogic.SaveCompanyFilter(filter);
            var resp = JsonConvert.SerializeObject(new TokenWebResponse(cmp, "SaveCompany"));
            return resp;
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "SaveCompany"));
        }

    }
    [HttpPost]
    [Route("DeleteCompany")]
    public string DeleteCompany(FilterCompany filter)
    {
        //ViewBag.Message = "Your contact page.";
        try
        {
            var cmp = BusinessLogic.DeleteCompany(filter).Result;
            var resp = JsonConvert.SerializeObject(new TokenWebResponse(cmp, "DeleteCompany"));
            return resp;
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "DeleteCompany"));
        }

    }
    [HttpPost]
    [Route("CompanyBlackList")]
    public string CompanyBlackList(FilterCompany filter)
    {
        //ViewBag.Message = "Your contact page.";
        try
        {
            if (filter.CheckedCompanies == null || !filter.CheckedCompanies.Any())
                throw new Exception("Checked Compamnies null");
            var cmp = BusinessLogic.CompanyBlackList(filter).Result;
            var resp = JsonConvert.SerializeObject(new TokenWebResponse(cmp, "CompanyBlackList"));
            return resp;
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "CompanyBlackList"));
        }

    }
    [HttpPost, DisableRequestSizeLimit]
    [Route("SaveEventImage")]
    public async Task<string> SaveEventImage() //string vectorViewModel
    {
        //ViewBag.Message = "Your contact page.";

        try
        {

            MultipartParser multiformParsed;

            multiformParsed = new MultipartParser(HttpContext.Request.Body, "IdEvent");
            //using (stream = new MemoryStream())
            //{
            //    await Request.Body.CopyToAsync(stream);

            //}
            Event result = null;
            string IdEvent = multiformParsed.AdditionalObject.ToString();
            //var file = new System.IO.MemoryStream(multiformParsed.FileContents);
            result = await BusinessLogic.SaveImageEvent(multiformParsed.FileContents, IdEvent);

            if (result == null)
                throw new Exception("Error on saving file");

            return JsonConvert.SerializeObject(new TokenWebResponse(result, "SaveEventImage"));
        }
        catch (Exception ex)
        {

            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "SaveEventImage")); ;
        }
    }
    [HttpPost]
    [Route("GetEventsList")]
    public async Task<string> GetEventsList(FilterEvent filter)
    {
        //ViewBag.Message = "Your contact page.";
        try
        {
            var resList = await BusinessLogic.GetEventsList(filter);
            var resp = JsonConvert.SerializeObject(new TokenWebResponse(resList, "getEventsList"));
            return resp;




        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "getEventsList"));
        }

    }
    [HttpPost]
    [Route("saveEvent")]
    public string saveEvent(FilterEvent filter)
    {
        //ViewBag.Message = "Your contact page.";
        try
        {
            var cmp = BusinessLogic.SaveEventFilter(filter);
            var resp = JsonConvert.SerializeObject(new TokenWebResponse(cmp, "saveEvent"));
            return resp;
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "saveEvent"));
        }

    }
    [HttpPost]
    [Route("DeleteEvent")]
    public string DeleteEvent(FilterEvent filter)
    {
        //ViewBag.Message = "Your contact page.";
        try
        {
            var cmp = BusinessLogic.DeleteEvent(filter).Result;
            var resp = JsonConvert.SerializeObject(new TokenWebResponse(cmp, "DeleteEvent"));
            return resp;
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "DeleteEvent"));
        }

    }
    [HttpPost]
    [Route("EventsBlackList")]
    public string EventsBlackList(FilterEvent filter)
    {
        //ViewBag.Message = "Your contact page.";
        try
        {
            if (filter.CheckedEvents == null || !filter.CheckedEvents.Any())
                throw new Exception("Checked Events null");
            var cmp = BusinessLogic.EventBlackList(filter).Result;
            var resp = JsonConvert.SerializeObject(new TokenWebResponse(cmp, "EventsBlackList"));
            return resp;
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "EventsBlackList"));
        }

    }
    [HttpPost]
    [Route("SendContact")]
    //[ValidateAntiForgeryToken]
    public string SendContact(ContactViewModel vmContact)
    {
        try
        {
            var us = BusinessLogic.Contact(vmContact);


            return JsonConvert.SerializeObject(new TokenWebResponse(us, "SendContact"));
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "SendContact"));
        }

    }
}

