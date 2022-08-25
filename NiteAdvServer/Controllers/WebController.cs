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
            UserDTO result = null;
            var us = BusinessLogic.LoginUser(vmLogin.Login, vmLogin.Password);
            if (us == null)
                throw new Exception("Utente non autorizzato");
            else
                result = us;
            return JsonConvert.SerializeObject(new TokenWebResponse(result, "GetLogin"));
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new TokenWebResponse(ex.Message, "GetLogin"));
        }

    }
    [HttpPost]
    [Route("getCompanyList")]
    public string GetCompanyList(FilterCompany filter)
    {
        //ViewBag.Message = "Your contact page.";
        try
        {
            var resList = BusinessLogic.GetCompaniesList(filter);
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
    public string saveCompany(string company)
    {
        //ViewBag.Message = "Your contact page.";
        try
        { 
            var cmp = BusinessLogic.SaveCompany(JsonConvert.DeserializeObject<Company>(company));
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
}

