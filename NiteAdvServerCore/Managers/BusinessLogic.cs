using NiteAdvServerCore.DTO;
using NiteAdvServerCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.Managers
{
    public static  class BusinessLogic
    {

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
            StatusDTO result= new StatusDTO();
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
        
    }
}
