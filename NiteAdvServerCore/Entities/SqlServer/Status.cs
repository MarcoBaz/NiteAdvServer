using NiteAdvServerCore.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.Entities
{
    public class Status : BaseEntity
    {

        public Status()
        {
        }
        public int IdConfig { get; set; }
        public int IdStatusDescription { get; set; }
        public string DeviceIp { get; set; }
        public string Message { get; set; }
        public string City { get; set; }
        public DateTime LastSyncDate { get; set; }

        [Ignore]
        public string DockerId { 
            get {
                return SqlServerManager.GetBaseEntity<Config>("Id=" + IdConfig).DockerId;
            } 
        }
    }
}
