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
    [Route("getCompanyList")]
    public async Task<string> GetCompanyList(FilterCompany filter)
    {
        //ViewBag.Message = "Your contact page.";
        try
        {
            var resList =  await BusinessLogic.GetCompaniesList(filter);
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
    public string saveCompany(Company company)
    {
        //ViewBag.Message = "Your contact page.";
        try
        { 
            var cmp = BusinessLogic.SaveCompany(company);
            var resp = JsonConvert.SerializeObject(new TokenWebResponse(cmp, "SaveCompany"));
            return resp;
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "SaveCompany"));
        }

    }
    [HttpPost]
    public string DeleteCompany(Company Company)
    {
        //ViewBag.Message = "Your contact page.";
        try
        {
            //company.Valid = false;
            var cmp = BusinessLogic.SaveCompany(Company);
            var resp = JsonConvert.SerializeObject(new TokenWebResponse(cmp, "DeleteCompany"));
            return resp;
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "DeleteCompany"));
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
    [Route("getEventsList")]
    public async Task<string> getEventsList(EventsViewModel viewmodel)
    {
        //ViewBag.Message = "Your contact page.";
        try
        {
            var resList = await BusinessLogic.GetEventsList(viewmodel);
            var resp = JsonConvert.SerializeObject(new TokenWebResponse(resList, "GetEventsList"));
            return resp;




        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "GetEventsList"));
        }

    }

    [HttpPost]
    [Route("saveEvent")]
    public async Task<string> SaveEvent(EventSaveViewModel evenVm)
    {
        //ViewBag.Message = "Your contact page.";
        try
        {
            var cmp = await BusinessLogic.SaveEvent(evenVm);
            var resp = JsonConvert.SerializeObject(new TokenWebResponse(cmp, "saveEvent"));
            return resp;
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "saveEvent"));
        }

    }
}

