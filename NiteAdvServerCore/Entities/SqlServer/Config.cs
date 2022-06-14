using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.Entities
{
    public class Config : BaseEntity
    {

        public Config()
        {
        }

        public string DockerId { get; set; }
        public string Port { get; set; }
        public string MongooseUrl { get; set; }
        public int TickMinutes { get; set; }
        public DateTime LastSyncDate { get; set; }

    }
}
