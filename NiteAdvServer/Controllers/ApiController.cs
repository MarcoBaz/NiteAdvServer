
using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NiteAdvServerCore.DTO;
using NiteAdvServerCore.Managers;

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
    }
}

