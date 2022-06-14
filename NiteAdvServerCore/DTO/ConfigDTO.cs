using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.DTO
{
    public  class ConfigDTO:BaseDTO
    {
        public int Id { get; set; }
        public string DockerId { get; set; }
        public string Port { get; set; }
        public string MongooseUrl { get; set; }

        public int TickMinutes { get; set; }
    }
}
