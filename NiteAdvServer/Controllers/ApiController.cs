
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NiteAdvServer.ViewModel;
using NiteAdvServerCore.DTO;
using NiteAdvServerCore.DTO.Token;
using NiteAdvServerCore.Entities;
using NiteAdvServerCore.Managers;
using NiteAdvServerCore.Util;

namespace NiteAdvServer.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ApiController:Controller
	{
        #region Private Methods
        protected string BearerToken
        {
            get
            {
                return "08d652aa-2529-44c3-af17-180094336911";
            }
        }
        private bool checkAuthentication()
        {
            bool result = false;
            string authString = HttpContext.Request.Headers["Authorization"];
            if (!String.IsNullOrWhiteSpace(authString))
            {
                var splittedAuth = authString.Split(' ');
                result = splittedAuth[1] == BearerToken ? true : false;
            }
            return result;
        }
        #endregion

        #region Crawler methos
       
        [HttpPost]
        [Route("SaveStatus")]
        public string SaveStatus(StatusDTO statusDTO)
        {
            if (checkAuthentication())
            {
                var cart = BusinessLogic.SaveStatus(statusDTO);
                return JsonConvert.SerializeObject(cart);

            }
            else
                return "Authentication Failed";

        }
        [HttpGet]
        [Route("GetStatus")]
        public string GetStatus(string DockerId)
        {
            if (checkAuthentication())
            {
                var status = BusinessLogic.GetStatus(DockerId);
                return JsonConvert.SerializeObject(status);

            }
            else
                return "Authentication Failed";

        }

        [HttpPost]
        [Route("SaveLog")]
        public string SaveLog(LogDTO logDTO)
        {
            if (checkAuthentication())
            {
                Logger.Write(logDTO.DockerID, logDTO.Message, logDTO.LogType);
                return JsonConvert.SerializeObject("true");

            }
            else
                return "Authentication Failed";

        }
        [HttpGet]
        [Route("GetConfig")]
        public string GetConfig(string DockerId)
        {
            if (checkAuthentication())
            {
                var config = BusinessLogic.GetConfig(DockerId);
                return JsonConvert.SerializeObject(config);

            }
            else
                return "Authentication Failed";

        }
        [HttpGet]
        [Route("GetCities")]
        public string GetCities()
        {
            if (checkAuthentication())
            {
                var cities = BusinessLogic.GetCities();
                return JsonConvert.SerializeObject(cities);

            }
            else
                return "Authentication Failed";

        }
        [HttpGet]
        [Route("GetFirstAvailableStatus")]
        public string GetFirstAvailableStatus()
        {
            if (checkAuthentication())
            {
                var status = BusinessLogic.GetFirstAvailableStaus();
                return JsonConvert.SerializeObject(status);

            }
            else
                return "Authentication Failed";

        }
        #endregion
        #region App Methods
        [HttpPost]
        [Route("SyncCompanies")]
        public SyncCompanyResponseDTO SyncCompanies(SyncCompanyTokenDTO token)
        {
            if (checkAuthentication())
            {
                var res = BusinessLogic.SyncCompanies(token);
                return res;

            }
            else
            {
                return new SyncCompanyResponseDTO() { ActionResult = 400, ActionError = "Authentication Failed" };
            }

        }
        [HttpPost]
        [Route("SyncEvents")]
        public SyncEventResponseDTO SyncEvents(SyncEventTokenDTO token)
        {
            if (checkAuthentication())
            {
                var res = BusinessLogic.SyncEvents(token);
                return res;

            }
            else
                return new SyncEventResponseDTO() { ActionResult = 400, ActionError = "Authentication Failed" };

        }
        [HttpPost]
        [Route("Nations")]
        public List<Nation> nations()
        {
            if (checkAuthentication())
            {
                return BusinessLogic.GetNations();
            }
            else
                return new List<Nation>();

        }
        [HttpPost]
        [Route("GetCitiesMobile")]
        public List<City> GetCitiesMobile(SyncCityTokenDTO token)
        {
            if (checkAuthentication())
            {
                var cities = BusinessLogic.GetCities(token.LastSyncDate);
                return cities;

            }
            else
                return new List<City>();

        }
    
        [HttpPost]
        [Route("RegisterUser")]
        public UserDTO RegisterUser(UserDTO token)
        {
            if (checkAuthentication())
            {
                var res = BusinessLogic.RegisterUser(token);
                return res;

            }
            else
                return new UserDTO() { ActionResult = 400, ActionError = "Authentication Failed" };

        }
        [HttpPost]
        [Route("LoginUser")]
        public UserDTO LoginUser(LoginDTO token)
        {
            if (checkAuthentication())
            {
                var res = BusinessLogic.LoginUser(token.Email,token.Password);
                return MapperConfigurator.ConvertUser(res);

            }
            else
                return new UserDTO() { ActionResult = 400, ActionError = "Authentication Failed" };

        }
        #endregion
        #region Test
        [HttpGet]
        [Route("Test")]
        public List<CompanyDTO> Test()
        {
            SyncCompanyTokenDTO token = new SyncCompanyTokenDTO();
            token.LastSyncDate = new DateTime(1970, 1, 1, 0, 0, 0);// LastSyncCompany;
            token.Region = "Milano";
            var res = BusinessLogic.SyncCompanies(token);

            return res.CompanyDTOList;
        }
       
        #endregion
    }
}

